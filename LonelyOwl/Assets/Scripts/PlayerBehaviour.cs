using UnityEngine;
using UnityEngine.UIElements; 
using UnityEngine.SceneManagement;
//using static UnityEditor.Searcher.SearcherWindow.Alignment; //Problem
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using System.Collections.Generic;
using System;
using UnityEditor;

using System.IO;
using System.Linq;
//using UnityEditor.SceneManagement; //Problem
using System.Collections;
using UnityEngine.Audio;
using TMPro;

public class PlayerBehaviour : MonoBehaviour
{
    public CharacterController characterController;
    public Transform cam;

    public GameObject mainCam;
    public GameObject otherCam;
    public Transform lookAtPoint;
    

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;

    public bool shouldSing = false;

    public List<AudioClip> hootClips = new List<AudioClip>();
    public AudioClip defaultHoot;

    float turnSmoothVelocity;
    float vertical;
    float horizontal;

    RaycastHit slopeHit;

    bool canPlayerMove = true;
    bool void2Breathing = false;

    NPCBehaviour nPCBehaviour;
    new Rigidbody rigidbody;


    [SerializeField] TextMeshProUGUI billboard;
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepHeight = 2.5f;
    [SerializeField] float stepSmooth = 0.2f;

    [SerializeField] Stage3EventHandler stage3EventHandler;
    [SerializeField] Animator animator;

    private class HootNote : IComparable
    {
        public HootNote(string name, AudioClip clip)
        {
            Name = name;
            Clip = clip;
        }

        public string Name { get; }
        public AudioClip Clip { get; }

        public int Octave
        {
            get
            {
                return int.Parse(Name[Name.Length - 1].ToString());
            }
        }

        public int CompareTo(object obj)
        {
            var o = obj as HootNote;

            var notes = new List<string> { "Gb", "Ab", "Bb", "Db",  "Eb" };
            var noteOrder = new List<string>();

            int octave = 1;
            do
            {
                for (int j = 0; j < notes.Count; j++)
                {
                    if (notes[j] == "Db") octave++;

                    noteOrder.Add(notes[j] + octave.ToString());
                }
            } while (octave < 5);

            return noteOrder.IndexOf(Name).CompareTo(noteOrder.IndexOf(o.Name)); 
        }
    }
    private List<HootNote> playerHootNotes = new List<HootNote>();
    private List<HootNote> bigOwlHootNotes = new List<HootNote>();
    private AudioSource playerAudioSource;
    private bool playerIsPlayingThroughScale = true;
    private int playerScaleNotesPlayed = 0;
    private HootNote lastNotePlayed;
    private List<float> timesPlayerScaleNotesPlayed = new List<float>();

    private bool bigOwlReadyToSing = false;
    public AudioSource BigOwlAudioSource;

    public void Start()
    {
        nPCBehaviour = FindAnyObjectByType<NPCBehaviour>();
        rigidbody = GetComponent<Rigidbody>();
        stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x,
            stepHeight, stepRayUpper.transform.position.z);


        //var hootFiles = Directory.GetFiles("Assets\\Sounds\\Generic_Hoots", "*.wav", SearchOption.TopDirectoryOnly);

        foreach (var clip in hootClips)
        {
            // Turns out AudioClip has a name property lol
            //var name = f.Split("\\")[3].Replace(".wav", "");
            //var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(f);
            //Debug.Log(f);
            //var clip = Resources.Load<AudioClip>(f);
            var hoot = new HootNote(clip.name, clip);

            if (hoot.Octave >= 3)
            {
                playerHootNotes.Add(hoot);
            }
            else
            {
                bigOwlHootNotes.Add(hoot);
            }
        }
        playerHootNotes.Sort();
        bigOwlHootNotes.Sort();

        playerAudioSource = GetComponent<AudioSource>() == null ? gameObject.AddComponent<AudioSource>() : GetComponent<AudioSource>();

        // TODO: this should be set to the audiosource of the big owl. Adding on Player for now.
        BigOwlAudioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (canPlayerMove)
        {
            Movement();
            StepClimb();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnHoot();  
        }

        //Adjust billboard to face camera
        if (billboard.enabled)
        {
            billboard.transform.LookAt(lookAtPoint.position);
            billboard.transform.RotateAround(billboard.transform.position, billboard.transform.up, 180f);
        }
    }

    //public void OnHoot(InputAction.CallbackContext context)
    public void OnHoot()
    {
        if (void2Breathing)
        {
            return;
        }

        if (!shouldSing)
        {
            playerAudioSource.PlayOneShot(defaultHoot);
            return;
        }

       /* if (!context.performed)
        {
            return;
        }

        if (context.interaction is PressInteraction)
        {*/
            AudioClip c = null;
            if (playerIsPlayingThroughScale)
            {
                c = playerHootNotes[playerScaleNotesPlayed].Clip;
                timesPlayerScaleNotesPlayed.Add(Time.realtimeSinceStartup);
                playerScaleNotesPlayed++;
                playerIsPlayingThroughScale = playerScaleNotesPlayed < playerHootNotes.Count;
                if (!playerIsPlayingThroughScale)
                {
                    // Start big owl singing
                    // Call out to stageManager
                    if (stage3EventHandler != null)
                        stage3EventHandler.owlEmerge = true;

                    float initialDelay = 1.0f;
                    float currentDelay = 0.0f;
                    StartCoroutine(playBigOwlSoundWithDelay(bigOwlHootNotes[0].Clip, initialDelay, false));
                    for (int i = 1; i < timesPlayerScaleNotesPlayed.Count; i++)
                    {
                        currentDelay += timesPlayerScaleNotesPlayed[i] - timesPlayerScaleNotesPlayed[i - 1];
                        StartCoroutine(playBigOwlSoundWithDelay(bigOwlHootNotes[i].Clip, initialDelay + currentDelay, i == timesPlayerScaleNotesPlayed.Count - 1));
                    }
                }
            }
            else if(bigOwlReadyToSing)
            {
                HootNote hootNote;
                if(lastNotePlayed != null)
                {
                    do
                    {
                        hootNote = playerHootNotes[UnityEngine.Random.Range(0, playerHootNotes.Count)];
                    } while (hootNote.Name == lastNotePlayed.Name);

                }
                else
                {
                    hootNote = playerHootNotes[UnityEngine.Random.Range(0, playerHootNotes.Count)];
                }
                c = hootNote.Clip;
                lastNotePlayed = hootNote;

                var bassHootNote = bigOwlHootNotes[UnityEngine.Random.Range(0, bigOwlHootNotes.Count)];
                playerAudioSource.PlayOneShot(bassHootNote.Clip);
            }
            if (c != null) {
                playerAudioSource.PlayOneShot(c);
            }


        //}
    }

    IEnumerator playBigOwlSoundWithDelay(AudioClip clip, float delay, bool isLastInScale)
    {

        //Before singing, do it here
        

        yield return new WaitForSeconds(delay);
        BigOwlAudioSource.PlayOneShot(clip);
        if (isLastInScale)
        {
            bigOwlReadyToSing = true;
        }
        //When start singing
    }

    //Switch camera based on interaction
    public void CheckCamera()
    {
        if (nPCBehaviour.IsInteracted())
        {
            mainCam.SetActive(false);
            otherCam.SetActive(true);           
            cam = otherCam.transform;
        }
        if (otherCam && !nPCBehaviour.IsInteracted())
        {
            otherCam.SetActive(false);
            mainCam.SetActive(true);
            cam = mainCam.transform;
        }
    }

    public bool CanPlayerMove()
    {
        return canPlayerMove;
    }

    public void SetPlayerMovable(bool canPlayerMove)
    {
        this.canPlayerMove = canPlayerMove;
    }

    public void SetVoid2Breathing(bool breathing)
    {
        this.void2Breathing = breathing;
    }

    public void ActivatePlayerBillboard(string newText)
    {
        billboard.text = newText;
        billboard.enabled = true;
    }

    public void DeactivatePlayerBillboard()
    {
        billboard.enabled = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("NextScene") && nPCBehaviour.IsInteracted())
        {
            //Go down staris
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("NextScene") && nPCBehaviour.IsInteracted())
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (currentScene > 2)
            {
                currentScene = 0;
            }
            nPCBehaviour.ResetIsInteracted();
            CheckCamera();
            //ToDo
            //SceneManager.LoadSceneAsync(currentScene + 1);
        }
    }

    void StepClimb()
    {
        // Step Up Logic
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position,
                            transform.TransformDirection(Vector3.forward),
                            out hitLower, 0.1f))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position,
                                 transform.TransformDirection(Vector3.forward),
                                 out hitUpper, 0.2f))
            {
                // Step up only if not on a slope (optional condition)
                if (!IsOnSlope())
                {
                    rigidbody.position += new Vector3(0f, stepSmooth, 0f);
                }
            }
        }

        // Downward step handling for stairs
        RaycastHit hitLowerDown;
        if (Physics.Raycast(transform.position + new Vector3(0, -stepHeight, 0),
                            Vector3.down, out hitLowerDown, 0.5f))
        {
            // Step down only if not already grounded or on a slope
            if (hitLowerDown.distance > stepHeight && !IsOnSlope())
            {
                rigidbody.position -= new Vector3(0f, stepSmooth, 0f);
            }
        }

        // Handle stepping at angles
        StepAtAngle(2.5f, 0, 2);  // Right 45-degree check
        StepAtAngle(-2.5f, 0, 2); // Left 45-degree check
    }

    void StepAtAngle(float x, float y, float z)
    {
        RaycastHit hitLowerAngle;
        if (Physics.Raycast(stepRayLower.transform.position,
                            transform.TransformDirection(x, y, z),
                            out hitLowerAngle, 0.5f))
        {
            RaycastHit hitUpperAngle;
            if (!Physics.Raycast(stepRayUpper.transform.position,
                                 transform.TransformDirection(x, y, z),
                                 out hitUpperAngle, 0.5f))
            {
                // Only step up if not on a slope
                if (!IsOnSlope())
                {
                    rigidbody.position += new Vector3(0f, stepSmooth, 0f);
                }
            }
        }

        RaycastHit hitLowerDownAngle;
        if (Physics.Raycast(transform.position + new Vector3(0, -stepHeight, 0),
                            Vector3.down, out hitLowerDownAngle, 0.5f))
        {
            // Step down logic at angles
            if (hitLowerDownAngle.distance > stepHeight && !IsOnSlope())
            {
                rigidbody.position -= new Vector3(0f, stepSmooth, 0f);
            }
        }
    }

    bool IsOnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 1.5f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle > 5f && angle < characterController.slopeLimit;
        }
        return false;
    }

    void Movement()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("Walking", true);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Apply movement, accounting for slopes and gravity
            if (IsOnSlope())
            {
                Vector3 slopeMovement = Vector3.ProjectOnPlane(moveDir, slopeHit.normal);
                characterController.Move(slopeMovement.normalized * speed * Time.deltaTime);
            }
            else
            {
                characterController.Move(moveDir.normalized * speed * Time.deltaTime);
            }
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

}
