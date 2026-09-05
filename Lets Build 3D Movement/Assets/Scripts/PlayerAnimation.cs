using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterController _characterController; // CharacterController reference add kiya
    [SerializeField] private float locomotionBlentSpeed = 10f; // Note: Isko thoda bada rakhein warna blend bohot slow hoga
    
    private PlayerLocomotionInput _playerLocomotionInput;

    private static int inputXHash = Animator.StringToHash("InputX");
    private static int inputYHash = Animator.StringToHash("InputY");
    
    // Jump ke liye naye Animator parameters ke hashes
    private static int isGroundedHash = Animator.StringToHash("IsGrounded");
    private static int verticalVelocityHash = Animator.StringToHash("VerticalVelocity");

    private Vector3 _currentBlendInput = Vector3.zero;

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
        
        // Smooth blending calculation
        _currentBlendInput = Vector3.Lerp(_currentBlendInput, new Vector3(inputTarget.x, inputTarget.y, 0f), locomotionBlentSpeed * Time.deltaTime);

        // FIX: inputTarget ki jagah _currentBlendInput use karein taaki transition smooth ho
        _animator.SetFloat(inputXHash, _currentBlendInput.x);
        _animator.SetFloat(inputYHash, _currentBlendInput.y);

        // JUMP ANIMATION LOGIC:
        // Animator ko zameen ka status aur vertical speed bhej rahe hain
        _animator.SetBool(isGroundedHash, _characterController.isGrounded);
        _animator.SetFloat(verticalVelocityHash, _characterController.velocity.y);
    }
}