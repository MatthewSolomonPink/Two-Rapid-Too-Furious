using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

public class MainMenuManager : MonoBehaviour
{
    static List<string> keyBindings = new List<string>{"W to move forward", "S to move backward", 
        "A to move left", "D to move right"};
    
    private Dictionary<string, string> keys = new Dictionary<string, string>();
    
    TimouttoNextScene timouttoNextScene;
    TMP_Text text;

    PlayerBehaviour playerBehaviour;

    static bool isAllKeyInteracted = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timouttoNextScene = FindAnyObjectByType<TimouttoNextScene>();
        playerBehaviour = FindAnyObjectByType<PlayerBehaviour>();
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckKeyBinding(Keyboard.current.wKey, "W to move forward");
        CheckKeyBinding(Keyboard.current.aKey, "A to move left");
        CheckKeyBinding(Keyboard.current.sKey, "S to move backward");
        CheckKeyBinding(Keyboard.current.dKey, "D to move right");

        if (keyBindings.Count == 0 && gameObject.CompareTag("Interact"))
        {
            this.text.enabled = true;
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                isAllKeyInteracted = true;
                this.gameObject.SetActive(false);
            }
        }
        if(isAllKeyInteracted && gameObject.CompareTag("Finish"))
        {
            this.text.enabled = true;
        }
    }

    //Check for loading next scene
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && isAllKeyInteracted)
        {
            Debug.Log("Move to next scene");
            playerBehaviour.SetPlayerMovable(false);
            StartCoroutine(timouttoNextScene.nextStage(1f));
        }
    }

    // Check the key press and remove the text
    void CheckKeyBinding(KeyControl key, string actionText)
    {
        if (key.wasPressedThisFrame && this.text.text == actionText)
        {
            keyBindings.Remove(this.text.text);
            this.gameObject.SetActive(false);
        }
    }
}
