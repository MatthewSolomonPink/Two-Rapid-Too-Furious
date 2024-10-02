using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HootCheckStage2 : MonoBehaviour
{

    //[SerializeField] private GameObject player;
    [SerializeField] private AudioClip m_AudioClip;
    [SerializeField] private float waitTime = 2;
    [SerializeField] private float speed = 2;
    [SerializeField] private GameObject manager;
    [SerializeField] private GameObject objects;
    [SerializeField] private TransitionBehavior transition;

    private float yVal = 0;

    private AudioSource AudioSource;

    private bool fired = false;
    private bool owlReady = false;

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>() == null ? gameObject.AddComponent<AudioSource>() : GetComponent<AudioSource>();

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

        if (fired)
        {
            yVal += Time.deltaTime * speed;
            if (yVal > 6.06f)
                yVal = 6.06f;
            objects.transform.position = new Vector3(objects.transform.position.x, yVal, objects.transform.position.z);
        }
    }

    public IEnumerator nextStage(float secondsToWait)
    {
        yield return new WaitForSecondsRealtime(.5f);
        if (m_AudioClip != null) 
        { 
            AudioSource.PlayOneShot(m_AudioClip);
            yield return new WaitForSecondsRealtime(.5f);
            AudioSource.PlayOneShot(m_AudioClip);
            yield return new WaitForSecondsRealtime(.2f);
            AudioSource.PlayOneShot(m_AudioClip);
        }
        Stage2EventHandler levelEvent = manager.GetComponent<Stage2EventHandler>();
        levelEvent.playerInteracted = true;

        yield return new WaitForSecondsRealtime(secondsToWait);

        /*int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Load Next Scene");
        SceneManager.LoadSceneAsync(currentSceneIndex + 1);*/

        
        transition.goToNextScene();
    }

}
