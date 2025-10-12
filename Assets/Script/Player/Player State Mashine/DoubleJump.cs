using UnityEngine;

public class DoubleJump : BaseAbilityPlayer
{
 
    private string doubleJumpAnimParamiterName = "DoubleJump";
    private int doubleJumpParamiterID;

    private bool doubleJumpAnimFinish = false;

    protected override void Initialization()
    {
        base.Initialization();
        doubleJumpParamiterID = Animator.StringToHash(doubleJumpAnimParamiterName);

    }

    public override void EnterAbility()
    {
        player.GetComponent<WeaponManager>().DeActivateCurrentWeapon();

        //player.DeactivateCurrentWeapon();
    }

    public override void ExitAbility()
    {
        player.GetComponent<WeaponManager>().AtivateCurrentlWeapon();
        doubleJumpAnimFinish = false;


        // player.ActivateCurrentWeapon();
    }
    public override void ProcessAbility()
    {
        if (!isParamited)
            return;

        if (linkedPhysics.grounded)
        {
            linkedStateMachine.ChangeState((int)PlayerStates.State.Idle);
            return;
        }

        if(doubleJumpAnimFinish)
            linkedStateMachine.ChangeState((int)PlayerStates.State.Jump);

    }
 
    public void DoubleJumpFinish()
    {
        doubleJumpAnimFinish = true;
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(doubleJumpParamiterID, linkedStateMachine.curentState == (int)PlayerStates.State.DoubleJump);
    }
}
