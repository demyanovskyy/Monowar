using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;


public enum TypeOfWeapon { Pistol, ShotGun, Rifle };

class Utility
{
    
    static public float RotateV1ToV2(Transform V1, Vector3 V2, float min, float max, float _speed, float inv)
    {
        Vector2 direction = V2 - V1.position;

        float _angle = Mathf.Atan2(direction.y, direction.x * inv) * Mathf.Rad2Deg;
        _angle = Mathf.Clamp(_angle, min, max);

        Quaternion rotation = Quaternion.AngleAxis(_angle * inv, Vector3.forward);

        V1.rotation = Quaternion.Slerp(V1.rotation, rotation, _speed * Time.deltaTime);

        return _angle;

    }

    static public float RotateV1ToV2AddParamiters(Transform V1, Vector3 V2, float min, float max, float _speed, float _parentAngle, float minDistX, float minDistY, bool friz, float inv)
    {
        if (friz)
        {
            Quaternion rotation = Quaternion.AngleAxis(_parentAngle * inv, Vector3.forward);
            V1.rotation = Quaternion.Slerp(V1.rotation, rotation, _speed * Time.deltaTime);

            float _angle = _parentAngle;

            return _angle;

        }
        else
        {
            Vector2 direction = V2 - V1.position;

            float _angle = Mathf.Atan2(direction.y, direction.x * inv) * Mathf.Rad2Deg;

            _angle = Mathf.Clamp(_angle, _parentAngle + min, _parentAngle + max);

            Quaternion rotation = Quaternion.AngleAxis(_angle * inv, Vector3.forward);

            V1.rotation = Quaternion.Slerp(V1.rotation, rotation, _speed * Time.deltaTime);

            if (Mathf.Abs(direction.x) > minDistX && Mathf.Abs(direction.y) < minDistY)
            {

                V1.rotation = Quaternion.Slerp(V1.rotation, rotation, _speed * Time.deltaTime);

            }

            return _angle;
        }

    }
}

