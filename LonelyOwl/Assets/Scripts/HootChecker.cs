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

    InputAction hoot;

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>() == null ? gameObject.AddComponent<AudioSource>() : GetComponent<AudioSource>();
        hoot = InputSystem.actions.FindAction("Hoot");
    }
    

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Collide");
        //if ((Input.GetKeyDown(KeyCode.Space)) && (other.gameObject.tag == "Player") && !fired)
        if ((hoot.IsPressed()) && (other.gameObject.tag == "Player") && !fired)
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
