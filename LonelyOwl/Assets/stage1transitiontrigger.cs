using UnityEngine;

public class stage1transitiontrigger : MonoBehaviour
{
    [SerializeField] private TransitionBehavior transition;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hello");
        if (collision.gameObject.tag == "Player")
        {
            transition.goToNextScene();
            Debug.Log("Hello");
        }
    }

   
}
