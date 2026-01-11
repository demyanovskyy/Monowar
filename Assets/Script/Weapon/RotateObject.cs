using UnityEngine;
public class RotateObject : MonoBehaviour
{
    [SerializeField] public Transform rotateObjectTransform;
    [SerializeField] private float offset;

    [SerializeField] private float speedRotateAim = 5f;
    [SerializeField] private float speedRotateGun = 2f;

    [SerializeField] private float armMinAngle = -40;
    [SerializeField] private float armMaxAngle = 40;


    [SerializeField] private float gunMinAngle = -10;
    [SerializeField] private float gunMaxAngle = 10;
    [SerializeField] private float offsetGun;


    [SerializeField] private bool frizeRotateArm = false;
    [SerializeField] private bool frizeRotateGun = false;

    [SerializeField] private WeaponManager _weaponManager;

    private float armAngle, gunAngle;
    private Vector3 mousePosMain;
    private float viewDirection;
    private bool facingRight = true;

    Quaternion rotZ;
    Quaternion rotZGun;

    public float ReturnGunAngle()
    {
        return armAngle;
    }

    public void Initialized(bool fDir)
    {
        facingRight = fDir;
        if (!facingRight) viewDirection = -1; else viewDirection = 1;
    }


    public void ActivateFrizeRotate()
    {
        frizeRotateArm = true;
    }

    public void DeActivateFrizeRotate()
    {
        frizeRotateArm = false;
    }


    public void UpdateRotateObjectParamites(Vector2 mPos, float vDir)
    {
        mousePosMain = mPos;
        viewDirection = vDir;
    }



    private void LateUpdate()
    {
        if (frizeRotateArm)
        {
            armAngle = 0;

            if (viewDirection == 1)
                rotZ = Quaternion.Euler(0f, 0f, armAngle + offset);
            else
                rotZ = Quaternion.Euler(0f, 180f, armAngle + offset);

            rotateObjectTransform.rotation = rotZ;

        }
        else
        {
            //=======================================================================================================================
            Vector3 direction = mousePosMain - rotateObjectTransform.position;

            float armDir = Mathf.Atan2(direction.y, direction.x * viewDirection) * Mathf.Rad2Deg;

            armAngle = Mathf.Clamp(armDir, armMinAngle, armMaxAngle);

            if (viewDirection == 1)
                rotZ = Quaternion.Euler(0f, 0f, armAngle + offset);
            else
                rotZ = Quaternion.Euler(0f, 180f, armAngle + offset);

            rotateObjectTransform.rotation = Quaternion.Slerp(rotateObjectTransform.rotation, rotZ, speedRotateAim * Time.deltaTime);
        }


        if (_weaponManager.ReturnCurrentWeapon() != null && _weaponManager.ReturnCurrentWeapon().isReloading == false)
        {
            if (frizeRotateGun)
            {
                _weaponManager.ReturnCurrentWeapon().transform.rotation = rotateObjectTransform.rotation;
            }
            else
            {
                Vector3 directionGun = mousePosMain - _weaponManager.ReturnCurrentWeapon().transform.position;

                float gunDir = Mathf.Atan2(directionGun.y, directionGun.x * viewDirection) * Mathf.Rad2Deg;

                gunAngle = Mathf.Clamp(gunDir, armAngle + gunMinAngle, armAngle + gunMaxAngle);

                if (viewDirection == 1)
                    rotZGun = Quaternion.Euler(0f, 0f, gunAngle + offsetGun);
                else
                    rotZGun = Quaternion.Euler(0f, 180f, gunAngle + offsetGun);

                _weaponManager.ReturnCurrentWeapon().transform.rotation = Quaternion.Slerp(_weaponManager.ReturnCurrentWeapon().transform.rotation, rotZGun, speedRotateGun * Time.deltaTime);
            }
        }
        //=======================================================================================================================

        //====Flip Arm===========================================================================================================

        Vector3 _locatScale = Vector3.one;

        if (armAngle > 90 || armAngle < -90)
        {
            _locatScale.y = -1f;
        }
        else
        {
            _locatScale.y = 1f;
        }

        rotateObjectTransform.localScale = _locatScale;

    }
}


