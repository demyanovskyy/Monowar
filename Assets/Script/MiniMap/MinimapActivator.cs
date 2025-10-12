using UnityEngine;
using UnityEngine.InputSystem;

public class MinimapActivator : MonoBehaviour
{
    public InputActionReference miniMapActionRef;
    private Player player;
    private bool mapActivated = false;
    [SerializeField] private CanvasGroup minimapCanvasGroup;


    private void OnEnable()
    {
        miniMapActionRef.action.performed += TryToOpenMiniMap;
    }

    private void OnDisable()
    {
        miniMapActionRef.action.performed -= TryToOpenMiniMap;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<Player>();
    }

    private void TryToOpenMiniMap(InputAction.CallbackContext value)
    {
        if (mapActivated)
        {
            //desable
            mapActivated =  !mapActivated;
            minimapCanvasGroup.alpha = 0;
            player.gatherInput.DisableMiniMap();
            if (player.stateMachine.curentState != (int)PlayerStates.State.Death && Interact.isInteracting == false)
                player.gatherInput.EnablePlayerMap();
        }
        else
        {
            //enable
            if (player.playerStats.GetCurrentHealth() <= 0)
                return;

            mapActivated = !mapActivated;
            minimapCanvasGroup.alpha = 1;
            player.gatherInput.DisablePlayerMap();
            player.gatherInput.EnableMiniMap();
        }
    }
}
