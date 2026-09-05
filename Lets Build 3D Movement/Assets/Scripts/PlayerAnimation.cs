using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
   [SerializeField] private Animator _animator;
   [SerializeField] private float locomotionBlentSpeed = 0.1f;
   private PlayerLocomotionInput _playerLocomotionInput;

   private static int inputXHash = Animator.StringToHash("InputX");
   private static int inputYHash = Animator.StringToHash("InputY");
   //making reference to animator parameters in unity
   //This is usefull because then we dont have to remember the exact string every time we want to reference it, and it also makes it easier to change the name of the parameter in the future if we need to.

    private Vector3 _currentBlendInput = Vector3.zero;
    private void Awake()
    {
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
    }

    private void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        Vector2 inputTarget = _playerLocomotionInput.MovementInput;
        _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, locomotionBlentSpeed*Time.deltaTime);

        _animator.SetFloat(inputXHash, inputTarget.x);
        _animator.SetFloat(inputYHash, inputTarget.y);
    }
}
