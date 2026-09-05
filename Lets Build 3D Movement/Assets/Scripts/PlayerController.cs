using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviour
{
    [Header("Component References")]
    //this is the main player controller script that handles the player's movement and input. It uses the PlayerLocomotionInput script to get the player's input and then applies that input to move the player character in the game world.
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _playerCamera;
    //using underscore to denote that this is a member variable of the class. This is a common convention in C# to differentiate between local variables and member variables.


    [Header("Movement Settings")]
    public float runAcceleration = 10f;
    public float runSpeed = 5f;
    public float drag = 0.1f;
    [Header("Look Settings")]
    public float LookSensH= 1f;
    public float LookSensV= 1f;
    public float LookLimitV= 80f;
    

    private PlayerLocomotionInput _playerLocomotionInput;
    private Vector2 _cameraRotation= Vector2.zero;
    private Vector2 _playerTargetRotation= Vector2.zero;

    private void Awake()
    {
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        //get the PlayerLocomotionInput component attached to the player game object and store it in the _playerLocomotionInput variable for later use.
    }


    private void Update()
    {
        Vector3 cameraforwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
        Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
        //this calculates the forward and right direction of the camera in the XZ plane (ignoring the Y axis) and normalizes them to get unit vectors. This is used to determine the direction of movement based on the camera's orientation.
        Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraforwardXZ * _playerLocomotionInput.MovementInput.y;
        //this calculates the movement direction based on the player's input and the camera's orientation. The player's input is a 2D vector (x, y) representing the direction of movement, and this is combined with the camera's forward and right vectors to get a 3D movement direction in the game world.
        //doing this if we press w or forward ew walk in the direction our camera facing and if we press d we walk in the direction 90 degrees to the right of the camera facing direction .
        Vector3 movementDelta = movementDirection * runAcceleration * Time.deltaTime;

        //tels how much the layer moves this frame
        Vector3 newVelocity =_characterController.velocity + movementDelta;
        // here acceleration is multiplied by delta time instead of speed because we want to apply the acceleration to the current velocity of the character controller, rather than setting a fixed speed. This allows for smoother and more responsive movement, as the character's velocity will gradually increase or decrease based on the player's input and the acceleration value.
        // this is based on 2nd law of kinamatics

        Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
        //add drag to player
        newVelocity =(newVelocity.magnitude > drag *Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
        //if our new velocities magnitude is greater than the drag multiplied by delta time, we subtract the current drag from the new velocity. Otherwise, we set the new velocity to zero. This prevents the player from sliding indefinitely when no input is given and simulates friction or resistance in movement.
        newVelocity =Vector3.ClampMagnitude(newVelocity, runSpeed);
        //clamp the new velocity to the maximum run speed, preventing the player from moving faster
        _characterController.Move(newVelocity * Time.deltaTime);
        //unity suggests calling it only once per frame, and it should be called in the Update() method to ensure that the character's movement is updated every frame based on the player's input and the current velocity of the character controller.
        
    }

    private void LateUpdate()
    {
        _cameraRotation.x += LookSensH * _playerLocomotionInput.LookInput.x;
        _cameraRotation.y = Mathf.Clamp(_cameraRotation.y + LookSensV * _playerLocomotionInput.LookInput.y, -LookLimitV, LookLimitV);
        //this will make sure that the camera rotation is clamped to a certain range, preventing the player from looking too far up or down and potentially breaking the camera's view.
        _playerTargetRotation.x+= LookSensH * _playerLocomotionInput.LookInput.x;
        transform.rotation = Quaternion.Euler(0f, _cameraRotation.x, 0f);
        //this will rotate the player character based on the camera's horizontal rotation, allowing the player
        _playerCamera.transform.localRotation = Quaternion.Euler(-_cameraRotation.y, _cameraRotation.x, 0f); 
    }




    
}


