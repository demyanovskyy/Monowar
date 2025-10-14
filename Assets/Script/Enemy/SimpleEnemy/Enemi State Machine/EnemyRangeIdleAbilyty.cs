using UnityEngine;

public class EnemyRangeIdleAbilyty : BaseAbilityEnemy
{

    private string idleAnimParamiterName = "Idle";
    private int idleParamiterID;

    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;
    private float idleStartTime;

    protected override void Initialization()
    {
        base.Initialization();
        idleParamiterID = Animator.StringToHash(idleAnimParamiterName);
        idleStartTime = Random.Range(minIdleTime, maxIdleTime);
    }
    public override void EnterAbility()
    {
        idleStartTime = Random.Range(minIdleTime, maxIdleTime);
        linkedPhysics.ResetVelocity();
    }

    public override void ProcessAbility()
    {
        if (!isParamited)
            return;

        if (linkedPhysics.playerBehind)
        {
            linkedStateMachine.ChangeState((int)EnemyStates.State.Move);
        }

        idleStartTime -= Time.deltaTime;
        if (idleStartTime <= 0 || linkedPhysics.playerAhead == false)
        {
            linkedStateMachine.ChangeState((int)EnemyStates.State.Move);
        }

        if (linkedPhysics.inAttackRange)
        {
            linkedStateMachine.ChangeState((int)EnemyStates.State.MeleeAttak);
        }
        else
        if (linkedPhysics.playerAhead)
        {
            linkedStateMachine.ChangeState((int)EnemyStates.State.Shoot);
        }
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(idleParamiterID, linkedStateMachine.curentState == (int)EnemyStates.State.Idle);
    }
}
