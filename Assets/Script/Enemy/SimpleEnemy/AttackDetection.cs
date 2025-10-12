using UnityEngine;

public class AttackDetection : MonoBehaviour
{
    [SerializeField] EnemyPhysicsControl enemyPhsicsControl;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemyPhsicsControl.inAttackRange = true;  
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemyPhsicsControl.inAttackRange = false;
    }
}
