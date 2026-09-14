using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
{
    public PlayerControls PlayerControls { get; private set; }
    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool SprintInput { get; private set; }
    
    // Jump input state store karne ke liye nayi property
    public bool JumpInput { get; private set; } 

    public bool CrouchInput { get; private set; }

    private void OnEnable()
    {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();
        // this enables player controlls and allows the player to use the input actions defined in the PlayerControls class.
        PlayerControls.PlayerLocomotionMap.Enable();
        PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
    }

    private void OnDisable()
    {
        PlayerControls.Disable();
        // this disables player controlls and prevents the player from using the input actions defined in the PlayerControls class.
        PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
        //properly disables the input actions and removes the callbacks when the script is disabled, preventing any potential issues with input handling.
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
        //set movement input to vector 2 output of the input action, which is a 2D vector representing the direction and magnitude of the player's movement input.
        // print(MovementInput); // Commented out to avoid console spam, enable if needed for debugging
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
        //set look input to vector 2 output of the input action, which is a 2D vector representing the direction and magnitude of the player's look input.
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Exception hata kar button ki boolean state read kar rahe hain
        // Jab button press hoga toh true hoga, release hone par false ho jayega
        JumpInput = context.ReadValueAsButton();
    }

    public void OnSprintToggle(InputAction.CallbackContext context)
    {
        // Jab shift press hoga tab true, release par false
        SprintInput = context.ReadValueAsButton();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        // Button hold karne par crouch, chhodne par stand (Hold-to-crouch)
        CrouchInput = context.ReadValueAsButton();
        
        // Agar aapko Toggle (ek baar dabane par crouch) chahiye, toh ye use karein:
        // if (context.performed) CrouchInput = !CrouchInput;
    }
}