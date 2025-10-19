using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponManager : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    public List<EnemyWeapon> weapons = new List<EnemyWeapon>();

    [SerializeField] private GameObject lArm;
    [SerializeField] private GameObject rArm;

    private EnemyWeapon currentWeapon;

    private TypeOfWeapon tempWeapon;

    private int weaponSelect = 0;

    private void Start()
    {
        weaponSelect = (int)weapons[0].weaponType;
        SelectWeapon(weaponSelect);
    }

    public EnemyWeapon ReturnCurrentWeapon()
    {
        return currentWeapon;
    }


    public void SelectWeapon(float _weaponSelect)
    {

        //weaponSelect += (int)_weaponSelect;

        //if (weaponSelect >= weapons.Count)
        //{
        //    weaponSelect = weapons.Count;
        //}
        //else
        //if (weaponSelect < 0)
        //{
        //    weaponSelect = 0;
        //}


        //if (weaponSelect != (int)currentWeapon.weaponType)
        //{

            ActivateWeapon((TypeOfWeapon)weaponSelect);
        //}
    }

    public void ActivateWeapon(TypeOfWeapon w)
    {
        if (w == TypeOfWeapon.Heand)
        {
            //foreach (Weapon weapon in weapons)
            //{
            //    deactivate all weapon
            //    weapon.weaponActiv = false;
            //    weapon.gameObject.SetActive(false);
            //    weapon.shootEnable = false;

            //    if (weapon.weaponType == w)
            //    {
            //         sellect weapon
            //        currentWeapon = weapon;
            //        tempWeapon = currentWeapon.weaponType;
            //    }
            //}
            // set weapon heand point
            //SetWeaponHeandPoint(currentWeapon.weaponType);
            //De activate rotateobject
            //player.rotateObject.rotateObjectTransform.SetActive(false);
            // Activate rotate 
            //player.rotateObject.ActivateFrizeRotate();
        }

        else
        {
            // for each weapon
            foreach (EnemyWeapon weapon in weapons)
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
            enemy.rotateObject.SetActive(true);
            //De activate rotate 
            enemy.DeactivateRotateobject();
        }

        
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
        enemy.rotateObject.SetActive(false);
        // Activate rotate 
        enemy.DeactivateRotateobject();
        // De activate Arm point
        lArm.SetActive(false);
        rArm.SetActive(false);
    }


    public void AtivateCurrentlWeapon()
    {
        ActivateWeapon(tempWeapon);
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
