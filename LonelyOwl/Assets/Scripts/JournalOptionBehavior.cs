using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class JournalOptionBehavior : MonoBehaviour
{
    bool interactable;
    InputAction interactAction;
    [SerializeField] TextMeshProUGUI optionText;
    [SerializeField] JournalManager journalManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactable = false;
        interactAction = InputSystem.actions.FindAction("Interact");
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        { 
            interactable = false; 
        }
    }
}
