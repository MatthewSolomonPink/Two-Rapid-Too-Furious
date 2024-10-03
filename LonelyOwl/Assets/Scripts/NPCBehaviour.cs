using System.Collections;
//using UnityEditor.ShaderGraph.Internal; //Problem
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    bool isInteracted = false;
    
    PlayerBehaviour playerBehaviour;
    MeshCollider stairMeshCollider;

    new Animation animation;

    [SerializeField] GameObject stairs;
    void Start()
    {
        playerBehaviour = FindFirstObjectByType<PlayerBehaviour>();
        animation = GetComponentInChildren<Canvas>().GetComponent<Animation>();
        stairMeshCollider = stairs.GetComponent<MeshCollider>();
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
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isInteracted)
        {
            isInteracted = true;
            playerBehaviour.SetPlayerMovable(false);
            stairMeshCollider.enabled = true;
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
