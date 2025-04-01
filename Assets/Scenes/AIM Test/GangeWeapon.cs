using UnityEngine;

public class GangeWeapon : MonoBehaviour
{
    private Animator _anim;

    [SerializeField] private WeaponManager _weaponManager;

    [SerializeField] private GameObject _Larm;
    [SerializeField] private GameObject _Rarm;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();

        _Larm.SetActive(false);
        _Rarm.SetActive(false);

        _weaponManager.DeactivateAllWeapon();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            _Larm.SetActive(false);
            _Rarm.SetActive(false);

            _weaponManager.DeactivateAllWeapon();

            _anim.SetTrigger("meleeAttack");
            _anim.GetComponentInParent<Aim>().SetfrizeRotateArm(true);

        }


        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            _Larm.SetActive(false);
            _Rarm.SetActive(false);

            _weaponManager.DeactivateAllWeapon();
            _anim.GetComponentInParent<Aim>().SetfrizeRotateArm(true);

        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            _Larm.SetActive(false);
            _Rarm.SetActive(true);

            _weaponManager.ActivateWeapon(TypeOfWeapon.Pistol);
            _anim.GetComponentInParent<Aim>().SetfrizeRotateArm(false);

        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            _Larm.SetActive(true);
            _Rarm.SetActive(true);

            _weaponManager.ActivateWeapon(TypeOfWeapon.Rifle);
            _anim.GetComponentInParent<Aim>().SetfrizeRotateArm(false);

        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            _Larm.SetActive(true);
            _Rarm.SetActive(true);

            _weaponManager.ActivateWeapon(TypeOfWeapon.ShotGun);
            _anim.GetComponentInParent<Aim>().SetfrizeRotateArm(false);

        }
    }
}
