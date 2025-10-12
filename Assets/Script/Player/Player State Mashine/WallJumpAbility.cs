using UnityEngine;
using UnityEngine.InputSystem;

public class WallJumpAbility : BaseAbilityPlayer
{
    public InputActionReference wallJumpActionRef;

    [SerializeField] private Vector2 wallJumpForce;
    [SerializeField] private float wallJumpMaxTimer;

    private float wallJumpMinimumTimer;
    private float wallJumpTimer;


    private void OnEnable()
    {
        wallJumpActionRef.action.performed += TryToWallJump;
    }

    private void OnDisable()
    {
        wallJumpActionRef.action.performed -= TryToWallJump;
    }


    protected override void Initialization()
    {
        base.Initialization();
        wallJumpTimer = wallJumpMaxTimer;
    }


    private void TryToWallJump(InputAction.CallbackContext value)
    {
        if (!isParamited || linkedStateMachine.curentState == (int)PlayerStates.State.KnockBack
            || linkedStateMachine.curentState == (int)PlayerStates.State.Death)
            return;

        if (EvaluateWallJumpConditions())
        {
            linkedStateMachine.ChangeState((int)PlayerStates.State.WallJump);
            wallJumpTimer = wallJumpMaxTimer;
            wallJumpMinimumTimer = 0.15f;

            entety.ForceFlip();
            if (entety.facingRight)
                linkedPhysics.rb.linearVelocity = new Vector2(wallJumpForce.x, wallJumpForce.y);
            else
                linkedPhysics.rb.linearVelocity = new Vector2(-wallJumpForce.x, wallJumpForce.y);

        }
    }

    public override void ProcessAbility()
    {
        wallJumpTimer -= Time.deltaTime;
        wallJumpMinimumTimer -= Time.deltaTime;

        if (wallJumpMinimumTimer < 0 && linkedPhysics.grounded)
        {
            if (linkedInput.horizontalInput != 0)
                linkedStateMachine.ChangeState((int)PlayerStates.State.Move);
            else
                linkedStateMachine.ChangeState((int)PlayerStates.State.Idle);
            return;
        }

        if (wallJumpTimer <= 0)
        {
            if (linkedPhysics.grounded)
                linkedStateMachine.ChangeState((int)PlayerStates.State.Idle);
            else
                linkedStateMachine.ChangeState((int)PlayerStates.State.Jump);
            return;
        }

        if (wallJumpMinimumTimer <= 0 && linkedPhysics.wallDetected)
        {
            linkedStateMachine.ChangeState((int)PlayerStates.State.WallSlide);
            wallJumpTimer = -1;

        }


    }

    private bool EvaluateWallJumpConditions()
    {
        if (linkedPhysics.grounded || !linkedPhysics.wallDetected)
            return false;

        return true;
    }
}
