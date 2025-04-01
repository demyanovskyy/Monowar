using UnityEngine;
using static UnityEngine.LightAnchor;
public class Aim : MonoBehaviour
{

    [SerializeField] private float _minRotateGundistanceX = 5f;
    [SerializeField] private float _minRotateGundistanceY = 8f;
    [SerializeField] private Transform _arm;
    [SerializeField] private Transform Parent;

    [SerializeField] private bool facingRight = true; 
    
    [SerializeField] private float speedRotateAim = 5f;
    [SerializeField] private float speedRotateGun = 2f;

    [SerializeField] private float armMinAngle = -40; 
    [SerializeField] private float armMaxAngle = 40;


    [SerializeField] private float gunMinAngle = -10;
    [SerializeField] private float gunMaxAngle = 10;

     
    [SerializeField] private bool frizeRotateArm = false;
    [SerializeField] private bool frizeRotateGun = false;

    [SerializeField] private GameObject _Head;

    [SerializeField] private WeaponManager _weaponManager;

    private float armAngle, gunAngle;
    private Vector3 mousePosMain;
    private float invert;

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
  
        if (!facingRight) invert = -1; else invert = 1;
    }

    public void SetfrizeRotateArm(bool _fR)
    {
        frizeRotateArm = _fR;
    }

    private void Update()
    {
        
        mousePosMain = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        invert = Mathf.Sign(Parent.localScale.x);

        if (frizeRotateArm)
        {
            armAngle = 0;
            Quaternion rotation = Quaternion.AngleAxis(armAngle * invert, Vector3.forward);
            _arm.rotation = Quaternion.Slerp(_arm.rotation, rotation, speedRotateAim * Time.deltaTime);
            _Head.SetActive(false);
        }
        else
        {
            MouseControl();
            _Head.SetActive(true);
        }
        FlipControl(mousePosMain);
    }

    public void GunRotateToMouse(Transform V1, Vector3 V2, float min, float max, float _speed, float _parentAngle, float minDistX, float minDistY, bool friz, float inv)
    {
        gunAngle = Utility.RotateV1ToV2AddParamiters(V1, V2, min, max, _speed, _parentAngle, minDistX, minDistY, friz, inv);
    }


    public void ArmRotateToMouse(Transform V1, Vector3 V2, float min, float max, float speed, float inv)
    {
       armAngle = Utility.RotateV1ToV2(V1, V2, min, max, speed, inv);
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
        ArmRotateToMouse(_arm,
                         mousePosMain,
                         armMinAngle,
                         armMaxAngle,
                         speedRotateAim,
                         invert);

        GunRotateToMouse(_weaponManager.ReturnCurrentWeapon().transform,
                         mousePosMain,
                         gunMinAngle,
                         gunMaxAngle,
                         speedRotateGun,
                         armAngle,
                         _minRotateGundistanceX,
                         _minRotateGundistanceY,
                         frizeRotateGun,
                         invert);


    }
    void Flip() 
    {
        facingRight = !facingRight;
        Vector3 theScale = Parent.localScale;
        theScale.x *= -1;
        invert *= -1;
        Parent.localScale = theScale;
    }



}
