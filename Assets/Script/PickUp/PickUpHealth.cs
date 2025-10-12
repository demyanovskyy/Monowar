using UnityEngine;

public class PickUpHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int addHealth;

    [Header("Audio")]
    [SerializeField] protected AudioSource audioSours;
    [SerializeField] protected AudioClip audioClip;

    private PlayerStats playerStats;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerStats = collision.GetComponentInChildren<PlayerStats>();

        if (playerStats.IsMaxHealth())
            return;

            playerStats.AddHealth(addHealth);
            audioSours.PlayOneShot(audioClip);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, 1f);

    }
}
