using UnityEngine;
using System.Collections;

public class StageIICharacterAnimator : MonoBehaviour
{



    // Reference to the Animator component for the secondary character
    private Animator animator;

    void Update()
    {
        // ??????????
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerAnimation();
        }
    }

    public void TriggerAnimation()
    {
        // ?? Animator ?? "angryHood" ??? true
        animator.SetBool("angry_Hood", true);

        // ?????????????????????????
        StartCoroutine(ResetAngryHoodAnimation());
    }

    private IEnumerator ResetAngryHoodAnimation()
    {
        // ???????? angryHood ??? false
        yield return new WaitForSeconds(1f); // ????????????????
        animator.SetBool("angry_Hood", false);
    }
}
