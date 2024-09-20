using System.Collections;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    bool isInteracted = false;
    PlayerBehaviour playerBehaviour;

    void Start()
    {
        playerBehaviour = FindFirstObjectByType<PlayerBehaviour>();
    }

    public IEnumerator AddTransition()
    {
        playerBehaviour.CheckCamera();
        yield return new WaitForSeconds(0.3f);
        playerBehaviour.fadeOut = true;
        playerBehaviour.CameraTransition();
    }    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isInteracted)
        {
            isInteracted=true;
            playerBehaviour.fadeIn = true;
            playerBehaviour.CameraTransition();
            StartCoroutine(AddTransition());
        }
    }

    public bool IsInteracted()
    {
        return isInteracted; 
    }

    public void ResetIsInteracted()
    {
        isInteracted = false;
    }
}
