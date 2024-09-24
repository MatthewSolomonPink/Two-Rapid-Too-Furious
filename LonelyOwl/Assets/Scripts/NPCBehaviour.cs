using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    bool isInteracted = false;
    PlayerBehaviour playerBehaviour;
    [SerializeField] Canvas canvas;
    new Animation animation;
    void Start()
    {
        playerBehaviour = FindFirstObjectByType<PlayerBehaviour>();
        animation = canvas.GetComponent<Animation>();
    }

    public IEnumerator AddTransition()
    {
        yield return new WaitForSeconds(1);
        if (animation.isPlaying)
        {
            playerBehaviour.CheckCamera();
            animation.Stop();
            animation.Play("FadeOut");
        }
        playerBehaviour.SetPlayerMovable(true);
    }

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
