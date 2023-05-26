using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TeleportController : MonoBehaviour
{
    
    [SerializeField] private GameObject baseController;
    [SerializeField] private GameObject teleportationController;
    [SerializeField] private InputActionReference teleportActivationReference;

    [Header("Events")]
    public UnityEvent onTeleportActivate;
    public UnityEvent onTeleportCancel;
    // Start is called before the first frame update
    void Start()
    {
        teleportActivationReference.action.performed += TeleportModeActivate;
        teleportActivationReference.action.canceled += TeleportModeCancel;
    }

    private void TeleportModeCancel(InputAction.CallbackContext obj) => Invoke("DeactivateTeleporter", .1f);
    void DeactivateTeleporter() => onTeleportCancel.Invoke();
    private void TeleportModeActivate(InputAction.CallbackContext obj) => onTeleportActivate.Invoke();

}