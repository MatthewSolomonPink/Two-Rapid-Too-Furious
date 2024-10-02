using System.Collections;
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

    private AudioSource AudioSource;
    private bool fired = false;
    private bool owlReady = false;

    InputAction hoot;

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>() == null ? gameObject.AddComponent<AudioSource>() : GetComponent<AudioSource>();
        hoot = InputSystem.actions.FindAction("Hoot");
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collide");
        if (other.gameObject.tag == "Player")
        {
            owlReady = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            owlReady = false;
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

        if (m_AudioClip != null) AudioSource.PlayOneShot(m_AudioClip);

        yield return new WaitForSecondsRealtime(secondsToWait);

        /*int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Load Next Scene");
        SceneManager.LoadSceneAsync(currentSceneIndex + 1);*/
        transition.goToNextScene();
    }

}
