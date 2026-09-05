using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
{
    
    public PlayerControls PlayerControls { get; private set; }
    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }


    public void OnJump(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

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
        print(MovementInput);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
        //set look input to vector 2 output of the input action, which is a 2D vector representing the direction and magnitude of the player's look input.
    }
    //getter and setter method makes PlayerControls  class property instead of a field
    //why?   To acces/get this variable from outside of the class but dont want anybody to change vriable from outside the class.
    //private set; means that the variable can only be set from within the class, but can be accessed from outside the class.


}
