using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour
{
    public CharacterController characterController;
    public Transform cam;

    public GameObject mainCam;
    public GameObject otherCam;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float vertical;
    float horizontal;

    bool canPlayerMove = true;

    NPCBehaviour nPCBehaviour;
    new Rigidbody rigidbody;

    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 0.1f;

    public void Start()
    {
        nPCBehaviour = FindAnyObjectByType<NPCBehaviour>();
        rigidbody = GetComponent<Rigidbody>();
        stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x,
            stepHeight, stepRayUpper.transform.position.z);
    }

    private void Update()
    {
        //ToDo
        //1.Need to check game status and restrict movement accordingly.
        if (canPlayerMove)
        {
            Movement();
            StepClimb();
        }
    }

    public void OnHoot(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        if (context.interaction is PressInteraction)
        {
            Debug.Log("Hooting");
        }
    }

    public void CheckCamera()
    {
        if (nPCBehaviour.IsInteracted())
        {
            mainCam.SetActive(false);
            otherCam.SetActive(true);           
            cam = otherCam.transform;
        }
        if (otherCam && !nPCBehaviour.IsInteracted())
        {
            otherCam.SetActive(false);
            mainCam.SetActive(true);
            cam = mainCam.transform;
        }
    }

    public bool CanPlayerMove()
    {
        return canPlayerMove;
    }

    public void SetPlayerMovable(bool canPlayerMove)
    {
        this.canPlayerMove = canPlayerMove;
    }

    void Movement()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) *
                Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            characterController.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("NextScene") && nPCBehaviour.IsInteracted())
        {
            Debug.Log("Get into the stairs");
            //Go down staris
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NextScene") && nPCBehaviour.IsInteracted())
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (currentScene > 2)
            {
                currentScene = 0;
            }
            nPCBehaviour.ResetIsInteracted();
            CheckCamera();
            //ToDo
            //SceneManager.LoadSceneAsync(currentScene + 1);
        }
    }

    void StepClimb()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position,
            transform.TransformDirection(Vector3.forward),
            out hitLower, 0.1f))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position,
                transform.TransformDirection(Vector3.forward),
                out hitUpper, 0.2f))
            {
                rigidbody.position -= new Vector3(0f, -stepSmooth, 0f);
            }
        }

        RaycastHit hitLower45;
        if (Physics.Raycast(stepRayLower.transform.position,
            transform.TransformDirection(1.5f, 0, 1),
            out hitLower45, 0.1f))
        {
            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepRayUpper.transform.position,
                transform.TransformDirection(1.5f, 0, 1),
                out hitUpper45, 0.2f))
            {
                rigidbody.position -= new Vector3(0f, -stepSmooth, 0f);
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(stepRayLower.transform.position,
            transform.TransformDirection(-1.5f, 0, 1),
            out hitLowerMinus45, 0.1f))
        {
            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(stepRayUpper.transform.position,
                transform.TransformDirection(-1.5f, 0, 1),
                out hitUpperMinus45, 0.2f))
            {
                rigidbody.position -= new Vector3(0f, -stepSmooth, 0f);
            }
        }
    }
}
