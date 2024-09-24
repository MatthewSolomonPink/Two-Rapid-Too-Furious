using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class JournalOptionBehavior : MonoBehaviour
{
    bool interactable;
    InputAction interactAction;

    private TextMeshPro floatingText;
    private Color c;
    [SerializeField] TextMeshProUGUI optionText;
    [SerializeField] JournalManager journalManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactable = false;
        interactAction = InputSystem.actions.FindAction("Interact");

        floatingText  = GetComponent<TextMeshPro>();
        c = floatingText.color; 
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable) 
        {
            if (interactAction.IsPressed())
            {
                optionText.enabled = true;
                journalManager.optionSelected();
                this.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactable = true;
            floatingText.fontStyle = FontStyles.Bold;
            floatingText.color = c * 4;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        { 
            interactable = false;
            floatingText.fontStyle = FontStyles.Normal;
            floatingText.color = c;
        }
    }
}
