using Unity.Mathematics;
using UnityEngine;

public class WallSlideAbility : BaseAbilityPlayer
{
    [SerializeField] private float maxSlideSpeed;


    private string wallSlideAnimParamiterName = "WallSlide";
    private int wallSlideParamiterID;
    protected override void Initialization()
    {
        base.Initialization();
        wallSlideParamiterID = Animator.StringToHash(wallSlideAnimParamiterName);
    }

    public override void EnterAbility()
    {
        linkedPhysics.rb.linearVelocity = Vector2.zero;
        player.GetComponent<WeaponManager>().DeActivateCurrentWeapon();
       

       //player.DeactivateCurrentWeapon();
    }

    public override void ExitAbility()
    {
        player.GetComponent<WeaponManager>().AtivateCurrentlWeapon();
       

        // player.ActivateCurrentWeapon();
    }
    public override void ProcessAbility()
    {
        if (!isParamited)
            return;

        if (linkedPhysics.grounded)
        {
            linkedStateMachine.ChangeState((int)PlayerStates.State.Idle);
            entety.ForceFlip();
            return;
        }

        if (entety.facingRight && linkedInput.horizontalInput < 0)
        {
            linkedStateMachine.ChangeState((int)PlayerStates.State.Jump);
            // sam fix
            linkedPhysics.wallDetected = false;
            linkedAnimator.SetBool("WallSlide", false);
            //
            return;
        }

        if (!entety.facingRight && linkedInput.horizontalInput > 0)
        {
            linkedStateMachine.ChangeState((int)PlayerStates.State.Jump);
            // sam fix
            linkedPhysics.wallDetected = false;
            linkedAnimator.SetBool("WallSlide", false);
            //
            return;
        }

        if (!linkedPhysics.wallDetected)
        {
            linkedStateMachine.ChangeState((int)PlayerStates.State.Jump);
            return;
        }

    }
    public override void ProcessFixedAbility()
    {
        linkedPhysics.rb.linearVelocityY = Mathf.Clamp(linkedPhysics.rb.linearVelocityY, -maxSlideSpeed, 1);
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(wallSlideParamiterID, linkedStateMachine.curentState == (int)PlayerStates.State.WallSlide);
    }
}
