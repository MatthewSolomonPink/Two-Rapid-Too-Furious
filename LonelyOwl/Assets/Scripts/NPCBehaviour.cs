using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    bool isInteracted = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Need to do some action
            isInteracted=true;
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
