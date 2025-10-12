using UnityEngine;

public class PlayerPhysicsControl : MonoBehaviour
{
    [Header("RigitBody2D paramiter")]
    public Rigidbody2D rb;


    [Header("Slop paramiter")]
    [SerializeField] private float maxSlopAngle;
    [SerializeField] private float slopRayVerticalDistance;
    [SerializeField] private float slopRayHorizontalDistance;
    [SerializeField] private Transform slopCheckPoint;
    [SerializeField] private LayerMask whatToSlopDetected;
    [SerializeField] PhysicsMaterial2D frictionOn;
    [SerializeField] PhysicsMaterial2D frictionOff;
    public bool slopDetected;
    public bool isOnSlope;
    public bool canWalkOnSlope;
    public Vector2 slopeNormalPerp;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float slopeDownAngleOld;




    [Header("Coyot Time")]
    [SerializeField] private float coyotSetTimer;
    [HideInInspector] public float coyotTimer;

    [Header("Ground")]
    [SerializeField] private float groundRayDistance;
    [SerializeField] private Transform leftGroundPoint;
    [SerializeField] private Transform rightGroundPoint;
    [SerializeField] private LayerMask whatToGroundDetected;
    public bool grounded;
    private RaycastHit2D hitInfoGroundLeft;
    private RaycastHit2D hitInfoGroundRight;

    [Header("Wall")]
    [SerializeField] private float wallRayDistance;
    [SerializeField] private Transform wallCheckPointUpper;
    [SerializeField] private Transform wallCheckPointLower;
    [SerializeField] private LayerMask whatToWallDetected;
    public bool wallDetected;
    private RaycastHit2D hitInfoWallUpper;
    private RaycastHit2D hitInfoWallLower;

    [Header("Ceiling")]
    [SerializeField] private float ceilinglRayDistance;
    [SerializeField] private Transform leftCeilingPoint;
    [SerializeField] private Transform rightCeilingPoint;
    //[SerializeField] private LayerMask whatToWallDetected;
    public bool ceilingDetected;
    private RaycastHit2D hitInfoCelingRight;
    private RaycastHit2D hitInfoCeilingLeft;

    [Header("Colliders")]
    [SerializeField] private Collider2D standCollider;
    [SerializeField] private Collider2D crouchCollider;

    [Header("Interpolation")]// ned for platform
    public RigidbodyInterpolation2D interpolate;
    public RigidbodyInterpolation2D extrapolate;

    private float gravityValue;


    public float GetGravity()
    {
        return gravityValue;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gravityValue = rb.gravityScale;
        coyotTimer = coyotSetTimer;

    }

    private bool CheckWall()
    {
        hitInfoWallUpper = Physics2D.Raycast(wallCheckPointUpper.position, transform.right, wallRayDistance, whatToWallDetected);
        hitInfoWallLower = Physics2D.Raycast(wallCheckPointLower.position, transform.right, wallRayDistance, whatToWallDetected);

        if (hitInfoWallUpper || hitInfoWallLower)
            return true;

        return false;

    }

    private bool CheckCeiling()
    {
        hitInfoCeilingLeft = Physics2D.Raycast(leftCeilingPoint.position, Vector2.up, ceilinglRayDistance, whatToGroundDetected);
        hitInfoCelingRight = Physics2D.Raycast(rightCeilingPoint.position, Vector2.up, ceilinglRayDistance, whatToGroundDetected);

        if (hitInfoCeilingLeft || hitInfoCelingRight)
            return true;

        return false;
    }

    private bool CheckGround()
    {
        hitInfoGroundLeft = Physics2D.Raycast(leftGroundPoint.position, Vector2.down, groundRayDistance, whatToGroundDetected);
        hitInfoGroundRight = Physics2D.Raycast(rightGroundPoint.position, Vector2.down, groundRayDistance, whatToGroundDetected);



        if (hitInfoGroundLeft || hitInfoGroundRight)
            return true;

        return false;
    }
    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopHitFront = Physics2D.Raycast(checkPos, transform.right, slopRayHorizontalDistance, whatToSlopDetected);
        if (slopHitFront)
        {
           // isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopHitFront.normal, Vector2.up);
        }
        else
        {
           // isOnSlope = false;
            slopeSideAngle = 0.0f;
        }

        Debug.DrawRay(slopHitFront.point, Vector2.Perpendicular(slopHitFront.normal).normalized, Color.yellow);
        Debug.DrawRay(slopHitFront.point, slopHitFront.normal, Color.magenta);
    }
    private void SlopeCheckVectical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopRayVerticalDistance, whatToSlopDetected);

        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld)
            {
                isOnSlope = true;
            }
            else
            {
                isOnSlope = false;
            }
                //slopeDownAngleOld = slopeDownAngle; ;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }
    }
    private bool CheckSlop()
    {
        SlopeCheckHorizontal(slopCheckPoint.position);
        SlopeCheckVectical(slopCheckPoint.position);

        if (isOnSlope)
        {
            rb.sharedMaterial = frictionOn;
            return true;
        }
        else
        {
            rb.sharedMaterial = frictionOff;
            return false;
        }
    }

    public bool CheckCanWalkOnSlope()
    {
        if (slopeDownAngle > maxSlopAngle || slopeSideAngle > maxSlopAngle)
        {
            rb.sharedMaterial = frictionOff;
            return false;
        }
        else
        {
            return true;
        }
    }

    private void Update()
    {
        if (!grounded)
        {
            coyotTimer -= Time.deltaTime;
        }
        else
        {
            coyotTimer = coyotSetTimer;
        }


    }

    private void FixedUpdate()
    {
        grounded = CheckGround();
        wallDetected = CheckWall();
        ceilingDetected = CheckCeiling();
        slopDetected = CheckSlop();
        canWalkOnSlope = CheckCanWalkOnSlope();



    }

    public void DisableGravity()
    {
        rb.gravityScale = 0;
    }

    public void EnableGravity()
    {
        rb.gravityScale = gravityValue;
    }

    public void ResetVelocity()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public void StandColliderEneble()
    {
        standCollider.enabled = true;
        crouchCollider.enabled = false;
    }

    public void CrouchColliderEneble()
    {
        standCollider.enabled = false;
        crouchCollider.enabled = true;
    }

    public void SetInterpolate()
    {
        rb.interpolation = interpolate;
    }

    public void SetExtrapolate()
    {
        rb.interpolation = extrapolate;
    }


    private void OnDrawGizmos()
    {
        //======Ground=============
        Debug.DrawRay(leftGroundPoint.position, new Vector3(0, -groundRayDistance, 0), Color.red);
        Debug.DrawRay(rightGroundPoint.position, new Vector3(0, -groundRayDistance, 0), Color.green);
        //=========================

        //======Celing=============
        Debug.DrawRay(leftCeilingPoint.position, new Vector3(0, ceilinglRayDistance, 0), Color.red);
        Debug.DrawRay(rightCeilingPoint.position, new Vector3(0, ceilinglRayDistance, 0), Color.green);
        //=========================

        //=======Wall==============
        Debug.DrawRay(wallCheckPointUpper.position, new Vector3(wallRayDistance, 0, 0), Color.red);
        Debug.DrawRay(wallCheckPointLower.position, new Vector3(wallRayDistance, 0, 0), Color.green);
        //=========================

        //=======Slop==============
        Debug.DrawRay(slopCheckPoint.position, new Vector3(0, -slopRayVerticalDistance, 0), Color.white);
        Debug.DrawRay(slopCheckPoint.position, new Vector3(slopRayHorizontalDistance, 0, 0), Color.white);
        //=========================
    }
}
