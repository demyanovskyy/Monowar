//using UnityEngine;

//public class MovingPlatform : MonoBehaviour
//{
//    [Header("Object To Move And Movement Speed")]
//    [SerializeField] GameObject gameObject;
//    [SerializeField] public float moveSpeed;

//    [Header("Waypoints For Object To Follow")]
//    [SerializeField] Transform[] waypoints;
//    [SerializeField] int firstWaypoint;

//    [Header("Movement Options")]
//    [SerializeField] bool useReverse = false;
//    [SerializeField] bool useFlip = false;
//    [SerializeField] bool facingRight = false;
//    [SerializeField] SpriteRenderer spriteRenderer;
//    [SerializeField] Animator animator;

//    Transform nextWaypoint;
//    public bool reverse;


//    private bool _playing;
//    private Rigidbody2D _rb;


//    private Player player;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        nextWaypoint = waypoints[firstWaypoint];

//        _playing = true;

//        _rb = GetComponentInChildren<Rigidbody2D>();

//    }
//    private void FixedUpdate()
//    {
//        Vector2 movementThisFrame = Vector2.MoveTowards(gameObject.transform.position, nextWaypoint.position, moveSpeed * Time.deltaTime);
//        Vector2 vel = movementThisFrame - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);///Time.deltaTime;
//        _rb.linearVelocity = vel;


//    }
//    void Update()
//    {

//        // gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextWaypoint.position, Time.deltaTime * moveSpeed);
//        if (reverse)
//        {
//            Reverse();
//        }
//        else
//        {
//            Forward();
//        }


//    }


//    #region User Methods
//    void Forward()
//    {
//        // if (gameObject.transform.position == nextWaypoint.position)
//        if (Vector2.Distance(gameObject.transform.position, nextWaypoint.position) < 0.1)

//        {
//            firstWaypoint++;

//            if (useFlip) Flip();

//            if (firstWaypoint == waypoints.Length)
//            {
//                if (useReverse)
//                {
//                    reverse = true;
//                    firstWaypoint = waypoints.Length - 1;
//                }
//                else
//                {
//                    firstWaypoint = 0;
//                }
//            }
//            nextWaypoint = waypoints[firstWaypoint];
//        }
//    }

//    void Reverse()
//    {
//        //if (gameObject.transform.position == nextWaypoint.position)
//        if (Vector2.Distance(gameObject.transform.position, nextWaypoint.position) < 0.1)
//        {
//            firstWaypoint--;

//            if (firstWaypoint == 0)
//            {
//                reverse = false;
//                firstWaypoint = 0;
//            }
//            nextWaypoint = waypoints[firstWaypoint];
//        }
//    }

//    void Flip()
//    {
//        facingRight = !facingRight;

//        if (facingRight)
//        {
//            spriteRenderer.flipX = true;
//            animator.SetBool("FacingRight", true);
//        }
//        else
//        {
//            spriteRenderer.flipX = false;
//            animator.SetBool("FacingRight", false);
//        }
//    }
//    #endregion
//    private void OnDrawGizmos()
//    {
//        if (transform.hasChanged && !_playing)
//        {
//            // _currentPosition = transform.position;
//        }

//        if (waypoints != null)
//        {
//            for (int i = 0; i < waypoints.Length; i++)
//            {
//                if (i < waypoints.Length)
//                {
//                    // Draw points
//                    Gizmos.color = Color.red;
//                    Gizmos.DrawWireSphere(waypoints[i].position, 0.4f);

//                    // Draw lines
//                    Gizmos.color = Color.black;
//                    if (i < waypoints.Length - 1)
//                    {
//                        Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
//                    }

//                    // Draw line from last point to first point
//                    if (i == waypoints.Length - 1)
//                    {
//                        Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
//                    }
//                }
//            }
//        }
//    }



//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            // chect top of platform contact
//            foreach (ContactPoint2D contactPoint in collision.contacts)
//            {
//                if (contactPoint.normal.y < -0.5f)
//                {
//                    collision.transform.SetParent(this.transform);
//                    player = collision.gameObject.GetComponent<Player>();
//                    player.physicsControl.SetExtrapolate();
//                    break;
//                }
//            }
//        }
//    }

//    private void OnCollisionExit2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            if (gameObject.activeInHierarchy == true)
//            {
//                collision.transform.SetParent(null);
//                if (player != null)
//                    player.physicsControl.SetInterpolate();
//                player = null;
//            }
//        }
//    }

//}


using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private int startIndex;
    [SerializeField] private Transform[] points;
    private int targetIndex;

    private Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetIndex = startIndex;
        transform.position = points[targetIndex].position;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, points[targetIndex].position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, points[targetIndex].position) < 0.05f)
        {
            targetIndex++;

            if (targetIndex == points.Length)
            {
                targetIndex = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // chect top of platform contact
            foreach (ContactPoint2D contactPoint in collision.contacts)
            {
                if (contactPoint.normal.y < -0.5f)
                {
                    collision.transform.SetParent(this.transform);
                    player = collision.gameObject.GetComponent<Player>();
                    player.physicsControl.SetExtrapolate();
                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.activeInHierarchy == true)
            {
                collision.transform.SetParent(null);
                if (player != null)
                    player.physicsControl.SetInterpolate();
                player = null;
            }
        }
    }

}
