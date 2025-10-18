using System.Collections;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Health")]
     protected float health;
    [SerializeField] protected float maxHealth;

    [Header("Flash")]
    [SerializeField] private float flashDuration;
    [SerializeField, Range(0, 1)] private float flashStrength;
    [SerializeField] private Color flashColor;
    [SerializeField] private Material flashMaterial;
    private Material defaultMatirial;
    [SerializeField] GameObject gafic;
    private SpriteRenderer[] spriteRenderer;
    private bool canTakeDamage = true;

    [Header("StatsColliders")]
    [SerializeField] private Collider2D statsCollider;


    protected Coroutine damageCorutine;
    private Material flashMatirialInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = gafic.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sRender in spriteRenderer)
        {
            defaultMatirial = sRender.material;
        }
       
        flashMatirialInstance = new Material(flashMaterial);
        health = maxHealth;
    }


    public void TakeDamage(float damage)
    {
        if (canTakeDamage == false)
            return;

        health -= damage;

        DamageProcess();
        if (damageCorutine != null)
            StopCoroutine(damageCorutine);

        damageCorutine = StartCoroutine(Flash());

        if (health <= 0)
        {
            DeathProcess();
        }
    }


    protected virtual void DeathProcess()
    {

       
    }

    protected virtual void DamageProcess()
    {
        StartCoroutine(Flash());
    }


    private IEnumerator Flash()
    {
        canTakeDamage = false;

        foreach (SpriteRenderer sRender in spriteRenderer)
        {
            sRender.material = flashMaterial;
        }

        // shader graf control====
        flashMatirialInstance.SetColor("_FlashColor", flashColor);
        flashMatirialInstance.SetFloat("_FlashAmount", flashStrength);
        //========================
        yield return new WaitForSeconds(flashDuration);
        foreach (SpriteRenderer sRender in spriteRenderer)
        {
            sRender.material = defaultMatirial;
        }

        damageCorutine = null;

        if (health > 0)
            canTakeDamage = true;


    }
    public void DisableStatsColider()
    {
        statsCollider.enabled = false;
    }

    public void EnableStatsColider()
    {
        statsCollider.enabled = true;
    }
    public float GetCurrentHealth()
    {
        return health;
    }

    public void DisableDamage()
    {
        canTakeDamage = false;
    }

    public void EnableDamage()
    {
        canTakeDamage = true;
    }

    public bool GetCanTakeDamage()
    {
        return canTakeDamage;
    }
}
