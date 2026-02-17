using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance {get;set;}
    
    //Ammo
    public TextMeshProUGUI magazineAmountUI;
    public TextMeshProUGUI totalAmountUI;
    public Image ammoTypeUI;

    //Weapon
    public Image activeWeaponUI;
    public Image inActiveWeaponUI;

    //Throwables
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;
    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;
    public Sprite greySlot;
    public GameObject middleDot;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        
        Weapon inActiveWeapon = GetInactiveWeaponSlot().GetComponentInChildren<Weapon>();

        if(!activeWeapon)
        {
            magazineAmountUI.text="";
            totalAmountUI.text="";
            ammoTypeUI.sprite = emptySlot;
            activeWeaponUI.sprite = emptySlot;
            inActiveWeaponUI.sprite = emptySlot;
            return;
        }

        if (activeWeapon)
        {
            magazineAmountUI.text = $"{activeWeapon.bulletsLeft/activeWeapon.bulletPerBurst}";
            totalAmountUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);
            activeWeaponUI.sprite = GetWeaponSprite(activeWeapon.thisWeaponModel);
        }

        if (inActiveWeapon)
        {
            inActiveWeaponUI.sprite = GetWeaponSprite(inActiveWeapon.thisWeaponModel);
        }

        if (WeaponManager.Instance.lethalCount <= 0)
        {
            lethalUI.sprite=greySlot;
        }
        if (WeaponManager.Instance.tacticalCount <= 0)
        {
            lethalUI.sprite=greySlot;
        }
        
    }
    
    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        string prefabName = model switch
        {
            Weapon.WeaponModel.Pistol => "Pistol_Weapon",
            Weapon.WeaponModel.Ak47 =>"AK47_Weapon",
            _=> null
        };
        
        if (string.IsNullOrEmpty(prefabName)) return emptySlot;

        GameObject prefab = Resources.Load<GameObject>(prefabName);

        if (prefab == null) return emptySlot;
        SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
        return sr != null && sr.sprite != null ? sr.sprite : emptySlot;
    }

    private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        string prefabName = model switch
        {
            Weapon.WeaponModel.Pistol => "Pistol_Ammo",
            Weapon.WeaponModel.Ak47 =>"AK47_Ammo",
            _=> null
        };
        
        if (string.IsNullOrEmpty(prefabName)) return emptySlot;

        GameObject prefab = Resources.Load<GameObject>(prefabName);

        if (prefab == null) return emptySlot;
        SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
        return sr != null && sr.sprite != null ? sr.sprite : emptySlot;
    }

    private GameObject GetInactiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }

        }
        return null;
    }

    internal void UpdateThrowables()
    {
        lethalAmountUI.text=$"{WeaponManager.Instance.lethalCount}";
        tacticalAmountUI.text=$"{WeaponManager.Instance.tacticalCount}";
        switch (WeaponManager.Instance.equippedLethalType)
        {
            case ThrowAbles.ThrowableType.Grenade:
                    lethalUI.sprite=Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                    break;
        }
        switch (WeaponManager.Instance.equippedTacticalType)
        {
            case ThrowAbles.ThrowableType.Smoke:
                    tacticalUI.sprite=Resources.Load<GameObject>("SmokeGrenade").GetComponent<SpriteRenderer>().sprite;
                    break;
        }
    } 
}
