using UnityEngine;

public class FlyEnemyIdleAmility : BaseAbilityFlyEnemy
{
    private string idleAnimParamiterName = "Idle";
    private int idleParamiterID;

     
    protected override void Initialization()
    {
        base.Initialization();
        idleParamiterID = Animator.StringToHash(idleAnimParamiterName);
        
    }


    public override void ProcessAbility()
    {
        if (!isParamited)
            return;

        if (linkedPhysics.inAttackRange)
            linkedStateMachine.ChangeState((int)FlyEnemyStates.State.Move);
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(idleParamiterID, linkedStateMachine.curentState == (int)BossStates.State.Idle);
    }

}
