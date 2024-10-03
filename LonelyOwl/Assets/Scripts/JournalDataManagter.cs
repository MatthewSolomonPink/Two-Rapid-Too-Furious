using UnityEngine;

public class JournalDataManagter : MonoBehaviour
{
    [SerializeField] JournalData journalData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addSelection(int journalIndex, int numSelectedWords, string optionText)
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
    }
}
