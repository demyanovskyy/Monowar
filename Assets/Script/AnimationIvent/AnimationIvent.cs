using UnityEngine;

public class AnimationIvent : MonoBehaviour
{
    [SerializeField] DoubleJump dJump;
    [SerializeField] MeleeAttack mAttack;

    public void DoubleJumpFinish()
    {
        dJump.DoubleJumpFinish();
    }

    public void MelleAttackFinish()
    {
        mAttack.MeleeAttackFinish();
    }
}
