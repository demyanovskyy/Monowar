using System.Collections;
using UnityEngine;


public class EnemyReloadAbility : BaseAbilityEnemy
{

    private string reloadAnimParamiterName = "Reload";
    private int reloadParamiterID;

    [SerializeField] private ReloadBar reloadBar;
    private EnemyWeapon currentWeapon;

    private Coroutine reloadCoroutin;


 
    protected override void Initialization()
    {
        base.Initialization();

        reloadParamiterID = Animator.StringToHash(reloadAnimParamiterName);

        currentWeapon = enemy.GetComponent<EnemyWeaponManager>().ReturnCurrentWeapon();
    }

    public override void EnterAbility()
    {

        linkedPhysics.ResetVelocity();

        enemy.GetComponent<EnemyWeaponManager>().DeActivateArm();


    }
    public override void ProcessAbility()
    {
        TryToReload();
    }

    private void TryToReload()
    {
        currentWeapon = enemy.GetComponent<EnemyWeaponManager>().ReturnCurrentWeapon();

        if (!isParamited || currentWeapon == null)
            return;

        if (linkedPhysics.grounded == false
            || linkedStateMachine.curentState == (int)EnemyStates.State.KnockBack)
            return;

        if (currentWeapon.ReloadCheck() == false || currentWeapon.isReloading)
            return;

        reloadCoroutin = StartCoroutine(ReloadRrocess());

    }

    private IEnumerator ReloadRrocess()
    {
        linkedStateMachine.ChangeState((int)EnemyStates.State.Reload);
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

        if (linkedStateMachine.curentState != (int)EnemyStates.State.Death && linkedStateMachine.curentState != (int)EnemyStates.State.KnockBack)
            linkedStateMachine.ChangeState((int)EnemyStates.State.Idle);
    }

    public override void ExitAbility()
    {
        reloadBar.DeactivateReloadBar();
        if (reloadCoroutin != null)
            StopCoroutine(reloadCoroutin);

        currentWeapon.isReloading = false;

        enemy.GetComponent<EnemyWeaponManager>().ActivateArm();
    }


    public override void UpdateAnimator()
    {
        //if yor hev animation -> use update Animator
        // else

        linkedAnimator.SetBool(reloadParamiterID, linkedStateMachine.curentState == (int)EnemyStates.State.Reload);
    }


}
