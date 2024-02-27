using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GangeWeapon : MonoBehaviour
{
    private Animator _anim;

    public Weapon Pistol, Rifle, Gun;

    public GameObject _primeParent;
    public GameObject _Larm;
    public GameObject _Rarm;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();

        _Larm.SetActive(false);
        _Rarm.SetActive(false);

        Pistol.SetActive(false);
        Rifle.SetActive(false);
        Gun.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            _Larm.SetActive(false);
            _Rarm.SetActive(false);

            Pistol.SetActive(false);
            Rifle.SetActive(false);
            Gun.SetActive(false);

            Pistol.weaponActiv = false;
            Gun.weaponActiv = false;
            Rifle.weaponActiv = false;

            _anim.SetTrigger("meleeAttack");

        }

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            _Larm.SetActive(false);
            _Rarm.SetActive(false);

            Pistol.SetActive(false);
            Rifle.SetActive(false);
            Gun.SetActive(false);

            Pistol.weaponActiv = false;
            Gun.weaponActiv = false;
            Rifle.weaponActiv = false;



        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            _Larm.SetActive(false);
            _Rarm.SetActive(true);

            Pistol.SetActive(true);
            Rifle.SetActive(false);
            Gun.SetActive(false);

            Pistol.weaponActiv = true;
            Gun.weaponActiv = false;
            Rifle.weaponActiv = false;

           

        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            _Larm.SetActive(true);
            _Rarm.SetActive(true);

            Pistol.SetActive(false);
            Rifle.SetActive(true);
            Gun.SetActive(false);

            Pistol.weaponActiv = false;
            Rifle.weaponActiv = true;
            Gun.weaponActiv = false;
           
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            _Larm.SetActive(true);
            _Rarm.SetActive(true);

            Pistol.SetActive(false);
            Rifle.SetActive(false);
            Gun.SetActive(true);

            Pistol.weaponActiv = false;
            Rifle.weaponActiv = false;
            Gun.weaponActiv = true;

            
        }


    }
    }
