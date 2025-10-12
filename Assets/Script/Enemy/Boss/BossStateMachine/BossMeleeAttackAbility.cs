using System.Collections;
using UnityEngine;

public class BossMeleeAttackAbility : BaseAbilityBoss
{
    private string meleeAnimParamiterName = "MeleeAttack";
    private int meleeParamiterID;

    [SerializeField] private float attackMeleeCooldownTimer;
    private float meleeAttackTimer;

    public float GetMeleeAttackTimer()
    {
        return meleeAttackTimer; 
    }

    public void ChangeStateToIdle()
    {
        linkedStateMachine.ChangeState((int)BossStates.State.Idle);
    }

    protected override void Initialization()
    {
        base.Initialization();
        meleeParamiterID = Animator.StringToHash(meleeAnimParamiterName);
        meleeAttackTimer = attackMeleeCooldownTimer;
    }

    public override void ProcessAbility()
    {
        if (!isParamited)
            return;
    }

    public override void EnterAbility()
    {
        linkedPhysics.DesableAttackDetectionCollider();
        linkedPhysics.inAttackRange = false;
    }

    public override void ExitAbility()
    {
        meleeAttackTimer = attackMeleeCooldownTimer;
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(meleeParamiterID, linkedStateMachine.curentState == (int)BossStates.State.MeleeAttack);
    }
}
