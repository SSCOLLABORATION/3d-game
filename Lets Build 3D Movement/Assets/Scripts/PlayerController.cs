using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class PlayerController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _playerCamera;

    [Header("Movement Settings")]
    public float runAcceleration = 10f;
    public float runSpeed = 5f;

    public float sprintAcceleration = 20f; 
    public float sprintSpeed = 10f;

    public float crouchAcceleration = 5f;
    public float crouchSpeed = 2.5f;

    [Header("Crouch Height Settings")]
    public float standingHeight = 2f;
    public float crouchHeight = 1f;

    [Header("Drag & Threshold Settings")]
    public float drag = 0.1f;
    public float movingThreshhold = 0.01f;



    
    

    [Header("Look Settings")]
    public float LookSensH = 1f;
    public float LookSensV = 1f;
    public float LookLimitV = 80f;

    [Header("Jump & Gravity Settings")]
    public float gravity = 9.81f;
    public float jumpHeight = 2f;
    
    private float _verticalVelocity; // Y-axis ki speed track karne ke liye

    private PlayerLocomotionInput _playerLocomotionInput;
    private PlayerState _playerState;
    private Vector2 _cameraRotation = Vector2.zero;

    private void Awake()
    {
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        _playerState = GetComponent<PlayerState>();
    }

    private void Update()
    {
         // Hide the cursor
    Cursor.visible = false;
    // Lock the cursor to the center of the screen
    Cursor.lockState = CursorLockMode.Locked;
        UpdateMovementState();
        HandleCrouchHeight(); // Naya function call
        
        // Dono movements (X,Z aur Y) ko alag alag calculate kar rahe hain
        Vector3 finalVelocity = HandleLateralMovement();
        finalVelocity.y = HandleVerticalMovement();
        
        // CharacterController ko ek hi baar move karenge
        _characterController.Move(finalVelocity * Time.deltaTime);
    }

    private void HandleCrouchHeight()
    {
        // CharacterController ki height adjust karein taaki collision chota ho jaye
        if (_playerLocomotionInput.CrouchInput)
        {
            _characterController.height = crouchHeight;
            _characterController.center = new Vector3(0, crouchHeight / 2f, 0);
        }
        else
        {
            _characterController.height = standingHeight;
            _characterController.center = new Vector3(0, standingHeight / 2f, 0);
        }
    }


   private void UpdateMovementState()
    {
        bool isMovementInput = _playerLocomotionInput.MovementInput != Vector2.zero;
        bool isMovingLaterally = IsMovingLaterally();
        bool isSprinting = _playerLocomotionInput.SprintInput && isMovingLaterally;

        // Player state ko update karein taaki animations ko pata chale ki hum sprint kar rahe hain
        PlayerMovementState lateralState = isMovementInput || isMovingLaterally 
            ? (isSprinting ? PlayerMovementState.Sprinting : PlayerMovementState.Running) 
            : PlayerMovementState.Idling;
            
        _playerState.SetPlayerMovementState(lateralState);
    }

    private Vector3 HandleLateralMovement()
    {
        // Sprinting check kar ke current speed aur acceleration decide karein
        bool isSprinting = _playerLocomotionInput.SprintInput;
        bool isCrouching = _playerLocomotionInput.CrouchInput;


        

        // Crouch override karega sprint ko (crouch karte waqt bhag nahi sakte)
        float currentSpeed = isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : runSpeed);
        float currentAcceleration = isCrouching ? crouchAcceleration : (isSprinting ? sprintAcceleration : runAcceleration);


        Vector3 cameraforwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
        Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
        Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraforwardXZ * _playerLocomotionInput.MovementInput.y;
       // runAcceleration ki jagah currentAcceleration use karein
        Vector3 movementDelta = movementDirection * currentAcceleration * Time.deltaTime;

        // characterController.velocity lateral math ke liye theek hai
        Vector3 currentLateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z);
        Vector3 newVelocity = currentLateralVelocity + movementDelta;
        
        Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
        
        newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
        
       // runSpeed ki jagah currentSpeed se clamp karein
        newVelocity = Vector3.ClampMagnitude(newVelocity, currentSpeed);
        
        return newVelocity; // Return kar rahe hain Move call karne ki jagah
    }

    private float HandleVerticalMovement()
    {
        // Check agar player zameen par hai
        if (_characterController.isGrounded)
        {
            // Player ko zameen par chhipka ke rakhne ke liye choti si downward force
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f; 
            }

            // Jump Logic: Yahan check karein ki kya jump button press hua hai
            // (Assumed variable _playerLocomotionInput.JumpInput. Agar naam alag hai toh change kar lein)
            if (_playerLocomotionInput.JumpInput) 
            {
                // Physics formula: v = sqrt(2 * gravity * height)
                _verticalVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity);
            }
        }

        // Gravity apply karna: v = u - gt
        _verticalVelocity -= gravity * Time.deltaTime;
        
        return _verticalVelocity;
    }

    private void LateUpdate()
    {
        _cameraRotation.x += LookSensH * _playerLocomotionInput.LookInput.x;
        _cameraRotation.y = Mathf.Clamp(_cameraRotation.y + LookSensV * _playerLocomotionInput.LookInput.y, -LookLimitV, LookLimitV);
        
        transform.rotation = Quaternion.Euler(0f, _cameraRotation.x, 0f);
        _playerCamera.transform.localRotation = Quaternion.Euler(-_cameraRotation.y, 0f, 0f); 
    }

    private bool IsMovingLaterally()
    {
        Vector3 lateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z);
        return lateralVelocity.magnitude > movingThreshhold;  
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}