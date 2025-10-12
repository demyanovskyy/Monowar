using UnityEngine;

public class RotatingBlade : MonoBehaviour
{
    [SerializeField] private float bladeDamage;
    [SerializeField] private float bladeRotationSpeed;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackForce;  

     // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, bladeRotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockBackAbility knockBackAbility = collision.GetComponentInParent<KnockBackAbility>();
        knockBackAbility.StartKnockBack(knockBackDuration, knockBackForce, transform);
 
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();

        playerStats.DamagePlayer(bladeDamage);
    }
}
