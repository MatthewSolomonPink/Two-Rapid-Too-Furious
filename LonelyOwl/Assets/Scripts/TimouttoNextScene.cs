using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimouttoNextScene : MonoBehaviour
{
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private bool startAutomatic = true;
    [SerializeField] TransitionBehavior transition;
    void Start()
    {
        if (startAutomatic) { StartCoroutine(nextStage(waitTime)); }
    }

    public IEnumerator nextStage(float secondsToWait)
    {
        yield return new WaitForSecondsRealtime(secondsToWait);

        transition.goToNextScene();
    }

}

