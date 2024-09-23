using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class JournalManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mainText;
    int numSelectedWords = 0;
    [SerializeField] int maxSelectedWords;
    [SerializeField] GameObject[] wordOptions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (numSelectedWords == maxSelectedWords)
        {
            mainText.enabled = true;
            foreach (var word in wordOptions)
            {
                word.GameObject().SetActive(false);
            }
        }
    }

    public void optionSelected()
    {
        numSelectedWords++;
    }
}
