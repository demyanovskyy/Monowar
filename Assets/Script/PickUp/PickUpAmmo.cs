using UnityEngine;

public class PickUpAmmo : MonoBehaviour
{
    [Header("Ammo")]
    [SerializeField] private TypeOfWeapon ID;
    [SerializeField] private int ammo;

    [Header("Audio")]
    [SerializeField] protected AudioSource audioSours;
    [SerializeField] protected AudioClip audioClip;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out WeaponManager wManager))
        {
            wManager.AddStorageAmmo(ID, ammo);
            audioSours.PlayOneShot(audioClip);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, 1f);
        }
    }
}
