using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterController _characterController; 
    [SerializeField] private PlayerState _playerState;
    [SerializeField] private float locomotionBlentSpeed = 10f; 
    
    private PlayerLocomotionInput _playerLocomotionInput;
    
    private static int isCrouchingHash = Animator.StringToHash("IsCrouching");
    private static int inputXHash = Animator.StringToHash("InputX");
    private static int inputYHash = Animator.StringToHash("InputY");
    private static int inputMagnitudeHash = Animator.StringToHash("InputMagnitude");
    
    private static int isGroundedHash = Animator.StringToHash("IsGrounded");
    private static int verticalVelocityHash = Animator.StringToHash("VerticalVelocity");
    private static int isZipliningHash = Animator.StringToHash("IsZiplining");
    
    // DASH: Dash ke liye naya hash add kiya
    private static int isDashingHash = Animator.StringToHash("IsDashing");

    private Vector3 _currentBlendInput = Vector3.zero;
    private float _currentMagnitude = 0f;

    private void Awake()
    {
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        
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

        float targetMagnitude = 0f;
        if (inputTarget != Vector2.zero)
        {
            targetMagnitude = _playerLocomotionInput.SprintInput ? 1.5f : inputTarget.magnitude;
        }
        
        _currentBlendInput = Vector3.Lerp(_currentBlendInput, new Vector3(inputTarget.x, inputTarget.y, 0f), locomotionBlentSpeed * Time.deltaTime);
        _currentMagnitude = Mathf.Lerp(_currentMagnitude, targetMagnitude, locomotionBlentSpeed * Time.deltaTime);

        _animator.SetFloat(inputXHash, _currentBlendInput.x);
        _animator.SetFloat(inputYHash, _currentBlendInput.y);
        _animator.SetFloat(inputMagnitudeHash, _currentMagnitude);

        // ZIPLINE ANIMATION
        bool isZiplining = _playerState.CurrentPlayerMovementState == PlayerMovementState.Ziplining;
        _animator.SetBool(isZipliningHash, isZiplining);

        // DASH ANIMATION: Player state se check kar rahe hain ki kya player dash kar raha hai
        bool isDashing = _playerState.CurrentPlayerMovementState == PlayerMovementState.Dashing;
        _animator.SetBool(isDashingHash, isDashing);

        // JUMP & CROUCH
        _animator.SetBool(isGroundedHash, _characterController.isGrounded);
        _animator.SetFloat(verticalVelocityHash, _characterController.velocity.y);
        _animator.SetBool(isCrouchingHash, _playerLocomotionInput.CrouchInput);
    }
}