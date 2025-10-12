using UnityEngine;
using UnityEngine.InputSystem;

public class ActivateCheckPoint : MonoBehaviour
{
    public InputActionReference activateCheckPoint;
    [SerializeField] CheckPointControll ceckControl;

    [HideInInspector]
    public CheckPoint checkPoint;

    private void OnEnable()
    {
        activateCheckPoint.action.performed += TryToActivateCheckPoint;
    }

    private void OnDisable()
    {
        activateCheckPoint.action.performed -= TryToActivateCheckPoint;
    }

    private void TryToActivateCheckPoint(InputAction.CallbackContext value)
    {
        if (checkPoint == null)
            return;

        // deactivated all
        ceckControl.ChecPointDeActivated();
        // activate
        checkPoint.ActivateCheckPoint();
 

    }
}
