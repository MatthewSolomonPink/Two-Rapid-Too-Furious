using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    bool isInteracted = false;
    
    PlayerBehaviour playerBehaviour;
    
    new Animation animation;
    void Start()
    {
        playerBehaviour = FindFirstObjectByType<PlayerBehaviour>();
        animation = GetComponentInChildren<Canvas>().GetComponent<Animation>();
    }

    //Fade screen black transition
    public IEnumerator AddTransition()
    {
        yield return new WaitForSeconds(1);
        playerBehaviour.CheckCamera();
        if (animation.isPlaying)
        {
            animation.Stop();
        }
        animation.Play("FadeOut");
        playerBehaviour.SetPlayerMovable(true);
    }

    //Check player interaction
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isInteracted)
        {
            isInteracted = true;
            playerBehaviour.SetPlayerMovable(false);
            if (animation.isPlaying)
            {
                animation.Stop();
            }
            animation.Play("FadeIn");
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
