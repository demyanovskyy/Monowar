using UnityEngine;

public class MoveAbility : BaseAbilityPlayer
{
    [SerializeField] private float speed;

    private string moveAnimParamiterName = "Move";
    private int moveParamiterID;

    private string moveSpeedAnimParamiterName = "xSpeed";
    private int moveSpeedParamiterID;

    protected override void Initialization()
    {
        base.Initialization();
        moveParamiterID = Animator.StringToHash(moveAnimParamiterName);
        moveSpeedParamiterID = Animator.StringToHash(moveSpeedAnimParamiterName);
    }
    public override void EnterAbility()
    {
        entety.Flip();
    }

    public override void ProcessAbility()
    {
        if (!isParamited)
            return;

        if (linkedPhysics.wallDetected == true)
        {
            linkedStateMachine.ChangeState((int)PlayerStates.State.Idle);
            return;
        }

        if (linkedPhysics.grounded && linkedInput.horizontalInput == 0)
        {
            linkedStateMachine.ChangeState((int)PlayerStates.State.Idle);
        }

        if (!linkedPhysics.grounded)
        {
            linkedStateMachine.ChangeState((int)PlayerStates.State.Jump);
        }
    }

    public override void ProcessFixedAbility()
    {
        entety.Flip();

        if (linkedPhysics.slopDetected)
        {
            if (!linkedPhysics.canWalkOnSlope)
            {
                linkedPhysics.rb.linearVelocity = new Vector2(0f, linkedPhysics.rb.linearVelocityY);
                return;
            }
            else
            {
                linkedPhysics.rb.linearVelocity = new Vector2(speed * linkedPhysics.slopeNormalPerp.x * -linkedInput.horizontalInput, speed * linkedPhysics.slopeNormalPerp.y * -linkedInput.horizontalInput);
            }
        }
        else
            linkedPhysics.rb.linearVelocity = new Vector2(speed * linkedInput.horizontalInput, linkedPhysics.rb.linearVelocityY);

    }

    public override void UpdateAnimator()
    {
        if ((player.facingRight))
        {
            linkedAnimator.SetFloat(moveSpeedParamiterID, linkedInput.horizontalInput);
        }
        else
        {
            linkedAnimator.SetFloat(moveSpeedParamiterID, linkedInput.horizontalInput *(-1));
        }


        linkedAnimator.SetBool(moveParamiterID, linkedStateMachine.curentState == (int)PlayerStates.State.Move);
    }



}
