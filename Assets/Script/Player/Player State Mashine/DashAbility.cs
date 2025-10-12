using UnityEngine;
using UnityEngine.InputSystem;

public class DashAbility : BaseAbilityPlayer
{

    public InputActionReference dashActionRef;
    [SerializeField] private float dashForce;
    [SerializeField] private float maxDashDuration;
    private float dashTimer;

    private string dashAnimParamiterName = "Dash";
    private int dashParamiterID;


    private void OnEnable()
    {
        dashActionRef.action.performed += TryToDash;
    }

    private void OnDisable()
    {
        dashActionRef.action.performed -= TryToDash;
    }

    protected override void Initialization()
    {
        base.Initialization();
        dashParamiterID = Animator.StringToHash(dashAnimParamiterName);
    }

    public override void EnterAbility()
    {
        // player.playerStats.DisableDamage();
        //or
        player.playerStats.DisableStatsColider();
    }

    public override void ExitAbility()
    {
        linkedPhysics.EnableGravity();
        //optional
        linkedPhysics.ResetVelocity();

        //player.playerStats.EnableDamage();
        //or
        player.playerStats.EnableStatsColider();
    }

    private void TryToDash(InputAction.CallbackContext value)
    {
        if (!isParamited || linkedStateMachine.curentState == (int)PlayerStates.State.KnockBack
            || linkedStateMachine.curentState == (int)PlayerStates.State.Death)
            return;

        //othe conditions
        if (linkedStateMachine.curentState == (int)PlayerStates.State.Dash || linkedPhysics.wallDetected
            || linkedStateMachine.curentState == (int)PlayerStates.State.Crouch || linkedStateMachine.curentState == (int)PlayerStates.State.Reload)
            return;

        linkedStateMachine.ChangeState((int)PlayerStates.State.Dash);
        linkedPhysics.DisableGravity();
        linkedPhysics.ResetVelocity();



        if (entety.facingRight)
            linkedPhysics.rb.linearVelocityX = dashForce;
        else
            linkedPhysics.rb.linearVelocityX = -dashForce;

        dashTimer = maxDashDuration;
    }

    public override void ProcessAbility()
    {
        dashTimer -= Time.deltaTime;

        if (linkedPhysics.wallDetected)
            dashTimer = -1;

        if (dashTimer <= 0)
        {
            if (linkedPhysics.grounded)
                linkedStateMachine.ChangeState((int)PlayerStates.State.Idle);
            else
                linkedStateMachine.ChangeState((int)PlayerStates.State.Jump);
        }
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(dashParamiterID, linkedStateMachine.curentState == (int)PlayerStates.State.Dash);
    }
}
