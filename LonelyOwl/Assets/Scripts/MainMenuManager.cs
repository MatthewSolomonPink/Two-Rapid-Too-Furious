using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour
{
    static List<string> keyBindings = new List<string>{"W to move forward", "S to move backward", 
        "A to move left", "D to move right"};
    TimouttoNextScene timouttoNextScene;
    TMP_Text text;
    static bool isInteractedMovement = false;
    static bool isAllKeyInteracted = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timouttoNextScene = FindAnyObjectByType<TimouttoNextScene>();
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(keyBindings.Count == 0 && gameObject.CompareTag("Interact"))
        {
            isInteractedMovement = true;
            this.text.enabled = true;
        }
        Debug.Log(isAllKeyInteracted);
        if(isAllKeyInteracted && gameObject.CompareTag("Finish"))
        {
            this.text.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (keyBindings.Count != 0 && !gameObject.CompareTag("Interact"))
            {
                Debug.Log(this.text.text);
                keyBindings.Remove(this.text.text);
                this.gameObject.SetActive(false);
            }
            else if (isInteractedMovement && gameObject.CompareTag("Interact"))
            {
                //Input the button
                isAllKeyInteracted = true;
                this.gameObject.SetActive(false);
            }
            else if (isAllKeyInteracted && gameObject.CompareTag("Finish"))
            {
                //this.gameObject.SetActive(false);
                Debug.Log("Move to next scene");
                StartCoroutine(timouttoNextScene.nextStage(1f));
            }
        }
    }

}
