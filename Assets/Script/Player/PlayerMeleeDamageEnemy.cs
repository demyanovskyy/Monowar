using UnityEngine;

public class PlayerMeleeDamageEnemy : MonoBehaviour
{
    [Header("Damage Setings")]
    [SerializeField] private float damage;

    [Header("KnockBack Setings")]
    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackForce;
    private int pushDirection = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyKnockBackAbility knockBackAbility = collision.GetComponentInParent<EnemyKnockBackAbility>();

        if (transform.position.x > collision.transform.position.x)
            pushDirection = -1;
        else
        if (transform.position.x < collision.transform.position.x)
            pushDirection = 1;

        knockBackAbility.StartSwingKnockBack(knockBackDuration, knockBackForce, pushDirection);

        collision.GetComponent<EnemyStats>().TakeDamage(damage);
    }
}
