using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] Player player;

    public InputActionReference changeWeponActionRef;

    [SerializeField] private GameObject lArm;
    [SerializeField] private GameObject rArm;
    public List<Weapon> weapons = new List<Weapon>();


    private Weapon currentWeapon;

    private TypeOfWeapon tempWeapon;

    private int weaponSelect = 0;

    public static Action<Sprite, int, int, int> OnUpdateAllInfo;

    private void OnEnable()
    {
        changeWeponActionRef.action.performed += TryToChangeWepon;
    }

    private void OnDisable()
    {

        changeWeponActionRef.action.performed -= TryToChangeWepon;

        foreach (Weapon weapon in weapons)
        {
            weapon.SaveWeaponData();
        }
    }


    private void Start()
    {
        ActivateWeapon(TypeOfWeapon.Heand);

        LoadWeapon();

        OnUpdateAllInfo?.Invoke(currentWeapon.weaponIconSprite, currentWeapon.curentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);

    }

    private void LoadWeapon()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.LoadWeaponData();
        }
    }


    private void TryToChangeWepon(InputAction.CallbackContext value)
    {

        if (player.stateMachine.curentState == (int)PlayerStates.State.Ladder
        || player.stateMachine.curentState == (int)PlayerStates.State.Dash
        || player.stateMachine.curentState == (int)PlayerStates.State.WallSlide
        || player.stateMachine.curentState == (int)PlayerStates.State.KnockBack
        || player.stateMachine.curentState == (int)PlayerStates.State.Death)
            return;

        if (currentWeapon.isReloading)
            return;

        SelectWeapon(value.action.ReadValue<float>());

        OnUpdateAllInfo?.Invoke(currentWeapon.weaponIconSprite, currentWeapon.curentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
    }

    public void AddStorageAmmo(TypeOfWeapon ID, int ammoToAdd)
    {

        foreach (Weapon weapon in weapons)
        {
            if (weapon.weaponType == ID)
            {
                Debug.Log("ADD AMMO" + ammoToAdd);
                weapon.storageAmmo += ammoToAdd;
                OnUpdateAllInfo?.Invoke(currentWeapon.weaponIconSprite, currentWeapon.curentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
                break;
            }
        }
    }


    public void SelectWeapon(float _weaponSelect)
    {

        weaponSelect += (int)_weaponSelect;

        if (weaponSelect >= weapons.Count)
        {
            weaponSelect = weapons.Count;
        }
        else
        if (weaponSelect < 0)
        {
            weaponSelect = 0;
        }


        if (weaponSelect != (int)currentWeapon.weaponType)
        {

            ActivateWeapon((TypeOfWeapon)weaponSelect);
        }
    }

    public Weapon GetWeaponOfType(TypeOfWeapon w)
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon.weaponType == w)
            {
                return weapon;
            }
        }
        return null;
    }

    public void ActivateWeapon(TypeOfWeapon w)
    {
        if (w == TypeOfWeapon.Heand)
        {
            foreach (Weapon weapon in weapons)
            {
                //deactivate all weapon
                weapon.weaponActiv = false;
                weapon.gameObject.SetActive(false);
                weapon.shootEnable = false;

                if (weapon.weaponType == w)
                {
                    // sellect weapon
                    currentWeapon = weapon;
                    tempWeapon = currentWeapon.weaponType;
                }
            }
            // set weapon heand point
            SetWeaponHeandPoint(currentWeapon.weaponType);
            //De activate rotateobject
            player.rotateObject.rotateObjectTransform.SetActive(false);
            // Activate rotate 
            player.rotateObject.ActivateFrizeRotate();
        }

        else
        {
            // for each weapon
            foreach (Weapon weapon in weapons)
            {
                //deactivate all weapon
                weapon.weaponActiv = false;
                weapon.gameObject.SetActive(false);
                weapon.shootEnable = false;

                if (weapon.weaponType == w)
                {
                    // sellect weapon
                    currentWeapon = weapon;
                    tempWeapon = currentWeapon.weaponType;
                }
            }
            // activate weapon
            currentWeapon.weaponActiv = true;
            currentWeapon.gameObject.SetActive(true);
            currentWeapon.shootEnable = true;
            // set weapon heand point
            SetWeaponHeandPoint(currentWeapon.weaponType);
            currentWeapon.GetComponent<OffHeandsWeapon>().OffHeandUptatePoint();
            // activate rotateobject
            player.rotateObject.rotateObjectTransform.SetActive(true);
            //De activate rotate 
            player.rotateObject.DeActivateFrizeRotate();
        }

        OnUpdateAllInfo?.Invoke(currentWeapon.weaponIconSprite, currentWeapon.curentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);

    }

    public void DeactivateAllWeapon()
    {
        ActivateWeapon(TypeOfWeapon.Heand);
    }

    public void DeActivateCurrentWeapon()
    {
        if (currentWeapon.weaponType == TypeOfWeapon.Heand)
            return;

        //save temp curent weapon
        tempWeapon = currentWeapon.weaponType;
        //deactivate current weapon
        currentWeapon.weaponActiv = false;
        currentWeapon.gameObject.SetActive(false);
        currentWeapon.shootEnable = false;

        //De activate rotateobject
        player.rotateObject.rotateObjectTransform.SetActive(false);
        // Activate rotate 
        player.rotateObject.ActivateFrizeRotate();
        // De activate Arm point
        lArm.SetActive(false);
        rArm.SetActive(false);
    }


    public void AtivateCurrentlWeapon()
    {
        ActivateWeapon(tempWeapon);
    }


    public Weapon ReturnCurrentWeapon()
    {
        return currentWeapon;
    }



    public void SetWeaponHeandPoint(TypeOfWeapon wSelect)
    {
        switch (wSelect)
        {
            case TypeOfWeapon.Heand:
                lArm.SetActive(false);
                rArm.SetActive(false);

                break;
            case TypeOfWeapon.Pistol:
                lArm.SetActive(false);
                rArm.SetActive(true);

                break;
            case TypeOfWeapon.ShotGun:
                lArm.SetActive(true);
                rArm.SetActive(true);

                break;
            case TypeOfWeapon.Rifle:
                lArm.SetActive(true);
                rArm.SetActive(true);

                break;
        }
    }


}
