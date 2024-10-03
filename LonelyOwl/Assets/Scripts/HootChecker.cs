using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HootChecker : MonoBehaviour
{

    //[SerializeField] private GameObject player;
    [SerializeField] private AudioClip m_AudioClip;
    [SerializeField] private float waitTime = 2;
    [SerializeField] private TransitionBehavior transition;
    [SerializeField] private TextMeshProUGUI text;

    private AudioSource AudioSource;
    private bool fired = false;
    private bool owlReady = false;

    InputAction hoot;

    bool isInteracted = false;

    PlayerBehaviour playerBehaviour;
    MeshCollider stairMeshCollider;
    MeshCollider stairMeshCollider2;
    new Animation animation;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject stairs;
    [SerializeField] GameObject stairs2;
    [SerializeField] GameObject levelTrigger;
    private void Start()
    {
        AudioSource = GetComponent<AudioSource>() == null ? gameObject.AddComponent<AudioSource>() : GetComponent<AudioSource>();
        hoot = InputSystem.actions.FindAction("Hoot");

        playerBehaviour = FindFirstObjectByType<PlayerBehaviour>();
        animation = canvas.GetComponent<Animation>();
        stairMeshCollider = stairs.GetComponent<MeshCollider>();
        stairMeshCollider2 = stairs2.GetComponent<MeshCollider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collide");
        if (other.gameObject.tag == "Player")
        {
            owlReady = true;
            text.text = "Press space to hoot!";

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            owlReady = false;
            text.text = "";
        }
    }

    private void Update()
    {
        if (owlReady && !fired && (Input.GetKeyUp(KeyCode.Space)))
        {
            Debug.Log("Active");
            fired = true;
            StartCoroutine(nextStage(waitTime));
        }
    }

    public IEnumerator nextStage(float secondsToWait)
    {
        yield return new WaitForSeconds(1f);
        if (m_AudioClip != null) AudioSource.PlayOneShot(m_AudioClip);

        yield return new WaitForSecondsRealtime(secondsToWait);


        if (!isInteracted)
        {
            isInteracted = true;
            playerBehaviour.SetPlayerMovable(false);
            stairMeshCollider.enabled = true;
            if (animation.isPlaying)
            {
                animation.Stop();
            }
            animation.Play("FadeIn");
            StartCoroutine(AddTransition());
        }

        /*int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Load Next Scene");
        SceneManager.LoadSceneAsync(currentSceneIndex + 1);*/
        //transition.goToNextScene();
    }

    public IEnumerator AddTransition()
    {
        yield return new WaitForSeconds(1);
        //playerBehaviour.CheckCamera();

        playerBehaviour.mainCam.SetActive(false);
        playerBehaviour.otherCam.SetActive(true);
        playerBehaviour.cam = playerBehaviour.otherCam.transform;

        if (animation.isPlaying)
        {
            animation.Stop();
        }
        animation.Play("FadeOut");
        playerBehaviour.SetPlayerMovable(true);
        levelTrigger.SetActive(true);
        
        

    }

}
