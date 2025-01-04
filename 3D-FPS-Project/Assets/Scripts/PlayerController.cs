using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] Camera playerCamera;
    [SerializeField] Animator animator;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float lookSpeed = 2f;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private float rotationX = 0f;
    private float originalMoveSpeed;
    private float speed;

    private bool isAiming = false;
    private bool isSprinting = false;

    //Reference to Input System
    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();
        originalMoveSpeed = moveSpeed;
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.Player.Interact.performed += OnInteract;
        controls.Player.Aim.performed += OnAim;
        controls.Player.Sprint.performed += OnSprint;
    }

    private void OnDisable()
    {
        controls.Disable();

        controls.Player.Interact.performed -= OnInteract;
        controls.Player.Aim.performed -= OnAim;
        controls.Player.Sprint.performed -= OnSprint;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        //Player movement
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        Vector3 moveDirection = transform.TransformDirection(move);
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        if (isSprinting)
        {
            animator.SetFloat("Speed", move.magnitude * 2);
            moveSpeed = sprintSpeed;
        }
        else
        {
            animator.SetFloat("Speed", move.magnitude);
            moveSpeed = originalMoveSpeed;
        }

        //Camera controls
        Vector2 mouseDelta = lookInput * lookSpeed;
        rotationX -= mouseDelta.y; //Look up and down
        rotationX = Mathf.Clamp(rotationX, -45f, 45f); //stops camera from flipping around player

        //Camera rotation
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseDelta.x); //Look left and right
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interaction");
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        isAiming = !isAiming;

        animator.SetBool("Aiming", isAiming);
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = !isSprinting;
    }
}
