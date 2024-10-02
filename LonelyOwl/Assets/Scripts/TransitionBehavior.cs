using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines.Interpolators;

public class TransitionBehavior : MonoBehaviour
{
    [SerializeField] PlayerBehaviour player;
    [SerializeField] Material pageMat;
    [SerializeField] GameObject pageFlip;
    [SerializeField] Animator pageFlipAnim;

    [SerializeField] bool startingScene = true;
    float t = 0f;
    bool endingCurrentScene = false;
    public bool inVoid2 = false; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startingScene)
        {
            player.SetPlayerMovable(false);
            pageMat.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            pageMat.color = new Color(1f, 1f, 1f, 0f);
        }
        pageFlip.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (startingScene)
        {
            pageMat.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, t));
            t += 0.4f * Time.deltaTime;
            if (pageMat.color.a <= 0f)
            {
                startingScene = false;
                if (!inVoid2)
                {
                    player.SetPlayerMovable(true);
                }
            }
        }
        else if (endingCurrentScene)
        {
            pageMat.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, t));
            t += 0.4f * Time.deltaTime;
            if (pageMat.color.a >= 1f)
            {
                StartCoroutine("nextScene");
                endingCurrentScene= false;
            }
        }
    }

    public void goToNextScene()
    {
        endingCurrentScene = true;
        t = 0f;
    }

    IEnumerator nextScene()
    {
        player.SetPlayerMovable(false);
        pageFlip.gameObject.SetActive(true);
        pageFlipAnim.SetBool("FlipPage", true);
        yield return new WaitForSeconds(6f);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Load Next Scene");
        SceneManager.LoadSceneAsync(currentSceneIndex + 1);
    }
}
