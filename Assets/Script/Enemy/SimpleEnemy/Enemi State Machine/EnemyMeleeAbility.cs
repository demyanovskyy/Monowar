using System.Collections;
using UnityEngine;

public class EnemyMeleeAbility : BaseAbilityEnemy
{

    private string meleeAnimParamiterName = "Melee";
    private int meleeParamiterID;


    public void EndOfAttack()
    {
        if (linkedPhysics.inAttackRange)
        {
            linkedAnimator.Play(meleeParamiterID, 0, 0);
        }
        else
        {
            linkedStateMachine.ChangeState((int)EnemyStates.State.Idle);
        }

        StartCoroutine(CheckBehindDelay());

    }
    
    IEnumerator CheckBehindDelay()
    {
        yield return new WaitForSeconds(linkedPhysics.behindDelay);
        linkedPhysics.canCheckBehind = true;
    }

    protected override void Initialization()
    {
        base.Initialization();
        meleeParamiterID = Animator.StringToHash(meleeAnimParamiterName);
        
    }


    public override void EnterAbility()
    {
        linkedPhysics.ResetVelocity();
        linkedPhysics.canCheckBehind = false;
    }

    public override void ProcessFixedAbility()
    {
    }
       

    public override void ProcessAbility()
    {
        if (!isParamited)
            return;

    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(meleeParamiterID, linkedStateMachine.curentState == (int)EnemyStates.State.MeleeAttak);
    }
}
