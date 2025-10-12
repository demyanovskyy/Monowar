using UnityEngine;

public class FlyAttackDetectionPlayer : MonoBehaviour
{
    [SerializeField] FlyEnemyPhysicControl flyEnemyPhsicsControl;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        flyEnemyPhsicsControl.inAttackRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        flyEnemyPhsicsControl.inAttackRange = false;
    }
}
