using UnityEngine;

public class AttackDetecktionBoss : MonoBehaviour
{
    [SerializeField] BossPhysicsControl boss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        boss.inAttackRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        boss.inAttackRange = false;
    }
}
