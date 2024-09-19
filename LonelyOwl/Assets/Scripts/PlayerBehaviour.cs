using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerBehaviour : MonoBehaviour
{
    public CharacterController characterController;
    public Transform cam;

    public Camera mainCam;
    public Camera otherCam;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float vertical;
    float horizontal;

    NPCBehaviour nPCBehaviour;

    public void Start()
    {
        nPCBehaviour = FindAnyObjectByType<NPCBehaviour>();
    }

    private void Update()
    {
        //ToDo
        //1.Need to check game status and restrict movement accordingly.
        //2. Make the camera switch after NPC collision to move down to next level.

        if (nPCBehaviour.IsInteracted())
        {
            mainCam.enabled = false;
            otherCam.enabled = true;
            //Add a transition            
            cam = otherCam.transform;
        }
        Movement();
    }

    void Movement()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) *
                Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            characterController.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    //Change Scene
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("") && nPCBehaviour.IsInteracted())
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if(currentScene > 2)
            {
                currentScene = 0;
            }
            //ToDo
            //1.Add transition
            SceneManager.LoadSceneAsync(currentScene + 1);
        }
    }
}
