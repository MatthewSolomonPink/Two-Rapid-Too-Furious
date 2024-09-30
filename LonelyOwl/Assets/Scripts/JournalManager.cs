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

    [SerializeField] int journalIndex;
    [SerializeField] JournalData journalData;

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
        switch (journalIndex)
        {
            case 0:
                journalData.journal_1_selection[numSelectedWords] = optionText;
                break;

            case 1:
                journalData.journal_2_selection[numSelectedWords] = optionText;
                break;

            case 2:
                journalData.journal_3_selection[numSelectedWords] = optionText;
                break;

            case 3:
                journalData.journal_4_selection[numSelectedWords] = optionText;
                break;
        }

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
