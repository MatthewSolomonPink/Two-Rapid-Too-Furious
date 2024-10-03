using System.Collections;
using UnityEngine;

public class Stage3EventHandler : MonoBehaviour
{
    [SerializeField] private GameObject inkSplats;
    [SerializeField] private GameObject bigOwl;
    [SerializeField] private float targetHeight = 4f;
    [SerializeField] private float speed = 10f;
    [SerializeField] TransitionBehavior transition;
    private float owlHeight = 0f;

    private bool fired = false;
    public bool owlEmerge = false;

    private bool owlRise = false;
    private bool singing = false;
    void Update()
    {
        if(owlEmerge && !fired)
        {
            fired = true;
            StartCoroutine(spawnOwl());
        }
        if (owlRise)
        {
            owlHeight += Time.deltaTime * speed;
            if (owlHeight > targetHeight) 
            {
                owlHeight = targetHeight;
                singing = true;
            }
                
            bigOwl.transform.position = new Vector3(bigOwl.transform.position.x, owlHeight, bigOwl.transform.position.z);
        }

        if (singing && Input.GetKeyUp(KeyCode.Space))
        {
            //Play large owl animation
        }
    }

    private IEnumerator spawnOwl()
    {
        inkSplats.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        owlRise = true;
        yield return new WaitForSecondsRealtime(10f);
        transition.goToNextScene();
    }
}
