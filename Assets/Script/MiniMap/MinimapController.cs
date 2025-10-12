using UnityEngine;
using UnityEngine.InputSystem;

public class MinimapController : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Camera minimapCamera;

    [Header("Move Setings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dragSpeed;

    [Header("Zoom Setings")]
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;

    public InputActionReference moveActionRef;
    public InputActionReference zoomActionRef;
    public InputActionReference middleActionRef;
    public InputActionReference deltaActionRef;


    // Update is called once per frame
    void Update()
    {
        HandleZoom();
        HandlMove();
    }

    private void HandleZoom()
    {
        float scroll = zoomActionRef.action.ReadValue<float>();
        minimapCamera.orthographicSize = Mathf.Clamp(minimapCamera.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
    }

    private void HandlMove()
    {
        float zoomFactor = minimapCamera.orthographicSize;
        Vector2 moveInput = moveActionRef.action.ReadValue<Vector2>();
        if (moveInput != Vector2.zero)
        {
            Vector3 move = new Vector3(moveInput.x, moveInput.y, 0) * zoomFactor* moveSpeed*Time.deltaTime;

            minimapCamera.transform.position += move;
        }
        if(middleActionRef.action.IsPressed())
        {
            Vector2 delta = deltaActionRef.action.ReadValue<Vector2>();
            if(delta != Vector2.zero)
            {
                Vector3 dragMove = new Vector3(-delta.x, -delta.y, 0) * zoomFactor * dragSpeed * Time.deltaTime;

                minimapCamera.transform.position += dragMove;
            }
        }
    }
}
