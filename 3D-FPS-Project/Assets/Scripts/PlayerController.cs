using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] Camera playerCamera;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float lookSpeed = 2f;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float rotationX = 0f;

    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        //Player movement
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        Vector3 moveDirection = transform.TransformDirection(move);
        characterController.Move(moveDirection *  moveSpeed * Time.deltaTime);

        //Camera controls
        Vector2 mouseDelta = lookInput * lookSpeed;
        rotationX -= mouseDelta.y; //Look up and down
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); //stops camera from flipping around player

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
}
