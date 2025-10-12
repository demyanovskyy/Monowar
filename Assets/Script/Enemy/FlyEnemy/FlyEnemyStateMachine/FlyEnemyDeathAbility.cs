using UnityEngine;

public class FlyEnemyDeathAbility : BaseAbilityFlyEnemy
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
        linkedPhysics.DesableAllColliders();
        linkedPhysics.rb.bodyType = RigidbodyType2D.Dynamic;
    }
    public override void ProcessAbility()
    {
        if (!isParamited)
            return;

    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(deathParamiterID, linkedStateMachine.curentState == (int)FlyEnemyStates.State.Death);
    }
}
