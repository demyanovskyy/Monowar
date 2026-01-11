using UnityEngine;

public class Player : Entety
{
    public GetherInput gatherInput;

    public PlayerPhysicsControl physicsControl;

    public PlayerStats playerStats;

    public Transform graficData;

    public RotateObject rotateObject;

    private float vievDirection;

   // readonly Quaternion flippedRotation = new Quaternion(0, -180, 0, 0);

    //[Header("Tail")]
    //[SerializeField] Transform tailAnchor = null;
    //[SerializeField] Rigidbody2D tailRigidbody = null;

    private void Awake()
    {
        // init state machine
        stateMachine = new StateMachine();
        abilitys = GetComponents<BaseAbility>();
        stateMachine.arrayOfAbilities = abilitys;

        // init rotate object
        rotateObject.Initialized(facingRight);

        if (facingRight)
            vievDirection = 1;
        else
            vievDirection = -1;

    }

    //private void UpdateTailPose()
    //{
    //    // Calculate the extrapolated target position of the tail anchor.
    //    Vector2 targetPosition = tailAnchor.position;
    //    targetPosition += physicsControl.rb.linearVelocity * Time.fixedDeltaTime;

    //    tailRigidbody.MovePosition(targetPosition);
    //    if (facingRight)
    //        tailRigidbody.SetRotation(tailAnchor.rotation * flippedRotation);
    //    else
    //        tailRigidbody.SetRotation(tailAnchor.rotation);
    //}

    private void Update()
    {
        foreach (BaseAbilityPlayer ability in abilitys)
        {
            if (ability.thisAbilityState == stateMachine.curentState)
            {
                ability.ProcessAbility();
            }
            ability.UpdateAnimator();
        }
       // Debug.Log("Curent state is:" + gameObject.name + ":" + stateMachine.curentState);


        rotateObject.UpdateRotateObjectParamites(gatherInput.mouseInputWorldPosition, Mathf.Sign(vievDirection));

        Flip();
    }

    private void FixedUpdate()
    {

        foreach (BaseAbilityPlayer ability in abilitys)
        {
            if (ability.thisAbilityState == stateMachine.curentState)
            {
                ability.ProcessFixedAbility();
            }
        }

      //  UpdateTailPose();
    }



    public override void Flip()
    {
        if (facingRight == true && gatherInput.horizontalInput < 0)
        {
            transform.Rotate(0, 180, 0);
            facingRight = !facingRight;
            vievDirection *= -1;
        }
        else
        if (facingRight == false && gatherInput.horizontalInput > 0)
        {
            transform.Rotate(0, 180, 0);
            facingRight = !facingRight;
            vievDirection *= -1;
        }
    }

    public override void ForceFlip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        vievDirection *= -1;
    }

    Quaternion rotZ;


 
}
