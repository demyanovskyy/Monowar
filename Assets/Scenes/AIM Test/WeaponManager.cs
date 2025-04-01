using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class WeaponManager : MonoBehaviour
{

    [SerializeField] private Weapon[] _weapons;

    private GameObject currentWeapon;

    private Weapon _activWeapon;

    private void Start()
    {
        currentWeapon = _weapons[0].gameObject;
    }

    private void Update()
    {
        GetCurrentWeapon();
    }

    private void GetCurrentWeapon()
    {
        foreach (Weapon weapon in _weapons)
        {
  
            if (weapon.weaponActiv == true)
            {
                currentWeapon = weapon.gameObject;
 
                return;
            }
        }

    }

    public void ActivateWeapon(TypeOfWeapon w)
    {
        foreach (Weapon weapon in _weapons)
        {
            weapon.weaponActiv = false;
            weapon.SetActive(false);
            if(weapon.weaponType == w)
            {
                _activWeapon = weapon;
            }
        }

        _activWeapon.weaponActiv = true;
        _activWeapon.SetActive(true);
    }

    public void DeactivateAllWeapon()
    {
        foreach (Weapon weapon in _weapons)
        {
            weapon.weaponActiv = false;
            weapon.SetActive(false);
        }
    }


    public GameObject ReturnCurrentWeapon()
    {
        return currentWeapon;
    }
}
