using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HootChecker : MonoBehaviour
{

    //[SerializeField] private GameObject player;
    [SerializeField] private AudioClip m_AudioClip;
    [SerializeField] private float waitTime = 2;

    private AudioSource AudioSource;
    private bool fired = false;

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>() == null ? gameObject.AddComponent<AudioSource>() : GetComponent<AudioSource>();

    }
    

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Collide");
        if ((Input.GetKeyUp(KeyCode.Space)) && (other.gameObject.tag == "Player") && !fired)
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

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Load Next Scene");
        SceneManager.LoadSceneAsync(currentSceneIndex + 1);
    }

}
