using System.Collections;
using System.IO;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] Player player;
    [SerializeField] HealthBarControl healthBarControl;
    [SerializeField] private float maxHealth;
    private float currentHealth;

    [Header("Flash")]
    [SerializeField] private float flashDuration;
    [SerializeField, Range(0, 1)] private float flashStrength;
    [SerializeField] private Color flashColor;
    [SerializeField] private Material flashMaterial;
    private Material defaultMatirial;
    private SpriteRenderer[] spriteRenderer;
    [SerializeField] GameObject gafic;
    private bool canTakeDamage = true;

    [Header("StatsColliders")]
    [SerializeField] private Collider2D standingStatsCollider;
    [SerializeField] private Collider2D crouchingStatsCollider;
    private Collider2D curentStateCillider;

    [SerializeField]
    private PlayerParamiters playerData = new PlayerParamiters();
    private string loadPath;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        loadPath = Path.Combine(Application.persistentDataPath,
            ServiceLocator.Current.Get<SaveLoadManager>().folderName,
            ServiceLocator.Current.Get<SaveLoadManager>().fileNamePlayerData);

        if (File.Exists(loadPath))
        {
            ServiceLocator.Current.Get<SaveLoadManager>().LoadData(playerData,
                ServiceLocator.Current.Get<SaveLoadManager>().folderName,
                ServiceLocator.Current.Get<SaveLoadManager>().fileNamePlayerData);


            currentHealth = playerData.currentHealth;
        }
        else
        {

            SavePlayerHealth();

            currentHealth = maxHealth;
        }


        healthBarControl.SetSliderValue(currentHealth, maxHealth);

        spriteRenderer = gafic.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sRender in spriteRenderer)
        {
            defaultMatirial = sRender.material;
        }

        curentStateCillider = standingStatsCollider;
    }

    public void EnebleStandingStatsCollider()
    {
        if (currentHealth <= 0)
            return;

        crouchingStatsCollider.enabled = false;
        standingStatsCollider.enabled = true;
        curentStateCillider = standingStatsCollider;

    }

    public void EnebleCrouchingStatsCollider()
    {
        if (currentHealth <= 0)
            return;

        standingStatsCollider.enabled = false;
        crouchingStatsCollider.enabled = true;
        curentStateCillider = crouchingStatsCollider;

    }

    public void DamagePlayer(float damage)
    {
        if (canTakeDamage == false)
            return;

        currentHealth -= damage;

        healthBarControl.SetSliderValue(currentHealth, maxHealth);
        StartCoroutine(Flash());
        if (currentHealth <= 0)
        {
            DisableStatsColider();
            if (player.stateMachine.curentState != (int)PlayerStates.State.KnockBack)
            {
                currentHealth = maxHealth;
                SavePlayerHealth();
                player.stateMachine.ChangeState((int)PlayerStates.State.Death);
            }

        }
    }

    public void AddHealth(float health)
    {
        currentHealth += health;
        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;

        healthBarControl.SetSliderValue(currentHealth, maxHealth);
    }

    public bool IsMaxHealth()
    {
        if(currentHealth == maxHealth)
            return true;

        return false;
    }

    private IEnumerator Flash()
    {
        canTakeDamage = false;

        foreach (SpriteRenderer sRender in spriteRenderer)
        {
          sRender.material = flashMaterial;
        }

        //spriteRenderer.material = flashMaterial;
        // shader graf control====
        flashMaterial.SetColor("_FlashColor", flashColor);
        flashMaterial.SetFloat("_FlashAmount", flashStrength);
        //========================
        yield return new WaitForSeconds(flashDuration);

        foreach (SpriteRenderer sRender in spriteRenderer)
        {
            sRender.material = defaultMatirial;
        }

        //spriteRenderer.material = defaultMatirial;

        if (currentHealth > 0)
            canTakeDamage = true;

    }

    public void DisableStatsColider()
    {
        curentStateCillider.enabled = false;
    }

    public void EnableStatsColider()
    {
        curentStateCillider.enabled = true;
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

    public float GetCurrentHealth()
    {
        return currentHealth;
    }


    public void SavePlayerHealth()
    {
        playerData.currentHealth = currentHealth;

        ServiceLocator.Current.Get<SaveLoadManager>().SaveData(playerData,
            ServiceLocator.Current.Get<SaveLoadManager>().folderName,
            ServiceLocator.Current.Get<SaveLoadManager>().fileNamePlayerData);
    }

    public void LoadPlayerHealth()
    {
        ServiceLocator.Current.Get<SaveLoadManager>().LoadData(playerData,
            ServiceLocator.Current.Get<SaveLoadManager>().folderName,
            ServiceLocator.Current.Get<SaveLoadManager>().fileNamePlayerData);

        currentHealth = playerData.currentHealth;

    }
}
