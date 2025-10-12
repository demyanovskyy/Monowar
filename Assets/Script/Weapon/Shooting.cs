using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{

    [Header("Input Action References")]
    public InputActionReference shootActionRef;

    [SerializeField] WeaponManager weaponManager;

    [SerializeField] private Player player;

    private Weapon currentWeapon;

    public bool shootButtonHeld = false;
    private bool shootCooldownOver = true;

    //for UI update

    public static Action<int, int, int> OnUpdateAmmo;

    private void OnEnable()
    {
        shootActionRef.action.performed += TryToShoot;
        shootActionRef.action.canceled += StopShooting;
    }

    private void OnDisable()
    {
        shootActionRef.action.performed -= TryToShoot;
        shootActionRef.action.canceled -= StopShooting;
    }

    private void Start()
    {
        player = GetComponent<Player>();

        currentWeapon = weaponManager.ReturnCurrentWeapon();

    }



    private void TryToShoot(InputAction.CallbackContext value)
    {

        currentWeapon = weaponManager.ReturnCurrentWeapon();

        if (currentWeapon.weaponType != TypeOfWeapon.Heand)
        {
            if (currentWeapon.shootEnable == false)
                return;

            if (shootButtonHeld || shootCooldownOver == false)
                return;

            if (currentWeapon.isAvtomatic)
            {
                shootButtonHeld = true;
                return;
            }

            shootButtonHeld = true;
            Shoot();
        }


    }

    private void StopShooting(InputAction.CallbackContext value)
    {
        shootButtonHeld = false;
    }


    public void Shoot()
    {

        //Check Ammo
        if (currentWeapon.curentAmmo <= 0 || currentWeapon.isReloading)
            return;

        // Play Sound SFX
        currentWeapon.audioSours.PlayOneShot(currentWeapon.audioClip);

        // Instantiate Shell
       //Instantiate(currentWeapon.shellPrefab, currentWeapon.shellSpawnPoint.position, currentWeapon.transform.rotation);
        IsPooleble s = ServiceLocator.Current.Get<LevelManager>().objectPoole.GetObject(currentWeapon.shellPrefab);
        s.GetComponent<Shell>().SetParamiter(currentWeapon.shellSpawnPoint.position, currentWeapon.transform.rotation);

        //Collback weapon
        currentWeapon.defaultWeaponVectorPos.localPosition = currentWeapon.tempPosColbackWeaponPos - Vector3.right * currentWeapon.recoilStrenght;

        // Instatiate Bullet
        //Bullet bullet = Instantiate(currentWeapon._bulletPrefab, currentWeapon._shootPoint.position, currentWeapon._shootPoint.rotation);
        //bullet.BulletMove();

        Bullet bullet = ServiceLocator.Current.Get<LevelManager>().objectPoole.GetObject(currentWeapon._bulletPrefab);
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

        //Update UI
        OnUpdateAmmo?.Invoke(currentWeapon.curentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
    }


    private IEnumerator ShootDelay()
    {
        shootCooldownOver = false;
        yield return new WaitForSeconds(currentWeapon.recoilTime);
        //Collback weapon
        currentWeapon.defaultWeaponVectorPos.localPosition = currentWeapon.tempPosColbackWeaponPos;

        yield return new WaitForSeconds(currentWeapon.shootCooldown - currentWeapon.recoilTime);
        shootCooldownOver = true;
    }

    private void Update()
    {
        if (currentWeapon != null)
            if (currentWeapon.weaponType != TypeOfWeapon.Heand)
            {
                if (shootButtonHeld && currentWeapon.isAvtomatic && shootCooldownOver)
                {
                    Shoot();
                }
            }

    }

}