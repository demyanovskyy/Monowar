using UnityEngine;

public class OffHeandsWeapon : MonoBehaviour
{

    [Header("RightArm")]
    [Header("RightArmPoints")]
    public Transform _RPoint;
    public Transform _RCCDPoint;

    [Header("RightArmIK")]
    public Transform offHandIKR;
    public Transform offHandIKRCCD;

    [Header("LeftArm")]
    [Header("LeftArmPoints")]
    public Transform _LPoint;
    public Transform _LCCDPoint;

    [Header("LeftArmIK")]
    public Transform offHandIKL;
    public Transform offHandIKLCCD;

    [Header("Head")]
    public Transform offHeadPoint;
    public Transform offHeadIKRCCD;

    // Update is called once per frame


    public void OffHeandUptatePoint()
    {
        if (offHeadIKRCCD != null)
            offHeadIKRCCD.position = offHeadPoint.position;

        if (offHandIKR != null)
            offHandIKR.position = _RPoint.position;
        if (offHandIKL != null)
            offHandIKL.position = _LPoint.position;

        if (offHandIKRCCD != null)
            offHandIKRCCD.position = _RCCDPoint.position;
        if (offHandIKLCCD != null)
            offHandIKLCCD.position = _LCCDPoint.position;
    }

    private void Update()
    {

        OffHeandUptatePoint();

    }
}
