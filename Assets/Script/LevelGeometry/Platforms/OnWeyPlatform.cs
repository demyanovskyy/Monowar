using UnityEngine;

public class OnWeyPlatform : MonoBehaviour
{

    private GetherInput playerInput;
    private PlatformEffector2D platformEffecor2D;

    private void Awake()
    {
        platformEffecor2D = GetComponent<PlatformEffector2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // get player control
            playerInput = collision.gameObject.GetComponent<GetherInput>();

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (playerInput == null)
            return;

        if(playerInput.jumpOnWey)
        {
            platformEffecor2D.rotationalOffset = 180;
            playerInput = null;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        playerInput = null;
        platformEffecor2D.rotationalOffset = 0;
    }
}
