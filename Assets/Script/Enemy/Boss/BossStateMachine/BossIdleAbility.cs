using UnityEngine;

public class BossIdleAbility : BaseAbilityBoss
{
    private string idleAnimParamiterName = "Idle";
    private int idleParamiterID;

    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;
    private float idleStartTime;

    private float meleeTimer;
    protected override void Initialization()
    {
        base.Initialization();
        idleParamiterID = Animator.StringToHash(idleAnimParamiterName);
        idleStartTime = Random.Range(minIdleTime, maxIdleTime);
    }

    public override void EnterAbility()
    {
        meleeTimer = GetComponent<BossMeleeAttackAbility>().GetMeleeAttackTimer();
    }

    public override void ProcessAbility()
    {
        if (!isParamited)
            return;

        meleeTimer -= Time.deltaTime;

        if (linkedPhysics.inAttackRange)
        {
            if (meleeTimer <= 0)
                linkedStateMachine.ChangeState((int)BossStates.State.MeleeAttack);
            return;
        }

        idleStartTime -= Time.deltaTime;

        if (idleStartTime <= 0)
            linkedStateMachine.ChangeState((int)BossStates.State.Teleport);
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(idleParamiterID, linkedStateMachine.curentState == (int)BossStates.State.Idle);
    }
}
