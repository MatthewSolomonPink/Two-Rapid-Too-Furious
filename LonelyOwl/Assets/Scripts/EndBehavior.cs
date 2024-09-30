using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndBehavior : MonoBehaviour
{
    [SerializeField] JournalData journalData;
    [SerializeField] TextMeshProUGUI recapText;

    string journal_1_selections;
    string journal_2_selections;
    string journal_3_selections;
    string journal_4_selections;

    [SerializeField] string journal_1_mainText;
    [SerializeField] string journal_2_mainText;
    [SerializeField] string journal_3_mainText;
    [SerializeField] string journal_4_mainText;

    [SerializeField] Animation fadeout;
    [SerializeField] Button endBtn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        journal_1_selections = journalData.journal_1_selection[0] + ", " + journalData.journal_1_selection[1] + ", " +
            journalData.journal_1_selection[2] + "\n";

        journal_2_selections = journalData.journal_2_selection[0] + ", " + journalData.journal_2_selection[1] + ", " +
            journalData.journal_2_selection[2] + "\n";

        journal_3_selections = journalData.journal_3_selection[0] + ", " + journalData.journal_3_selection[1] + ", " +
            journalData.journal_3_selection[2] + "\n";

        journal_4_selections = journalData.journal_4_selection[0] + ", " + journalData.journal_4_selection[1] + ", " +
            journalData.journal_4_selection[2] + "\n";

        StartCoroutine("recapSequence");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator recapSequence()
    {
        recapText.text = journal_1_selections;
        yield return new WaitForSeconds(1);
        recapText.text += journal_1_mainText + "\n\n";
        yield return new WaitForSeconds(2);

        recapText.text += journal_2_selections;
        yield return new WaitForSeconds(1);
        recapText.text += journal_2_mainText + "\n\n";
        yield return new WaitForSeconds(2);

        recapText.text += journal_3_selections;
        yield return new WaitForSeconds(1);
        recapText.text += journal_3_mainText + "\n\n";
        yield return new WaitForSeconds(2);

        recapText.text += journal_4_selections;
        yield return new WaitForSeconds(1);
        recapText.text += journal_4_mainText + "\n\n";
        yield return new WaitForSeconds(5);

        fadeout.Play("FadeIn");

        yield return new WaitForSeconds(2);
        endBtn.enabled = true;
        //endBtn.GetComponent<CanvasGroup>().alpha = 1.0f;
    }

    public void endGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
