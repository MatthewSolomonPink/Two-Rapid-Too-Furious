using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    Animator animator;
    [SerializeField] GameObject bookCutOut;
    [SerializeField] GameObject bookText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            bookCutOut.SetActive(false);
            bookText.SetActive(false);
            animator.Play(0);
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(animator.GetBool("isCompleted"));
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            bookCutOut.SetActive(true);
            bookText.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
