using UnityEngine;

public class IdleAbility : BaseAbilityPlayer
{
    private string idleAnimParamiterName = "Idle";
    private int idleParamiterID;

    public override void EnterAbility()
    {
        // linkedPhysics.rb.linearVelocityX = 0;
        linkedPhysics.ResetVelocity();
    }

    protected override void Initialization()
    {
        base.Initialization();
        idleParamiterID = Animator.StringToHash(idleAnimParamiterName);
    }

    public override void ProcessAbility()
    {
        if (!isParamited)
            return;

        if (linkedPhysics.wallDetected == true)
             return;
 

        if (linkedPhysics.slopDetected == false && linkedInput.horizontalInput == 0)
        {
            linkedPhysics.ResetVelocity();
        }


        if (linkedInput.horizontalInput != 0)
        {
            entety.Flip();
            linkedStateMachine.ChangeState((int)PlayerStates.State.Move);
        }
        if(linkedPhysics.grounded ==  false)
            linkedStateMachine.ChangeState((int)PlayerStates.State.Jump);

    }

    public override void UpdateAnimator()
    {
        // for reload its trick
        linkedAnimator.SetBool(idleParamiterID, linkedStateMachine.curentState == (int)PlayerStates.State.Idle 
            || linkedStateMachine.curentState == (int)PlayerStates.State.Reload);
    }
}
