using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterController _characterController; // CharacterController reference add kiya
    [SerializeField] private float locomotionBlentSpeed = 10f; // Note: Isko thoda bada rakhein warna blend bohot slow hoga
    
    private PlayerLocomotionInput _playerLocomotionInput;

    private static int isCrouchingHash = Animator.StringToHash("IsCrouching");

    private static int inputXHash = Animator.StringToHash("InputX");
    private static int inputYHash = Animator.StringToHash("InputY");

    private static int inputMagnitudeHash = Animator.StringToHash("InputMagnitude");
    
    // Jump ke liye naye Animator parameters ke hashes
    private static int isGroundedHash = Animator.StringToHash("IsGrounded");
    private static int verticalVelocityHash = Animator.StringToHash("VerticalVelocity");

    private Vector3 _currentBlendInput = Vector3.zero;
    private float _currentMagnitude = 0f;

    private void Awake()
    {
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        
        // Agar inspector mein assign karna bhool gaye toh auto-fetch kar lega
        if (_characterController == null) 
        {
            _characterController = GetComponent<CharacterController>();
        }
    }

    private void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        Vector2 inputTarget = _playerLocomotionInput.MovementInput;

        // Agar player move kar raha hai aur sprint daba hai, toh target magnitude 1.5f hoga, warna normal movement ka magnitude
        float targetMagnitude = 0f;
        if (inputTarget != Vector2.zero)
        {
            targetMagnitude = _playerLocomotionInput.SprintInput ? 1.5f : inputTarget.magnitude;
        }
        
        // Smooth blending calculation
        _currentBlendInput = Vector3.Lerp(_currentBlendInput, new Vector3(inputTarget.x, inputTarget.y, 0f), locomotionBlentSpeed * Time.deltaTime);


        // FIX 2: Magnitude ko Lerp karke smooth kiya
        _currentMagnitude = Mathf.Lerp(_currentMagnitude, targetMagnitude, locomotionBlentSpeed * Time.deltaTime);



        // FIX: inputTarget ki jagah _currentBlendInput use karein taaki transition smooth ho
        _animator.SetFloat(inputXHash, _currentBlendInput.x);
        _animator.SetFloat(inputYHash, _currentBlendInput.y);

        // FIX 3: Animator ko naya magnitude parameter bheja
        _animator.SetFloat(inputMagnitudeHash, _currentMagnitude);

        // JUMP ANIMATION LOGIC:
        // Animator ko zameen ka status aur vertical speed bhej rahe hain
        _animator.SetBool(isGroundedHash, _characterController.isGrounded);
        _animator.SetFloat(verticalVelocityHash, _characterController.velocity.y);

        _animator.SetBool(isCrouchingHash, _playerLocomotionInput.CrouchInput);
    }
}