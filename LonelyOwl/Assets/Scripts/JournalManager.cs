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
    [SerializeField] TransitionBehavior transition;

    [SerializeField] int journalIndex;
    [SerializeField] JournalDataManagter journalData;

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

    public void optionSelected(string optionText)
    {
        journalData.addSelection(journalIndex, numSelectedWords, optionText);
        numSelectedWords++;
    }

    IEnumerator nextStage()
    {
        yield return new WaitForSecondsRealtime(7);

        //stop singing?

        transition.goToNextScene();
    }
}
