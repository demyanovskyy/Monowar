using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : BaseAbilityPlayer
{


    public InputActionReference meleAttackActionRef;


    private string meleeAttackAnimParamiterName = "MeleeAttack";
    private int meleeAttackParamiterID;

    private void OnEnable()
    {
        meleAttackActionRef.action.performed += TryToMeleeAttack;
    }

    private void OnDisable()
    {
        meleAttackActionRef.action.performed -= TryToMeleeAttack;
    }

    protected override void Initialization()
    {
        base.Initialization();
        meleeAttackParamiterID = Animator.StringToHash(meleeAttackAnimParamiterName);
    }

    public override void EnterAbility()
    {
        linkedPhysics.ResetVelocity();
    }

    public override void ExitAbility()
    {

        linkedPhysics.ResetVelocity();
    }

    private void TryToMeleeAttack(InputAction.CallbackContext value)
    {
        if (!isParamited || linkedStateMachine.curentState == (int)PlayerStates.State.KnockBack
            || linkedStateMachine.curentState == (int)PlayerStates.State.Death)
            return;

        //othe conditions
        if (linkedStateMachine.curentState == (int)PlayerStates.State.Dash || linkedPhysics.wallDetected
            || linkedStateMachine.curentState == (int)PlayerStates.State.Crouch 
            || linkedStateMachine.curentState == (int)PlayerStates.State.Reload
            || linkedStateMachine.curentState == (int)PlayerStates.State.Jump
            || linkedStateMachine.curentState == (int)PlayerStates.State.DoubleJump
            || linkedStateMachine.curentState == (int)PlayerStates.State.Ladder)
            return;

        linkedStateMachine.ChangeState((int)PlayerStates.State.MeleeAttack);
    }

    public override void ProcessAbility()
    {
     
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(meleeAttackParamiterID, linkedStateMachine.curentState == (int)PlayerStates.State.MeleeAttack);
    }

    public void MeleeAttackFinish()
    {
        if (linkedPhysics.grounded == true)
        {
            if (linkedInput.horizontalInput == 0)
                linkedStateMachine.ChangeState((int)PlayerStates.State.Idle);
            else
            if (linkedInput.horizontalInput != 0)
                linkedStateMachine.ChangeState((int)PlayerStates.State.Move);
        }
        else
            linkedStateMachine.ChangeState((int)PlayerStates.State.Jump);
    }
}

