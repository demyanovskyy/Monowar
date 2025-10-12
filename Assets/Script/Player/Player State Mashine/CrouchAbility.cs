using UnityEngine;
using UnityEngine.InputSystem;

public class CrouchAbility : BaseAbilityPlayer
{

    public InputActionReference crouchActionRef;
    [SerializeField] private float crouchSpeed;

    private bool wantToStop;

    private string crouchAnimParamiterName = "Crouch";
    private int crouchParamiterID;
    private string xSpeedAnimParamiterName = "xSpeed";
    private int xSpeedParamiterID;


    private void OnEnable()
    {
        crouchActionRef.action.performed += TryToCrouch;
        crouchActionRef.action.canceled += StopCrouch;
    }

    private void OnDisable()
    {
        crouchActionRef.action.performed -= TryToCrouch;
        crouchActionRef.action.canceled -= StopCrouch;
    }

    protected override void Initialization()
    {
        base.Initialization();
        crouchParamiterID = Animator.StringToHash(crouchAnimParamiterName);
        xSpeedParamiterID = Animator.StringToHash(xSpeedAnimParamiterName);
    }

    public override void EnterAbility()
    {
        linkedPhysics.CrouchColliderEneble();
        player.playerStats.EnebleCrouchingStatsCollider();
        //player.SetCrouchWeaponPos();
    }

    public override void ExitAbility()
    {
        wantToStop = false;
        linkedPhysics.StandColliderEneble();
        player.playerStats.EnebleStandingStatsCollider();
        //player.SetStandWeaponPos();
    }

    private void TryToCrouch(InputAction.CallbackContext value)
    {
        if (!isParamited || linkedStateMachine.curentState == (int)PlayerStates.State.KnockBack
            || linkedStateMachine.curentState == (int)PlayerStates.State.Death)
            return;

        if (linkedPhysics.grounded == false 
            || linkedStateMachine.curentState == (int)PlayerStates.State.Dash
            || linkedStateMachine.curentState == (int)PlayerStates.State.Ladder 
            || linkedStateMachine.curentState == (int)PlayerStates.State.Reload)
            return;

        wantToStop = false;

        linkedStateMachine.ChangeState((int)PlayerStates.State.Crouch);
    }

    private void StopCrouch(InputAction.CallbackContext value)
    {
        if (!isParamited || linkedStateMachine.curentState == (int)PlayerStates.State.KnockBack)
            return;

        if (linkedStateMachine.curentState != (int)PlayerStates.State.Crouch) 
            return;

        if (linkedPhysics.ceilingDetected)
        {
            wantToStop = true;
            return;
        }


        if (linkedInput.horizontalInput == 0)
            linkedStateMachine.ChangeState((int)PlayerStates.State.Idle);
        else
            if (linkedInput.horizontalInput != 0)
            linkedStateMachine.ChangeState((int)PlayerStates.State.Move);

    }

    public override void ProcessAbility()
    {
        entety.Flip();

        if (wantToStop && linkedPhysics.ceilingDetected == false)
        {
            if (linkedInput.horizontalInput == 0)
                linkedStateMachine.ChangeState((int)PlayerStates.State.Idle);
            else
            if (linkedInput.horizontalInput != 0)
                linkedStateMachine.ChangeState((int)PlayerStates.State.Move);
        }

        if (linkedPhysics.grounded == false)
            linkedStateMachine.ChangeState((int)PlayerStates.State.Jump);

    }

    public override void ProcessFixedAbility()
    {
        if (linkedPhysics.grounded)
            linkedPhysics.rb.linearVelocity = new Vector2(linkedInput.horizontalInput * crouchSpeed, linkedPhysics.rb.linearVelocityY);
    }

    public override void UpdateAnimator()
    {
         // linkedAnimator.SetFloat(xSpeedParamiterID, Mathf.Abs(linkedPhysics.rb.linearVelocityX));
        if ((player.facingRight))
        {
            linkedAnimator.SetFloat(xSpeedParamiterID, linkedInput.horizontalInput);
        }
        else
        {
            linkedAnimator.SetFloat(xSpeedParamiterID, linkedInput.horizontalInput * (-1));
        }

        linkedAnimator.SetBool(crouchParamiterID, linkedStateMachine.curentState == (int)PlayerStates.State.Crouch);
    }
}
