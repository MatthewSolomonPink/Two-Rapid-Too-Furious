using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimouttoNextScene : MonoBehaviour
{
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private bool startAutomatic = true;

    PlayerBehaviour playerBehaviour;
    void Start()
    {
        if (startAutomatic) { StartCoroutine(nextStage(waitTime)); }
    }

    public IEnumerator nextStage(float secondsToWait)
    {
        yield return new WaitForSecondsRealtime(secondsToWait);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Load Next Scene");
        SceneManager.LoadSceneAsync(currentSceneIndex + 1);
        playerBehaviour = FindAnyObjectByType<PlayerBehaviour>();
        playerBehaviour.SetPlayerMovable(true);
    }
}

