using UnityEngine;
using UnityEngine.InputSystem;

public class LaddersAbility : BaseAbilityPlayer
{

    public InputActionReference ladderActionRef;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float setMinLadderTime;
    private float minimumLadderTime;
    private bool climb;
    public bool canGoOnLadder = false;

    private string ladderAnimParamiterName = "Ladder";
    private int ladderParamiterID;


    private void OnEnable()
    {
        ladderActionRef.action.performed += TryToClimb;
        ladderActionRef.action.canceled += StopClimb;
    }

    private void OnDisable()
    {
        ladderActionRef.action.performed -= TryToClimb;
        ladderActionRef.action.canceled -= StopClimb;
    }

    protected override void Initialization()
    {
        base.Initialization();
        ladderParamiterID = Animator.StringToHash(ladderAnimParamiterName);
        minimumLadderTime = setMinLadderTime;


    }

    public bool GetClimbParamiter()
    {
        return climb;
    }

    private void TryToClimb(InputAction.CallbackContext value)
    {
        if (!isParamited || linkedStateMachine.curentState == (int)PlayerStates.State.KnockBack
            || linkedStateMachine.curentState == (int)PlayerStates.State.Death)
            return;

        linkedAnimator.enabled = true;

        if (linkedStateMachine.curentState == (int)PlayerStates.State.Ladder 
            || linkedStateMachine.curentState == (int)PlayerStates.State.Dash
            || !canGoOnLadder || linkedStateMachine.curentState == (int)PlayerStates.State.Reload)
            return;

        linkedStateMachine.ChangeState((int)PlayerStates.State.Ladder);
        linkedPhysics.DisableGravity();
        linkedPhysics.ResetVelocity();

        climb = true;

        minimumLadderTime = setMinLadderTime;


    }
    private void StopClimb(InputAction.CallbackContext value)
    {
        if (!isParamited)
            return;

        if (linkedStateMachine.curentState != (int)PlayerStates.State.Ladder)
            return;

        linkedPhysics.ResetVelocity();

        linkedAnimator.enabled = false;

    }

    public override void EnterAbility()
    {
        player.GetComponent<WeaponManager>().DeActivateCurrentWeapon(); //player.DeactivateCurrentWeapon();
    }

    public override void ExitAbility()
    {
        linkedPhysics.EnableGravity();
        climb = false;
        linkedAnimator.enabled = true;

        player.GetComponent<WeaponManager>().AtivateCurrentlWeapon();// player.ActivateCurrentWeapon();
    }

    public override void ProcessAbility()
    {
        minimumLadderTime -= Time.deltaTime;

        if (linkedInput.horizontalInput != 0)
        {
            linkedStateMachine.ChangeState((int)PlayerStates.State.Jump);
            return;
        }

        if (canGoOnLadder == false)
        {
            if (linkedPhysics.grounded == false)
            {
                linkedStateMachine.ChangeState((int)PlayerStates.State.Jump);
            }
        }

        if (linkedPhysics.grounded && minimumLadderTime <= 0)
        {
            linkedStateMachine.ChangeState((int)PlayerStates.State.Idle);
        }
    }

    public override void ProcessFixedAbility()
    {
        if (climb)
            linkedPhysics.rb.linearVelocity = new Vector2(0, linkedInput.verticalInput * climbSpeed);
    }

    public override void UpdateAnimator()
    {
 
        linkedAnimator.SetBool(ladderParamiterID, linkedStateMachine.curentState == (int)PlayerStates.State.Ladder);
    }
}
