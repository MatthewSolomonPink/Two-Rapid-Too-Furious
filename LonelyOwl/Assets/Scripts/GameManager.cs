using UnityEngine;
using System.IO;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI journalText;
    [SerializeField] TextAsset scene_1_text;
    [SerializeField] TextAsset scene_2_text;
    [SerializeField] TextAsset scene_3_text;

    //DEBUG Int
    int journalCounter = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //DEBUG
        journalText.text += "\n";
    }

    // Update is called once per frame
    void Update()
    {
        //DEBUG - Add text
        if (Input.GetKeyUp(KeyCode.O))
        {
            addJournalText(journalCounter);
        }

        //Open Journal
        if (Input.GetKeyUp(KeyCode.J))
        {
            if (journalText.IsActive())
            {
                journalText.enabled = false;
            }
            else
            {
                journalText.enabled = true;
            }

        }
    }

     public void addJournalText(int text_Index)
    {
        switch (text_Index)
        {
            case 0:
                journalText.text += scene_1_text.text + "\n\n";
                journalCounter++;
                break;
            case 1:
                journalText.text += scene_2_text.text + "\n\n";
                journalCounter++;
                break;

            case 2:
                journalText.text += scene_3_text.text + "\n\n";
                journalCounter++;
                break;
        }
    }
}
