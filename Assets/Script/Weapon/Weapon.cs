using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Weapon : MonoBehaviour
{
    [Header("Base Data")]
    public TypeOfWeapon weaponType;
    public Sprite weaponIconSprite;
    public bool weaponActiv = false;
    public bool shootEnable = true;
    public float shootCooldown;
    public bool isAvtomatic;
 
    [Header("Bullet Data")]
    public Transform _shootPoint;
    public Bullet _bulletPrefab;

    [Header("Shell Data")]
    public Transform shellSpawnPoint;
    public IsPooleble shellPrefab;

    [Header("Reload")]
    public float reloadTime;
    public bool isReloading;

    [Header("Ammo")]
    public int curentAmmo;
    public int maxAmmo;
    public int storageAmmo;

    [Header("Flash Particle Data")]
    public bool flashingParticle = false;
    public GameObject MuzleFlashPartical;
 
    [Header("Flash Sprite Data")] 
    public bool flashingSprite = false;
    public GameObject MuzleFlash;
    [Range(0, 5)]
    public int framToFlash = 1;

    [Header("Recoil")]
    public float recoilStrenght;
    public float recoilTime;// it needs to be less that "shootCooldown"
    public float armRecoil = 1.5f;

    public Vector3 tempPosColbackWeaponPos;
    public Transform defaultWeaponVectorPos;

    [Header("Audio")]
    public AudioSource audioSours;
    public AudioClip audioClip;

    public List<ParticleSystem> particles;

    [SerializeField]
    private WeaponData weaponData = new WeaponData();

    private void Awake()
    {

        if (flashingParticle) FillListParticle();

        MuzleFlash.SetActive(false);

        tempPosColbackWeaponPos = new Vector2(defaultWeaponVectorPos.localPosition.x, defaultWeaponVectorPos.localPosition.y);
    }

    private void FillListParticle()
    {
        for (int i = 0; i < MuzleFlashPartical.transform.childCount; i++)
        {
            var ps = MuzleFlashPartical.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
            {
                particles.Add(ps);
            }
        }
    }


    public IEnumerator FlashTime()
    {
        MuzleFlash.SetActive(true);
        int frameflash = 0;
        flashingSprite = true;
        while (frameflash <= framToFlash)
        {
            frameflash++;
            yield return null;
        }
        MuzleFlash.SetActive(false);
        flashingSprite = false;
    }

    public bool ReloadCheck()
    {
        int neededAmmo = maxAmmo - curentAmmo;
        if (neededAmmo <= 0 || storageAmmo <= 0)
            return false;

        return true;
    }

    public void Reload()
    {
        int neededAmmo = maxAmmo - curentAmmo;
        int ammoToReload = Mathf.Min(neededAmmo, storageAmmo);
        curentAmmo += ammoToReload;
        storageAmmo -= ammoToReload;
        isReloading = false;

    }


    public void SaveWeaponData()
    {
        weaponData.ID = weaponType.ToString();
        weaponData.currentAmmo = curentAmmo;
        weaponData.storageAmmo = storageAmmo;
        ServiceLocator.Current.Get<SaveLoadManager>().SaveData(weaponData, ServiceLocator.Current.Get<SaveLoadManager>().folderName, weaponType.ToString() + ".json");
    }

    public void LoadWeaponData()
    {
        ServiceLocator.Current.Get<SaveLoadManager>().LoadData(weaponData, ServiceLocator.Current.Get<SaveLoadManager>().folderName, weaponType.ToString() + ".json");
        if (weaponData.ID != "")
        {
            curentAmmo = weaponData.currentAmmo;
            storageAmmo = weaponData.storageAmmo;
        }
    }

    public void SootEnable()
    {
        shootEnable = true;
    }

    public void SootDisable()
    {
        shootEnable = false;
    }
}
