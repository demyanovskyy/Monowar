using Unity.Android.Types;
using UnityEngine;

public class SwingBlade : MonoBehaviour
{
    [Header("Swing Setings")]
    [SerializeField] private float maxAngle;
    [SerializeField] private float speed;
    private float timer;
    private int pushDirection = 1;
    private float previousAngle = 0f;

    [Header("KnockBack Setings")]
    [SerializeField] private float swingDamage;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackForce;

    // Update is called once per frame
    void Update()
    {
        timer += speed * Time.deltaTime;
        float angle = maxAngle * Mathf.Sin(timer);
        transform.localRotation = Quaternion.Euler(0, 0, angle);

        if (angle > previousAngle)
            pushDirection = 1;
        else
        if (angle < previousAngle)
            pushDirection = -1;

        previousAngle = angle;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockBackAbility knockBackAbility = collision.GetComponentInParent<KnockBackAbility>();
        knockBackAbility.StartSwingKnockBack(knockBackDuration, knockBackForce, pushDirection);

        PlayerStats playerStats = collision.GetComponent<PlayerStats>();

        playerStats.DamagePlayer(swingDamage);
    }
}
