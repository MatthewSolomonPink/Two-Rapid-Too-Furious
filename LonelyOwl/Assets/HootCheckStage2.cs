using System.Collections;
using Unity.VisualScripting;
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
    [SerializeField] Animator stage2Animator;

    private float yVal = 0;
    private StageIICharacterAnimator animator;
    private void TriggerAngryHoodAnimation()
    {
        // 设置 Animator 的 AngryHood 参数为 true
        stage2Animator.SetBool("Angry_Hood", true);

        // 你也可以考虑在这里使用协程来控制动画的持续时间
        StartCoroutine(ResetAngryHoodAnimation());
    }

    private IEnumerator ResetAngryHoodAnimation()
    {
        // 等待一段时间再将 AngryHood 设置为 false
        yield return new WaitForSeconds(1f);
        stage2Animator.SetBool("Angry_Hood", false);
    }

    private AudioSource AudioSource;

    private bool fired = false;
    private bool owlReady = false;

    private void Start()
    {
        //stage2Animator = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>() == null ? gameObject.AddComponent<AudioSource>() : GetComponent<AudioSource>();
        animator = GetComponent<StageIICharacterAnimator>();
        stage2Animator.SetBool("Angry_Hood", false);
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

        //Set the animator
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stage2Animator.SetBool("Angry_Hood", true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            stage2Animator.SetBool("Angry_Hood", false);
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerAngryHoodAnimation();
        }
        if (m_AudioClip != null)
        {
            // Triggers Screech
            AudioSource.PlayOneShot(m_AudioClip);
            yield return new WaitForSecondsRealtime(.5f);
            AudioSource.PlayOneShot(m_AudioClip);
            yield return new WaitForSecondsRealtime(.2f);
            AudioSource.PlayOneShot(m_AudioClip);

        }
        Stage2EventHandler levelEvent = manager.GetComponent<Stage2EventHandler>();
        levelEvent.playerInteracted = true;


        yield return new WaitForSecondsRealtime(secondsToWait);

        /* int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Load Next Scene");
        SceneManager.LoadSceneAsync(currentSceneIndex + 1);*/


        transition.goToNextScene();
    }


}



