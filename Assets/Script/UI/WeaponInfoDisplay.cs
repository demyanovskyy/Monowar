using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoDisplay : MonoBehaviour
{
    [SerializeField] private Image currentWeaponIcon;
    [SerializeField] private TextMeshProUGUI currentAmoText;
    [SerializeField] private TextMeshProUGUI storageAmmoText;


    private void OnEnable()
    {
        WeaponManager.OnUpdateAllInfo += UpdateAllWeaponInfo;
        Shooting.OnUpdateAmmo += UpdateAmmoInfo;

    }

    private void OnDisable()
    {
        WeaponManager.OnUpdateAllInfo -= UpdateAllWeaponInfo;
        Shooting.OnUpdateAmmo -= UpdateAmmoInfo;
    }

    private void UpdateAllWeaponInfo(Sprite weaponSpriteIcon, int currentAmmo, int maxAmmo, int storageAmmo)
    {
        currentWeaponIcon.sprite = weaponSpriteIcon;
        currentAmoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        storageAmmoText.text = storageAmmo.ToString();
    }

    private void UpdateAmmoInfo(int currentAmmo, int maxAmmo, int storageAmmo)
    {
        currentAmoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        storageAmmoText.text = storageAmmo.ToString();
    }
}
