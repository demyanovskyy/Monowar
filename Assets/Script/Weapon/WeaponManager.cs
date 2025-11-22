using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.IK;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] Player player;

    public InputActionReference changeWeponActionRef;


    [SerializeField] private IKManager2D _IKManager;
    [SerializeField] private Animator _animator;

    [Header("Weapon solvers")]
    [Header("Left arm solvers")]
    [SerializeField] private Solver2D weaponLeftArmSolver;
    [SerializeField] private Solver2D weaponLeftHeandSolver;

    [Header("Right arm solvers")]
    [SerializeField] private Solver2D weaponRightArmSolver;
    [SerializeField] private Solver2D weaponRightHeandSolver;

    [Header("Animaten solvers")]
    [Header("Left arm solvers")]
    [SerializeField] private Solver2D animLeftArmSolver;
    [SerializeField] private Solver2D animLeftHeandSolver;

    [Header("Right arm solvers")]
    [SerializeField] private Solver2D animRightArmSolver;
    [SerializeField] private Solver2D animRightHeandSolver;

    [Header("Head solvers")]
    [SerializeField] private Solver2D weaponHeadSolver;
    [SerializeField] private Solver2D animHeadSolver;



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
        if (weaponSelect <= 0)
        {
            weaponSelect = 0;
        }


        if (weaponSelect != (int)currentWeapon.weaponType)
        {

            ActivateWeapon((TypeOfWeapon)weaponSelect);
            SetAnimator((TypeOfWeapon)weaponSelect);
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

        RemoveAllWeaponSolver();

        AddAllAnimSolver();
       

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
                RemoveAllWeaponSolver();
                RemoveAllAnimSolver();

                AddAnimLeftArmSolver();
                AddAnimRightArmSolver();
                SetAnimationHeadSolwer();


                break;
            case TypeOfWeapon.Pistol:
                RemoveWeaponLeftArmSolver();
                AddAnimLeftArmSolver();

                RemoveAnimRightArmSolver();
                AddWeaponRightArmSolver();

                SetWeaponHeadSolwer();



                break;
            case TypeOfWeapon.ShotGun:

                RemoveAnimLeftArmSolver();
                AddWeaponLeftArmSolver();

                RemoveAnimRightArmSolver();
                AddWeaponRightArmSolver();

                SetWeaponHeadSolwer();



                break;
            case TypeOfWeapon.Rifle:
                RemoveAnimLeftArmSolver();
                AddWeaponLeftArmSolver();

                RemoveAnimRightArmSolver();
                AddWeaponRightArmSolver();

                SetWeaponHeadSolwer();



                break;
        }
    }




    public void RemoveAllWeaponSolver()
    {
        _IKManager.RemoveSolver(weaponLeftArmSolver);
        _IKManager.RemoveSolver(weaponLeftHeandSolver);

        _IKManager.RemoveSolver(weaponRightArmSolver);
        _IKManager.RemoveSolver(weaponRightHeandSolver);

        _IKManager.RemoveSolver(weaponHeadSolver);
    }


    public void RemoveWeaponLeftArmSolver()
    {
        _IKManager.RemoveSolver(weaponLeftArmSolver);
        _IKManager.RemoveSolver(weaponLeftHeandSolver);
    }

    public void AddWeaponLeftArmSolver()
    {
        _IKManager.AddSolver(weaponLeftArmSolver);
        _IKManager.AddSolver(weaponLeftHeandSolver);
    }

    public void RemoveWeaponRightArmSolver()
    {

        _IKManager.RemoveSolver(weaponRightArmSolver);
        _IKManager.RemoveSolver(weaponRightHeandSolver);
    }

    public void AddWeaponRightArmSolver()
    {
        _IKManager.AddSolver(weaponRightArmSolver);
        _IKManager.AddSolver(weaponRightHeandSolver);
    }

    public void SetWeaponHeadSolwer()
    {
        _IKManager.RemoveSolver(animHeadSolver);
        _IKManager.RemoveSolver(weaponHeadSolver);

        _IKManager.solvers.Insert(0, weaponHeadSolver);
    }

 


    ///=================anim solver=======================
    ///
    public void RemoveAllAnimSolver()
    {
        _IKManager.RemoveSolver(animLeftArmSolver);
        _IKManager.RemoveSolver(animLeftHeandSolver);

        _IKManager.RemoveSolver(animRightArmSolver);
        _IKManager.RemoveSolver(animRightHeandSolver);

        _IKManager.RemoveSolver(weaponHeadSolver);
    }


    public void AddAllAnimSolver()
    {
        _IKManager.AddSolver(animLeftArmSolver);
        _IKManager.AddSolver(animLeftHeandSolver);

        _IKManager.AddSolver(animRightArmSolver);
        _IKManager.AddSolver(animRightHeandSolver);

        SetAnimationHeadSolwer();
    }



    public void RemoveAnimLeftArmSolver()
    {
        _IKManager.RemoveSolver(animLeftArmSolver);
        _IKManager.RemoveSolver(animLeftHeandSolver);
    }

    public void AddAnimLeftArmSolver()
    {
        _IKManager.AddSolver(animLeftArmSolver);
        _IKManager.AddSolver(animLeftHeandSolver);
    }

    public void RemoveAnimRightArmSolver()
    {

        _IKManager.RemoveSolver(animRightArmSolver);
        _IKManager.RemoveSolver(animRightHeandSolver);
    }

    public void AddAnimRightArmSolver()
    {
        _IKManager.AddSolver(animRightArmSolver);
        _IKManager.AddSolver(animRightHeandSolver);
    }

    public void SetAnimationHeadSolwer()
    {
        _IKManager.RemoveSolver(animHeadSolver);
        _IKManager.RemoveSolver(weaponHeadSolver);

        _IKManager.solvers.Insert(0, animHeadSolver);
    }


    private void SetAnimator(TypeOfWeapon wSelect)
    {


        switch (wSelect)
        {
            case TypeOfWeapon.Heand:

                
                _animator.SetLayerWeight(_animator.GetLayerIndex("Weapon"), 0);

                break;
            case TypeOfWeapon.Pistol:

               
                _animator.SetLayerWeight(_animator.GetLayerIndex("Weapon"), 1);
                _animator.SetFloat("WeaponType", (float)TypeOfWeapon.Pistol);
                break;
            case TypeOfWeapon.ShotGun:
                
                _animator.SetLayerWeight(_animator.GetLayerIndex("Weapon"), 1);
                _animator.SetFloat("WeaponType", (float)TypeOfWeapon.ShotGun);

                break;
            case TypeOfWeapon.Rifle:
               
                _animator.SetLayerWeight(_animator.GetLayerIndex("Weapon"), 1);
                _animator.SetFloat("WeaponType", (float)TypeOfWeapon.Rifle);
                break;
        }


        
     }
}
