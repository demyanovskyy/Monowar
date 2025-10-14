using UnityEngine;

public class EnemyPhysicsControl : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Ground")]
    [SerializeField] private float groundRayDistance;
    [SerializeField] private Transform groundPoint;
    [SerializeField] private LayerMask whatToGroundDetected;
    public bool grounded;
    private RaycastHit2D hitInfoGround;

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
    //public bool canWalkOnSlope;
    public Vector2 slopeNormalPerp;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float slopeDownAngleOld;


    [Header("Wall")]
    [SerializeField] private float wallRayDistance;
    [SerializeField] private Transform wallCheckPointUpper;
    [SerializeField] private Transform wallCheckPointLower;
    [SerializeField] private LayerMask whatToWallDetected;
    public bool wallDetected;
    private RaycastHit2D hitInfoWallUpper;
    private RaycastHit2D hitInfoWallLower;

    [Header("Colliders")]
    [SerializeField] private Collider2D attackDetectionCollider;
    [SerializeField] private Collider2D attackCollider;
    [SerializeField] private Collider2D statsCollider;
    public bool inAttackRange;

    [Header("Player Ahead")]
    [SerializeField] private Transform frontCheckPoint;
    [SerializeField] private float rayFrontCheckLength;
    private RaycastHit2D hitInfoCheckAhead;
    public bool playerAhead;


    [Header("Player Behind")]
    public bool canCheckBehind = true;
    [SerializeField] private Transform backCheckPoint;
    [SerializeField] private float rayBackCheckLength;
    private RaycastHit2D hitInfoCheckBehaind;
    public bool playerBehind;
    public float behindDelay;

    [SerializeField] private LayerMask playerDetectMask;



 
    public void ActivatedAttackCollider()
    {
        attackCollider.enabled = true;
    }

    public void DeactivatedAttackCollider()
    {
        attackCollider.enabled = false;
    }

    public void DeathColliderDeactivation()
    {
        DeactivatedAttackCollider();
        attackDetectionCollider.enabled = false;
        statsCollider.enabled = false;
    }

    private void FixedUpdate()
    {
        grounded = CheckGround();
        slopDetected = CheckSlop();
        //canWalkOnSlope = CheckCanWalkOnSlope();
        wallDetected = CheckWall();
        playerAhead = CheckAhead();
        if (canCheckBehind)
            playerBehind = CheckBehaind();

    }

    public bool CheckAhead()
    {
        hitInfoCheckAhead = Physics2D.Raycast(frontCheckPoint.position, transform.right, rayFrontCheckLength, playerDetectMask);
        if (hitInfoCheckAhead)
            return true;

        return false;
    }
    public bool CheckBehaind()
    {
        hitInfoCheckBehaind = Physics2D.Raycast(backCheckPoint.position, transform.right*(-1), rayBackCheckLength, playerDetectMask);
        if (hitInfoCheckBehaind)
            return true;

        return false;
    }

    public void ResetVelocity()
    {
        rb.linearVelocity = Vector2.zero;
    }

    private bool CheckGround()
    {
          hitInfoGround = Physics2D.Raycast(groundPoint.position, Vector2.down, groundRayDistance, whatToGroundDetected);

        if (hitInfoGround)
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

    //public bool CheckCanWalkOnSlope()
    //{
    //    if (slopeDownAngle > maxSlopAngle || slopeSideAngle > maxSlopAngle)
    //    {
    //        rb.sharedMaterial = frictionOff;
    //        return false;
    //    }
    //    else
    //    {
    //        return true;
    //    }
    //}

    private bool CheckWall()
    {
        hitInfoWallUpper = Physics2D.Raycast(wallCheckPointUpper.position, transform.right, wallRayDistance, whatToWallDetected);
        hitInfoWallLower = Physics2D.Raycast(wallCheckPointLower.position, transform.right, wallRayDistance, whatToWallDetected);

        if (hitInfoWallUpper || hitInfoWallLower)
            return true;

        return false;

    }

    private void OnDrawGizmos()
    {
        //======Ground=============
        Debug.DrawRay(groundPoint.position, new Vector3(0, -groundRayDistance, 0), Color.red);
        //=========================

        //=======Wall==============
        Debug.DrawRay(wallCheckPointUpper.position, new Vector3(wallRayDistance, 0, 0), Color.red);
        Debug.DrawRay(wallCheckPointLower.position, new Vector3(wallRayDistance, 0, 0), Color.green);
        //=========================

        //=======Slop==============
        Debug.DrawRay(slopCheckPoint.position, new Vector3(0, -slopRayVerticalDistance, 0), Color.white);
        Debug.DrawRay(slopCheckPoint.position, new Vector3(slopRayHorizontalDistance, 0, 0), Color.white);
        //=========================

        //==Ahead=================
        Debug.DrawLine(frontCheckPoint.position, frontCheckPoint.position + transform.right*rayFrontCheckLength, Color.yellow);
        //==Behind=================
        Debug.DrawLine(backCheckPoint.position, backCheckPoint.position + transform.right*(-1) * rayBackCheckLength, Color.yellow);
    }
}
