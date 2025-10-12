using UnityEngine;

public class Kosa : MonoBehaviour
{

    readonly Vector3 flippedScale = new Vector3(-1, 1, 1);
    readonly Quaternion flippedRotation = new Quaternion(0, 0, -1, 0);

    public Rigidbody2D controllerRigidbody;

    [Header("Tail")]
    [SerializeField] Transform tailAnchor = null;
    [SerializeField] Rigidbody2D tailRigidbody = null;
    Vector2 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        controllerRigidbody = GetComponent<Rigidbody2D>();
        targetPosition = tailAnchor.position;
    }

    // Update is called once per frame

    void FixedUpdate()
    {

        UpdateTailPose();

    }
    private void UpdateTailPose()
    {
        // Calculate the extrapolated target position of the tail anchor.
        targetPosition = tailAnchor.position;
       targetPosition += controllerRigidbody.linearVelocity * Time.fixedDeltaTime;

        tailRigidbody.MovePosition(targetPosition);



        if (transform.localScale.x < 0)
            tailRigidbody.SetRotation(tailAnchor.rotation * flippedRotation);
        else
            tailRigidbody.SetRotation(tailAnchor.rotation);
    }


}
