using System.Net;
using UnityEditor;
using UnityEngine;
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
    private float viewDirection;


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
        if (!facingRight) viewDirection = -1; else viewDirection = 1;
    }

    private void Start()
    {

        if (!facingRight) viewDirection = -1; else viewDirection = 1;
    }

    public void SetfrizeRotateArm(bool _fR)
    {
        frizeRotateArm = _fR;
    }

    private void Update()
    {

        mousePosMain = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        viewDirection = Mathf.Sign(Parent.localScale.x);

        if (frizeRotateArm)
        {
            armAngle = 0;
            Quaternion rotation = Quaternion.AngleAxis(armAngle * viewDirection, Vector3.forward);
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
                         viewDirection);

        GunRotateToMouse(_weaponManager.ReturnCurrentWeapon().transform,
                         mousePosMain,
                         gunMinAngle,
                         gunMaxAngle,
                         speedRotateGun,
                         armAngle,
                         _minRotateGundistanceX,
                         _minRotateGundistanceY,
                         frizeRotateGun,
                         viewDirection);


    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = Parent.localScale;
        theScale.x *= -1;
        viewDirection *= -1;
        Parent.localScale = theScale;
    }




/*
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 angeMin, angeMax;
        float radius = 6f;
        float deltaAngle = -90;

        if (viewDirection > 0)
        {
            angeMin = DerectionFromAngle(0, -deltaAngle - armMinAngle);
            angeMax = DerectionFromAngle(0, -deltaAngle - armMaxAngle);
        }
        else
        {
            angeMin = DerectionFromAngle(180, -deltaAngle + armMinAngle);
            angeMax = DerectionFromAngle(180, -deltaAngle + armMaxAngle);
        }


        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(_arm.position, _arm.position + angeMin * radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_arm.position, _arm.position + angeMax * radius);


    }


    private Vector2 DerectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

#endif
*/

}
