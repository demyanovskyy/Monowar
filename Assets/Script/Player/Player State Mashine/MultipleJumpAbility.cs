using UnityEngine;
using UnityEngine.InputSystem;

public class MultipleJumpAbility : BaseAbilityPlayer
{
    public InputActionReference jumpActionRef;

    [SerializeField] private int maxNumberOfJumps;
    private int numberOfJumps;
    private bool canActivateAdditionalJumps;

    [SerializeField] private float jumpForce;
    [SerializeField] private float airSpeed;
    [SerializeField] private float minimumAirTime;
    private float startMinimumAirTime;

    [SerializeField] private float setMaxJumpTimer;
    private float jumpTimer;
    private bool jumping;
    [SerializeField] private float gravityDevider;
    [SerializeField] private float maxFollSpeed = -10;

    private string jumpAnimParamiterName = "Jump";
    private string ySpeedAnimParamiterName = "ySpeed";
    private int jumpParamiterID;
    private int ySpeedParamiterID;


    protected override void Initialization()
    {
        base.Initialization();

        startMinimumAirTime = minimumAirTime;
        numberOfJumps = maxNumberOfJumps;

        jumpParamiterID = Animator.StringToHash(jumpAnimParamiterName);
        ySpeedParamiterID = Animator.StringToHash(ySpeedAnimParamiterName);
    }


    private void OnEnable()
    {
        jumpActionRef.action.performed += TryToJump;
        jumpActionRef.action.canceled += StopJump;
    }

    private void OnDisable()
    {
        jumpActionRef.action.performed -= TryToJump;
        jumpActionRef.action.canceled -= StopJump;

    }

    public override void ProcessAbility()
    {
        entety.Flip();
        Debug.Log("Flip in Jump");
        minimumAirTime -= Time.deltaTime;

        if (jumping)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0)
            {
                jumping = false;
            }
        }

        if (linkedPhysics.grounded && minimumAirTime < 0)
        {
            if (linkedInput.horizontalInput != 0)
                linkedStateMachine.ChangeState((int)PlayerStates.State.Move);
            else
                linkedStateMachine.ChangeState((int)PlayerStates.State.Idle);
        }

        if (!linkedPhysics.grounded && linkedPhysics.wallDetected)
        {
            if (linkedPhysics.rb.linearVelocityY < 0)
            {
                linkedStateMachine.ChangeState((int)PlayerStates.State.WallSlide);
            }

        }
    }

    public override void ProcessFixedAbility()
    {
        if (!linkedPhysics.grounded)
        {
            if (jumping)
                linkedPhysics.rb.linearVelocity = new Vector2(airSpeed * linkedInput.horizontalInput, jumpForce);
            else
                linkedPhysics.rb.linearVelocity = new Vector2(airSpeed * linkedInput.horizontalInput, Mathf.Clamp(linkedPhysics.rb.linearVelocityY, -10, jumpForce));
        }

        if (linkedPhysics.rb.linearVelocityY < 0)
        {
            linkedPhysics.rb.gravityScale = linkedPhysics.GetGravity() / gravityDevider;
        }
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(jumpParamiterID, linkedStateMachine.curentState == (int)PlayerStates.State.Jump
            || linkedStateMachine.curentState == (int)PlayerStates.State.WallJump);
        linkedAnimator.SetFloat(ySpeedParamiterID, linkedPhysics.rb.linearVelocityY);
    }

    private void TryToJump(InputAction.CallbackContext value)
    {
        // Debug.Log("Jump button pressed:");

        if (linkedPhysics.isOnSlope && !linkedPhysics.canWalkOnSlope)
            return;


        if (!isParamited || linkedStateMachine.curentState == (int)PlayerStates.State.KnockBack
            || linkedStateMachine.curentState == (int)PlayerStates.State.Death
            || linkedStateMachine.curentState == (int)PlayerStates.State.Reload)
            return;

        if (linkedStateMachine.curentState == (int)PlayerStates.State.Ladder)
        {
            linkedStateMachine.ChangeState((int)PlayerStates.State.Jump);

            linkedPhysics.rb.linearVelocity = new Vector2(airSpeed * linkedInput.horizontalInput, 0);

            minimumAirTime = startMinimumAirTime;

            jumping = true;
            jumpTimer = setMaxJumpTimer;

            // == jump from ladder======
            numberOfJumps = maxNumberOfJumps;
            canActivateAdditionalJumps = true;
            numberOfJumps -= 1;
            audioSours.PlayOneShot(audioClip);

            return;
        }

        // === first jump========
        if (linkedPhysics.coyotTimer > 0)
        {
            linkedStateMachine.ChangeState((int)PlayerStates.State.Jump);

            linkedPhysics.rb.linearVelocity = new Vector2(airSpeed * linkedInput.horizontalInput, jumpForce);

            minimumAirTime = startMinimumAirTime;

            linkedPhysics.coyotTimer = -1;

            jumping = true;
            jumpTimer = setMaxJumpTimer;

            numberOfJumps = maxNumberOfJumps;
            canActivateAdditionalJumps = true;
            numberOfJumps -= 1;
            audioSours.PlayOneShot(audioClip);
            return;
        }

        if (numberOfJumps > 0 && canActivateAdditionalJumps)
        {
            //linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysics.EnableGravity();// reset gravity

            linkedPhysics.rb.linearVelocity = new Vector2(airSpeed * linkedInput.horizontalInput, jumpForce*1.5f);

            minimumAirTime = startMinimumAirTime;

            linkedPhysics.coyotTimer = -1;

            jumping = true;
            jumpTimer = setMaxJumpTimer;

            numberOfJumps -= 1;
            audioSours.PlayOneShot(audioClip);
            linkedStateMachine.ChangeState((int)PlayerStates.State.DoubleJump);
        }
        else
        {
            canActivateAdditionalJumps = false;
        }

    }

    private void StopJump(InputAction.CallbackContext value)
    {
        jumping = false;
        //Debug.Log("Jump button an pressed:");
    }

    public override void ExitAbility()
    {
        linkedPhysics.EnableGravity();
        canActivateAdditionalJumps = false;
    }

    public void SetMaxJumpNumber(int maxJumps)
    {
        maxNumberOfJumps = maxJumps;
    }
}
