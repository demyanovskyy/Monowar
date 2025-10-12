using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    [SerializeField] EnemyPhysicsControl ePhysics;
    [SerializeField] EnemyMeleeAbility mAttack;

    public void ActivateAttackColider()
    {
        ePhysics.ActivatedAttackCollider();
    }

    public void DeActivateAttackColider()
    {
        ePhysics.DeactivatedAttackCollider();
    }

    public void EndOffMelleAttack()
    {
        mAttack.EndOfAttack();
    }
}
