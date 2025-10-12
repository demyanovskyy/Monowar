using UnityEngine;

public class FlyEnemyMoveAbility : BaseAbilityFlyEnemy
{
    private Player player;

    private string moveAnimParamiterName = "Move";
    private int moveParamiterID;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float returnSpeed;
    [SerializeField] private Transform startPoint;
    [SerializeField] private float weithTime;
    private float weithTimerColdown;

    private Vector2 moveDerection;

    protected override void Initialization()
    {
        base.Initialization();
        moveParamiterID = Animator.StringToHash(moveAnimParamiterName);
        player = FindAnyObjectByType<Player>();
    }

    public override void EnterAbility()
    {
        weithTimerColdown = weithTime;
    }

    public override void ExitAbility()
    {
        weithTimerColdown = weithTime;
    }

    public override void ProcessFixedAbility()
    {

    }

    public override void ProcessAbility()
    {
        if (!isParamited)
            return;



        if (linkedPhysics.inAttackRange)
        {
            weithTimerColdown = weithTime;
            FollowToPlayer();
        }
        else
        {
            weithTimerColdown -= Time.deltaTime;

            if (weithTimerColdown <= 0)
                ReturToStartPoint();
        }


    }

    public void FollowToPlayer()
    {

        FlipMove(player.transform);

        flyEnemy.transform.position = Vector2.MoveTowards(flyEnemy.transform.position, player.transform.position, moveSpeed * Time.deltaTime);


    }

    public void ReturToStartPoint()
    {
        FlipMove(startPoint);

        flyEnemy.transform.position = Vector2.MoveTowards(flyEnemy.transform.position, startPoint.position, returnSpeed * Time.deltaTime);

        if (Vector2.Distance(flyEnemy.transform.position, startPoint.position) <= 0.1f)
        {
            linkedStateMachine.ChangeState((int)FlyEnemyStates.State.Idle);
        }

    }

    public void FlipMove(Transform target)
    {
        if ((flyEnemy.transform.position.x > target.position.x) && flyEnemy.facingRight)
        {
            flyEnemy.facingRight = true;
            flyEnemy.ForceFlip();
        }
        else
     if ((flyEnemy.transform.position.x < target.position.x) && !flyEnemy.facingRight)
        {
            flyEnemy.facingRight = false;
            flyEnemy.ForceFlip();
        }
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(moveParamiterID, linkedStateMachine.curentState == (int)FlyEnemyStates.State.Move);
    }
}
