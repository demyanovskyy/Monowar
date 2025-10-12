using UnityEngine;

public class BossPhysicsControl : MonoBehaviour
{
    [Header("Colliders")]
    [SerializeField] private Collider2D statsCollider;
    [SerializeField] private Collider2D attackCollider;
    [SerializeField] private Collider2D attackDetectionCollider;
    public bool inAttackRange;

    public void EnableStateCollider()
    {
        statsCollider.enabled = true;
    }
    public void DesableStateCollider()
    {
        statsCollider.enabled = false;
    }
    public void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }
    public void DesableAttackCollider()
    {
        attackCollider.enabled = false;
    }
    public void EnableAttackDetectionCollider()
    {
        attackDetectionCollider.enabled = true;
    }
    public void DesableAttackDetectionCollider()
    {
        attackDetectionCollider.enabled = false;
    }

    public void DesableAllColliders()
    {
        DesableStateCollider();
        DesableAttackCollider();
        DesableAttackDetectionCollider();
    }

}
