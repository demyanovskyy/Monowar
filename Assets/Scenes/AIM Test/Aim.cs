using UnityEngine;
public class Aim : MonoBehaviour
{


    public Weapon[] _weapons;
    private GameObject currentWeapon;
    public float _minRotateGundistanceX = 5f;
    public float _minRotateGundistanceY = 8f;
    public Transform _arm;
    public Transform Parent;

    public bool facingRight = true; // направление на старте сцены, вправо?
    private float invert;


    public float speed = 5f;

    public float armMinAngle = -40; // ограничение по углам
    public float armMaxAngle = 40;


    public float gunMinAngle = -10;
    public float gunMaxAngle = 10;

    private float armAngle, gunAngle;

    [HideInInspector]
    public Vector3 mousePosMain;
    public bool frizeRotateArm = false;
    public bool frizeRotateGun = false;

    public GameObject _Head;
    private void GetCurrentWeapon()
    {
        foreach (Weapon weapon in _weapons)
        {
            frizeRotateArm = true;

            if (weapon.weaponActiv == true)
            {
                currentWeapon = weapon.gameObject;
                frizeRotateArm = false;
                return;
            }
        }

        
    }


    public float ReturnGunAngle()
    {
        return armAngle;
    }

    private void OnEnable()
    {
        Initialized();
    }


    public void Initialized()
    {
        mousePosMain = new Vector3(0, 0, -Camera.main.transform.position.z);
        facingRight = true;
        if (!facingRight) invert = -1; else invert = 1;
    }

    private void Start()
    {
        currentWeapon = _weapons[0].gameObject;
        
        if (!facingRight) invert = -1; else invert = 1;
    }

    private void Update()
    {
        GetCurrentWeapon();

        mousePosMain = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        invert = Mathf.Sign(Parent.localScale.x);

        if (frizeRotateArm)
        {
            armAngle = 0;
            Quaternion rotation = Quaternion.AngleAxis(armAngle * invert, Vector3.forward);
            //currentWeapon.transform.rotation = Quaternion.Slerp(currentWeapon.transform.rotation, rotation, speed * Time.deltaTime);
            _arm.rotation = Quaternion.Slerp(_arm.rotation, rotation, speed * Time.deltaTime);
            _Head.SetActive(false);
        }
        else
        {
            MouseControl();
            _Head.SetActive(true);
        }
        FlipControl(mousePosMain);
    }

    public void GunRotateToMouse(Vector3 mPos, float inv)
    {
        if (frizeRotateGun)
        {
            Quaternion rotation = Quaternion.AngleAxis(armAngle * invert, Vector3.forward);
            currentWeapon.transform.rotation = Quaternion.Slerp(currentWeapon.transform.rotation, rotation, speed * Time.deltaTime);

        }
        else
        {
  
            Vector2 gunDirection = mPos - currentWeapon.transform.position;
            gunAngle = Mathf.Atan2(gunDirection.y, gunDirection.x * inv) * Mathf.Rad2Deg;
            Quaternion gunRotation = Quaternion.AngleAxis(gunAngle * inv, Vector3.forward);
            gunAngle = Mathf.Clamp(gunAngle, armAngle + gunMinAngle, armAngle + gunMaxAngle);
            if (Mathf.Abs(gunDirection.x) > _minRotateGundistanceX && Mathf.Abs(gunDirection.y) < _minRotateGundistanceY)
                currentWeapon.transform.rotation = Quaternion.Slerp(currentWeapon.transform.rotation, gunRotation, speed * Time.deltaTime);
        }
    }


    public void ArmRotateToMouse(Vector3 mPos, float inv)
    {
        Vector2 direction = mPos - _arm.position;
        armAngle = Mathf.Atan2(direction.y, direction.x * inv) * Mathf.Rad2Deg;
        armAngle = Mathf.Clamp(armAngle, armMinAngle, armMaxAngle);
        Quaternion rotation = Quaternion.AngleAxis(armAngle * inv, Vector3.forward);
        _arm.rotation = Quaternion.Slerp(_arm.rotation, rotation, speed * Time.deltaTime);
 
    }

    public void FlipControl(Vector3 mPos)
    {
        if (mPos.x < Parent.position.x && facingRight)
        {
            Flip();
        }
        else if (mPos.x > Parent.position.x && !facingRight)
        {
            Flip();
        }
    }
    public void MouseControl()
    {
        ArmRotateToMouse(mousePosMain, invert);
        GunRotateToMouse(mousePosMain, invert);
        
    }
    void Flip() // отражение по горизонтали
    {
        facingRight = !facingRight;
        Vector3 theScale = Parent.localScale;
        theScale.x *= -1;
        invert *= -1;
        Parent.localScale = theScale;
    }



}
