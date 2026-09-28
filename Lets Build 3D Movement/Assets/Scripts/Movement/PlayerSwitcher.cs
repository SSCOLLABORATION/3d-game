using UnityEngine;
using Unity.Cinemachine; // Naye version mein ye namespace hota hai (ya fir sirf Cinemachine check karein)
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cinemachine Cameras")]
    // Yahan CinemachineVirtualCamera ki jagah CinemachineCamera use kiya hai
    public CinemachineCamera thirdPersonCam;
    public CinemachineCamera firstPersonCam;
    
    private bool _isThirdPerson = true;

    private void Start()
    {
        // TP camera on
        thirdPersonCam.Priority = 10;
        firstPersonCam.Priority = 5;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.vKey.wasPressedThisFrame)
        {
            SwitchCamera();
        }
    }

    private void SwitchCamera()
    {
        _isThirdPerson = !_isThirdPerson;

        if (_isThirdPerson)
        {
            thirdPersonCam.Priority = 10;
            firstPersonCam.Priority = 5;
        }
        else
        {
            thirdPersonCam.Priority = 5;
            firstPersonCam.Priority = 10;
        }
    }
}