using UnityEngine;

public class EnemiDeathAbility : BaseAbilityEnemy
{

    private string deathAnimParamiterName = "Death";
    private int deathParamiterID;

    protected override void Initialization()
    {
        base.Initialization();
        deathParamiterID = Animator.StringToHash(deathAnimParamiterName);
       
    }
    public override void EnterAbility()
    {
        linkedPhysics.ResetVelocity();
        
        linkedPhysics.DeathColliderDeactivation();

        //=======================================
        enemy.DeactivateRotateobject();
        enemy.DeactivateFoV();

    }

    public override void ProcessAbility()
    {
        if (!isParamited)
            return;

    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(deathParamiterID, linkedStateMachine.curentState == (int)EnemyStates.State.Death);
    }

}
