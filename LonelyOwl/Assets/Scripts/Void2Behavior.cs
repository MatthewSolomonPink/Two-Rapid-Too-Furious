using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] PlayerBehaviour player;
    [SerializeField] int maxBreathing;
    int timesBreathed = 0;
    InputAction breath;

    bool moveToNextScene = false;
    bool resumeControl = false;
    bool canBreath = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player.SetPlayerMovable(false);
        player.SetVoid2Breathing(true);
        player.ActivatePlayerBillboard("Breath (E)");

        breath = InputSystem.actions.FindAction("Interact");
    }

    // Update is called once per frame
    void Update()
    {
        if (timesBreathed < maxBreathing)
        {
            if (breath.IsPressed() && canBreath)
            {
                
                StartCoroutine("breathe");
            }
            if (timesBreathed == maxBreathing)
            {
                resumeControl = true;
            }
        }
        
        if (resumeControl)
        {
            player.SetPlayerMovable(true);
            player.SetVoid2Breathing(false);
            player.DeactivatePlayerBillboard();
            moveToNextScene = true;
            resumeControl = false;
        }

        if (moveToNextScene)
        {
            StartCoroutine("sceneChange");
            moveToNextScene = false;
        }
    }

    IEnumerator breathe()
    {
        canBreath = false;
        timesBreathed++;

        //breathing behavior
        yield return new WaitForSeconds(0.5f);
        canBreath = true;
    }

    IEnumerator sceneChange()
    {
        yield return new WaitForSeconds(5);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Load Next Scene");
        SceneManager.LoadSceneAsync(currentSceneIndex + 1);
    }
}