using System.Collections;
using UnityEngine;

using UnityEngine.InputSystem;

public class ReloadAbility : BaseAbilityPlayer
{
    public InputActionReference reloadActionRef;
    [SerializeField] private ReloadBar reloadBar;
    private Weapon currentWeapon;

    private Coroutine reloadCoroutin;


    private void OnEnable()
    {
        reloadActionRef.action.performed += TryToReload;
    }

    private void OnDisable()
    {
        reloadActionRef.action.performed -= TryToReload;
    }
    protected override void Initialization()
    {
        base.Initialization();
        currentWeapon = player.GetComponent<WeaponManager>().ReturnCurrentWeapon();
    }

    public override void EnterAbility()
    {
        
        linkedPhysics.ResetVelocity();
    }

    private void TryToReload(InputAction.CallbackContext value)
    {
        currentWeapon = player.GetComponent<WeaponManager>().ReturnCurrentWeapon();

        if (!isParamited || currentWeapon == null)
            return;

        if (linkedPhysics.grounded == false 
            || linkedStateMachine.curentState == (int)PlayerStates.State.Dash
            || linkedStateMachine.curentState == (int)PlayerStates.State.Ladder 
            || linkedStateMachine.curentState == (int)PlayerStates.State.KnockBack)
            return;

        if (currentWeapon.ReloadCheck() == false || currentWeapon.isReloading)
            return;

        reloadCoroutin = StartCoroutine(ReloadRrocess());

    }

    private IEnumerator ReloadRrocess()
    {
        linkedStateMachine.ChangeState((int)PlayerStates.State.Reload);
        currentWeapon.isReloading = true;
        reloadBar.ActivateReloadBar();

        float elapsedTime = 0;

        while (elapsedTime < currentWeapon.reloadTime)
        {
            elapsedTime += Time.deltaTime;
            reloadBar.UpdateReloadBar(elapsedTime, currentWeapon.reloadTime);
            yield return null;
        }

        reloadBar.DeactivateReloadBar();
        currentWeapon.Reload();
        Shooting.OnUpdateAmmo?.Invoke(currentWeapon.curentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);

        if (linkedStateMachine.curentState != (int)PlayerStates.State.Death && linkedStateMachine.curentState != (int)PlayerStates.State.KnockBack)
            linkedStateMachine.ChangeState((int)PlayerStates.State.Idle);
    }

    public override void ExitAbility()
    {
        reloadBar.DeactivateReloadBar();
        if (reloadCoroutin != null)
            StopCoroutine(reloadCoroutin);

        currentWeapon.isReloading = false;
    }


    public override void UpdateAnimator()
    {
        //if yor hev animation -> use update Animator
        // else
    }



}
