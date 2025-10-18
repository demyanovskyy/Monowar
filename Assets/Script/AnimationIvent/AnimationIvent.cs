using UnityEngine;

public class AnimationIvent : MonoBehaviour
{
    [SerializeField] DoubleJump dJump;
    [SerializeField] MeleeAttack mAttack;
    [SerializeField] PlayerPhysicsControl pPhysics;

    public void DoubleJumpFinish()
    {
        dJump.DoubleJumpFinish();
    }

    public void MelleAttackFinish()
    {
        mAttack.MeleeAttackFinish();
    }

    public void ActivateAttackColider()
    {
        pPhysics.ActivatedAttackCollider();
    }

    public void DeActivateAttackColider()
    {
        pPhysics.DeactivatedAttackCollider();
    }
}
