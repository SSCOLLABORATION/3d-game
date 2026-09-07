using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwitcher : MonoBehaviour
{
    [Header("Player Objects")]
    public GameObject thirdPersonPlayer;
    public GameObject firstPersonPlayer;

    private bool _isThirdPersonActive = true;

    private void Start()
    {

        // Hide the cursor
    Cursor.visible = false;
    // Lock the cursor to the center of the screen
    Cursor.lockState = CursorLockMode.Locked;

    
        thirdPersonPlayer.SetActive(true);
        firstPersonPlayer.SetActive(false);
        _isThirdPersonActive = true;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.vKey.wasPressedThisFrame)
        {
            SwitchPlayer();
        }
    }

    private void SwitchPlayer()
    {
        if (_isThirdPersonActive)
        {
            // Fetch the First Person controller and disable it
            CharacterController fpController = firstPersonPlayer.GetComponent<CharacterController>();
            if (fpController != null) fpController.enabled = false;

            // Teleport the transform
            firstPersonPlayer.transform.position = thirdPersonPlayer.transform.position;
            firstPersonPlayer.transform.rotation = thirdPersonPlayer.transform.rotation;

            // Re-enable the controller
            if (fpController != null) fpController.enabled = true;

            thirdPersonPlayer.SetActive(false);
            firstPersonPlayer.SetActive(true);
        }
        else
        {
            // Fetch the Third Person controller and disable it
            CharacterController tpController = thirdPersonPlayer.GetComponent<CharacterController>();
            if (tpController != null) tpController.enabled = false;

            // Teleport the transform
            thirdPersonPlayer.transform.position = firstPersonPlayer.transform.position;
            thirdPersonPlayer.transform.rotation = firstPersonPlayer.transform.rotation;

            // Re-enable the controller
            if (tpController != null) tpController.enabled = true;

            firstPersonPlayer.SetActive(false);
            thirdPersonPlayer.SetActive(true);
        }

        _isThirdPersonActive = !_isThirdPersonActive;
    }
}