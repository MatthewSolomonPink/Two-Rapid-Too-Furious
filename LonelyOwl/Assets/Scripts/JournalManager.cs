using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JournalManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mainText;
    int numSelectedWords = 0;
    [SerializeField] int maxSelectedWords;
    [SerializeField] GameObject[] wordOptions;
    bool goingToNext = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (numSelectedWords == maxSelectedWords && !goingToNext)
        {
            mainText.enabled = true;
            foreach (var word in wordOptions)
            {
                word.GameObject().SetActive(false);
            }
            goingToNext = true;
            StartCoroutine("nextStage");
        }
    }

    public void optionSelected()
    {
        numSelectedWords++;
    }

    IEnumerator nextStage()
    {
        yield return new WaitForSecondsRealtime(7);

        //stop singing?

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Load Next Scene");
        SceneManager.LoadSceneAsync(currentSceneIndex + 1);
    }
}
