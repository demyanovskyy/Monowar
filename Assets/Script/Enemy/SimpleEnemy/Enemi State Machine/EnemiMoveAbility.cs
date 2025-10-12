using UnityEngine;

public class EnemiMoveAbility : BaseAbilityEnemy
{
    private string moveAnimParamiterName = "Move";
    private int moveParamiterID;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float minMoveTime;
    [SerializeField] private float maxMoveTime;
    [SerializeField] private float minimumTurnDelay;
    private float moveStartTime;
    private float turnCooldown;


    protected override void Initialization()
    {
        base.Initialization();
        moveParamiterID = Animator.StringToHash(moveAnimParamiterName);
        moveStartTime = Random.Range(minMoveTime, maxMoveTime);
    }

    public override void EnterAbility()
    {
        if (linkedPhysics.canCheckBehind)
        {
            if (linkedPhysics.playerBehind && turnCooldown <= 0)
            {
                enemy.ForceFlip();
                moveSpeed *= -1;
                turnCooldown = minimumTurnDelay;
            }
        }

        moveStartTime = Random.Range(minMoveTime, maxMoveTime);


    }

    public override void ProcessFixedAbility()
    {
        linkedPhysics.rb.linearVelocity = new Vector2(moveSpeed, linkedPhysics.rb.linearVelocityY);
    }

    public override void ProcessAbility()
    {
        if (!isParamited)
            return;

        moveStartTime -= Time.deltaTime;
        if (moveStartTime <= 0 && linkedPhysics.playerAhead == false)
        {
            linkedStateMachine.ChangeState((int)EnemyStates.State.Idle);
        }

        if(turnCooldown > 0)
            turnCooldown -= Time.deltaTime;

        // flip if behind detecteted
        if(linkedPhysics.playerBehind && turnCooldown <=0)
        {
            enemy.ForceFlip();
            moveSpeed *= -1;
            turnCooldown = minimumTurnDelay;
            return;
        }
        // flip if wall or not ground detected 
        if (linkedPhysics.wallDetected || linkedPhysics.grounded == false )
        {
            if (turnCooldown > 0)
                return;

            enemy.ForceFlip();
            moveSpeed *= -1;
            turnCooldown = minimumTurnDelay;
        }

        if (linkedPhysics.inAttackRange)
        {
            linkedStateMachine.ChangeState((int)EnemyStates.State.MeleeAttak);
        }
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(moveParamiterID, linkedStateMachine.curentState == (int)EnemyStates.State.Move);
    }
}
