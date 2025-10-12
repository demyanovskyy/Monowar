using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    public InputActionReference ineractActionRef;

    private IInteractable currentInteractable;

    public static bool isInteracting = false;

    private void OnEnable()
    {
        ineractActionRef.action.performed += TryToInreract;
    }

    private void OnDisable()
    {
        ineractActionRef.action.performed -= TryToInreract;
    }

    private void TryToInreract(InputAction.CallbackContext value)
    {
        if (currentInteractable != null && isInteracting ==  false)
        {
            currentInteractable.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            currentInteractable = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            currentInteractable = null;
        }
    }
}
