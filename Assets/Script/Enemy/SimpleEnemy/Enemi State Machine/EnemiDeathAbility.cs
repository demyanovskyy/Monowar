using System.Collections;
using UnityEngine;

public class EnemiDeathAbility : BaseAbilityEnemy
{

    private string deathAnimParamiterName = "Death";
    private int deathParamiterID;
    private bool destroy = false;

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
        enemy.DeactivateFoV();
        enemy.DeactivateRotateobject();
        

    }

    public override void ProcessFixedAbility()
    {
        if(destroy)
            StartCoroutine(Destroy());
    }

    public override void ProcessAbility()
    {
        if (!isParamited)
            return;

        destroy = true;
        
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(deathParamiterID, linkedStateMachine.curentState == (int)EnemyStates.State.Death);
    }


    IEnumerator Destroy()
    {

        yield return new WaitForSeconds(6f);
        Destroy(enemy.gameObject);
    }
}
