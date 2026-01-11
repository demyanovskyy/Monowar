using System;
using System.Collections.Generic;
using System.Linq;
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

                AddAllAnimSolver();


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



    public void AddSolversAfteReload(TypeOfWeapon wSelect)
    {
        switch (wSelect)
        {
            case TypeOfWeapon.Heand:

                break;
            case TypeOfWeapon.Pistol:

                AddAnimLeftArmSolver();
                AddWeaponRightArmSolver();
                SetWeaponHeadSolwer();

                break;
            case TypeOfWeapon.ShotGun:

                AddWeaponLeftArmSolver();
                AddWeaponRightArmSolver();
                SetWeaponHeadSolwer();

                break;
            case TypeOfWeapon.Rifle:

                AddWeaponLeftArmSolver();
                AddWeaponRightArmSolver();
                SetWeaponHeadSolwer();

                break;
        }
    }



    public void RemoveAllWeaponSolver()
    {

        HRemoveSolver(weaponLeftHeandSolver);

        HRemoveSolver(weaponLeftArmSolver);

        HRemoveSolver(weaponRightHeandSolver);

        HRemoveSolver(weaponRightArmSolver);

        HRemoveSolver(weaponHeadSolver);
    }


    public void RemoveWeaponLeftArmSolver()
    {

        HRemoveSolver(weaponLeftHeandSolver);
        HRemoveSolver(weaponLeftArmSolver);
    }

    public void AddWeaponLeftArmSolver()
    {


        HAddSolver(weaponLeftArmSolver);
        HAddSolver(weaponLeftHeandSolver);
    }

    public void RemoveWeaponRightArmSolver()
    {


        HRemoveSolver(weaponRightHeandSolver);
        HRemoveSolver(weaponRightArmSolver);
    }

    public void AddWeaponRightArmSolver()
    {

        HAddSolver(weaponRightHeandSolver);
        HAddSolver(weaponRightArmSolver);
    }

    public void SetWeaponHeadSolwer()
    {
        HRemoveSolver(animHeadSolver);
        HRemoveSolver(weaponHeadSolver);


        HAddSolver(weaponHeadSolver);

        // _IKManager.solvers.Insert(0, weaponHeadSolver);

        MoveSolverSafely(weaponHeadSolver);


    }




    ///=================anim solver=======================
    ///
    public void RemoveAllAnimSolver()
    {

        HRemoveSolver(animLeftHeandSolver);
        HRemoveSolver(animLeftArmSolver);


        HRemoveSolver(animRightHeandSolver);
        HRemoveSolver(animRightArmSolver);

        HRemoveSolver(weaponHeadSolver);
    }


    public void AddAllAnimSolver()
    {


        HAddSolver(animLeftArmSolver);
        HAddSolver(animLeftHeandSolver);


        HAddSolver(animRightArmSolver);
        HAddSolver(animRightHeandSolver);

        SetAnimationHeadSolwer();
    }



    public void RemoveAnimLeftArmSolver()
    {

        HRemoveSolver(animLeftHeandSolver);
        HRemoveSolver(animLeftArmSolver);
    }

    public void AddAnimLeftArmSolver()
    {


        HAddSolver(animLeftArmSolver);
        HAddSolver(animLeftHeandSolver);

    }

    public void RemoveAnimRightArmSolver()
    {


        HRemoveSolver(animRightHeandSolver);
        HRemoveSolver(animRightArmSolver);
    }

    public void AddAnimRightArmSolver()
    {


        HAddSolver(animRightArmSolver);
        HAddSolver(animRightHeandSolver);

    }

    public void SetAnimationHeadSolwer()
    {
        HRemoveSolver(animHeadSolver);
        HRemoveSolver(weaponHeadSolver);

        HAddSolver(animHeadSolver);
        //_IKManager.solvers.Insert(0, animHeadSolver);
        MoveSolverSafely(animHeadSolver);

    }


    public void SetAnimator(TypeOfWeapon wSelect)
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


    public void HRemoveSolver(Solver2D s)
    {
        _IKManager.RemoveSolver(s);
        // Debug.Log($"Removed solver: {s.name}");
    }

    public void HAddSolver(Solver2D s)
    {
        _IKManager.AddSolver(s);
        // Debug.Log($"Add solver: {s.name}");
    }



    public void MoveSolverSafely(Solver2D targetSolver)
    {
        // 1. Создаем копию текущего списка солверов
        List<Solver2D> currentSolvers = _IKManager.solvers.ToList();

        if (currentSolvers.Contains(targetSolver))
        {
            // 2. Очищаем оригинальный список в IK Manager'е
            _IKManager.solvers.Clear();

            // 3. Удаляем целевой солвер из временной копии списка
            currentSolvers.Remove(targetSolver);

            // 4. Добавляем целевой солвер первым в оригинальный список
            _IKManager.solvers.Add(targetSolver);

            // 5. Добавляем остальные солверы из временной копии обратно в список менеджера
            foreach (Solver2D solver in currentSolvers)
            {
                _IKManager.solvers.Add(solver);
            }

            //Debug.Log($"Solver {targetSolver.name} safely moved to the top position.");
        }
        else
        {
            Debug.LogWarning($"Solver {targetSolver.name} not found in the IK Manager list.");
        }
    }
}
