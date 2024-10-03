using System.Collections;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    Animator animator;

    [SerializeField] GameObject bookCutOut;
    [SerializeField] GameObject bookText;
    [SerializeField] GameObject player;
    [SerializeField] float waitForAnimation;
    void Start()
    {
        animator = GetComponent<Animator>();
        
        PlayBookOpenAnimation();
    }

    void Update()
    {
        //Check if animation is completed
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 
            && !animator.IsInTransition(0))
        {
            StartCoroutine(TranistionEffect());
            SetGameObjectStatus(true);
            this.gameObject.SetActive(false);
        }
    }
    private void PlayBookOpenAnimation()
    {
        if (animator != null)
        {
            SetGameObjectStatus(false);
            animator.Play(0);

        }
    }
    private void SetGameObjectStatus(bool isVisible)
    {
        player.SetActive(isVisible);
        bookCutOut.SetActive(isVisible);
        bookText.SetActive(isVisible);
    }

    //Added for smooth after animation effect
    IEnumerator TranistionEffect()
    {
        yield return new WaitForSeconds(waitForAnimation);
    }
}
