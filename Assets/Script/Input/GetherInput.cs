using UnityEngine;
using UnityEngine.InputSystem;

public class GetherInput : MonoBehaviour
{

    public PlayerInput playerInput;

    private InputActionMap playerMap;
    private InputActionMap uiMap;
    private InputActionMap miniMap;
    private InputActionMap activatorMap;
    private InputActionMap dialogueMap;

    public InputActionReference mouselActionRef;
    public InputActionReference horizontalActionRef;
    public InputActionReference verticalActionRef;
    public InputActionReference jumpOnWeyActionRef;

    public InputActionReference dialogueActionRef;

    [HideInInspector]
    public Vector2 mouseInput;
    [HideInInspector]
    public Vector2 mouseInputWorldPosition;
    [HideInInspector]
    public float horizontalInput;
    [HideInInspector]
    public float verticalInput;

    //[HideInInspector]
    public bool jumpOnWey = false;

    private void OnEnable()
    {

        jumpOnWeyActionRef.action.performed += TryToJumpOnWeyPlatform;
        jumpOnWeyActionRef.action.canceled += EndToJumpOnWeyPlatform;
        dialogueActionRef.action.performed += TryToContinueDialogue;
    }

    private void OnDisable()
    {
        jumpOnWeyActionRef.action.performed -= TryToJumpOnWeyPlatform;
        jumpOnWeyActionRef.action.canceled -= EndToJumpOnWeyPlatform;
        dialogueActionRef.action.performed -= TryToContinueDialogue;

        playerMap.Disable();
        activatorMap.Disable();
        dialogueMap.Disable();
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMap = playerInput.actions.FindActionMap("Player");
        uiMap = playerInput.actions.FindActionMap("UI");
        miniMap = playerInput.actions.FindActionMap("MinimapControls");
        activatorMap = playerInput.actions.FindActionMap("Activators");
        dialogueMap = playerInput.actions.FindActionMap("DialogueControl");
        playerMap.Enable();
        activatorMap.Enable();
        dialogueMap.Disable();

        ServiceLocator.Current.Get<DialogueManeger>().RegesterGetherInput(this);


        // Debug.Log("InputMap start:" + playerMap.name);
    }

    #region Player Map
    // Update is called once per frame
    void Update()
    {
        mouseInput = mouselActionRef.action.ReadValue<Vector2>();
        mouseInputWorldPosition = Camera.main.ScreenToWorldPoint(mouseInput);
        horizontalInput = horizontalActionRef.action.ReadValue<float>();
        verticalInput = verticalActionRef.action.ReadValue<float>();

       // Debug.Log("Horizontal input:" + horizontalInput);

    }

    public void EnablePlayerMap()
    {
        playerMap.Enable();
    }

    public void DisablePlayerMap()
    {
        playerMap.Disable();

    }
    #endregion

    #region Mini Map
    public void EnableMiniMap()
    {
        miniMap.Enable();
    }

    public void DisableMiniMap()
    {
        miniMap.Disable();
    }
    #endregion

    #region Dialogue Map
    public void EnableDialogueMap()
    {
        dialogueMap.Enable();
    }

    public void DisableDialogueMap()
    {
        dialogueMap.Disable();
    }

    public void DialogueActivated()
    {
        DisablePlayerMap();
        EnableDialogueMap();
    }

    public void DialogueDeactivated()
    {
        EnablePlayerMap();
        DisableDialogueMap();
    }

    private void TryToContinueDialogue(InputAction.CallbackContext value)
    {
        ServiceLocator.Current.Get<DialogueManeger>().ContinueDialogue();
    }
    #endregion

    #region OnWeyPlatform Input
    private void TryToJumpOnWeyPlatform(InputAction.CallbackContext value)
    {
        jumpOnWey = true;
    }
    private void EndToJumpOnWeyPlatform(InputAction.CallbackContext value)
    {
        jumpOnWey = false;
    }
    #endregion

}
