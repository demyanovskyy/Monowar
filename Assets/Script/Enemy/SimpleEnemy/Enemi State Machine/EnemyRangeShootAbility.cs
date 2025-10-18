using System.Collections;
using UnityEngine;

public class EnemyRangeShootAbility : BaseAbilityEnemy
{
    private string shootAnimParamiterName = "Idle";
    private int shootParamiterID;

    [SerializeField] private float shootTime;

    [SerializeField] private EnemyWeapon currentWeapon;


    private bool shootCooldownOver = true;

    private float shootCooldown;

    //public void EndOfAttack()
    //{
    //    if (enemy.fieldOfViev.targetInSight)
    //    {
    //        linkedStateMachine.ChangeState((int)PlayerStates.State.Shoot);
    //    }
    //    else
    //    {
    //        linkedStateMachine.ChangeState((int)PlayerStates.State.Idle);
    //    }

    //    StartCoroutine(CheckBehindDelay());

    //}

    public override void ExitAbility()
    {

        linkedPhysics.canCheckBehind = true;


    }

    public void Shoot()
    {

        //Check Ammo
        if (currentWeapon.curentAmmo <= 0 || currentWeapon.isReloading)
            return;

        // Play Sound SFX
        currentWeapon.audioSours.PlayOneShot(currentWeapon.audioClip);

        // Instantiate Shell
        IsPooleble s = ServiceLocator.Current.Get<LevelManager>().objectPoole.GetObject(currentWeapon.shellPrefab);
        s.GetComponent<Shell>().SetParamiter(currentWeapon.shellSpawnPoint.position, currentWeapon.transform.rotation);

        //Collback weapon
        currentWeapon.defaultWeaponVectorPos.localPosition = currentWeapon.tempPosColbackWeaponPos - Vector3.right * currentWeapon.recoilStrenght;


        // Instatiate Bullet
        EnemyBullet bullet = ServiceLocator.Current.Get<LevelManager>().objectPoole.GetObject(currentWeapon._bulletPrefab);
        bullet.SetParamiter(currentWeapon._shootPoint.position, currentWeapon._shootPoint.rotation);
        bullet.BulletMove();


        // particle flash
        if (currentWeapon.flashingParticle)
        {
            for (int i = 0; i < currentWeapon.particles.Count; i++)
            {
                currentWeapon.particles[i].Play();
            }
        }
        // sprite flash
        if (currentWeapon.flashingSprite)
        {
            StartCoroutine(currentWeapon.FlashTime());
        }

        currentWeapon.curentAmmo -= 1;

        StartCoroutine(ShootDelay());


    }
    private IEnumerator ShootDelay()
    {
        shootCooldownOver = false;
        yield return new WaitForSeconds(currentWeapon.recoilTime);
        //Collback weapon
        //currentWeapon.defaultWeaponVectorPos.localPosition = currentWeapon.tempPosColbackWeaponPos;

        yield return new WaitForSeconds(currentWeapon.shootCooldown - currentWeapon.recoilTime);

        shootCooldownOver = true;
    }


    IEnumerator CheckBehindDelay()
    {
        yield return new WaitForSeconds(linkedPhysics.behindDelay);
        linkedPhysics.canCheckBehind = true;
    }

    protected override void Initialization()
    {
        base.Initialization();
        shootParamiterID = Animator.StringToHash(shootAnimParamiterName);

    }


    public override void EnterAbility()
    {
        linkedPhysics.ResetVelocity();
        linkedPhysics.canCheckBehind = false;

        shootCooldown = shootTime;
    }

    public override void ProcessFixedAbility()
    {
    }


    public override void ProcessAbility()
    {
        if (!isParamited)
            return;

        if (enemy.fieldOfViev.targetInSight)
        {
            shootCooldown -= Time.deltaTime;

        }

        if (currentWeapon != null)
            if (currentWeapon.weaponType != TypeOfWeapon.Heand)
            {
                if (currentWeapon.isAvtomatic && shootCooldownOver)
                {
                    Shoot();
                }
            }

        if (enemy.fieldOfViev.targetInSight == false)
        {
            linkedStateMachine.ChangeState((int)EnemyStates.State.Idle);
        }

        if (shootCooldown <= 0)
        {
            if (enemy.fieldOfViev.targetInSight)
                linkedStateMachine.ChangeState((int)EnemyStates.State.Shoot);
            else
                linkedStateMachine.ChangeState((int)EnemyStates.State.Move);
        }

        if (currentWeapon != null)
            currentWeapon.defaultWeaponVectorPos.localPosition = Vector2.Lerp(currentWeapon.defaultWeaponVectorPos.localPosition, currentWeapon.tempPosColbackWeaponPos, currentWeapon.armRecoil * Time.deltaTime);
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(shootParamiterID, linkedStateMachine.curentState == (int)EnemyStates.State.Shoot);
    }
}












