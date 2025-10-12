using UnityEngine;

public class MeleeAttackBoss : MonoBehaviour
{

    [Header("Damage Setings")]
    [SerializeField] private float damage;

    [Header("KnockBack Setings")]
    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackForce;
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockBackAbility knockBackAbility = collision.GetComponentInParent<KnockBackAbility>();

        knockBackAbility.StartKnockBack(knockBackDuration, knockBackForce, transform.parent);

        collision.GetComponent<PlayerStats>().DamagePlayer(damage);
    }

}
