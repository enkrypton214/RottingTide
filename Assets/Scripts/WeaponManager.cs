using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance {get;set;}

    public List<GameObject> weaponSlots;
    public GameObject activeWeaponSlot;


    public int totalRifleAmmo=0;
    public int totalPistolAmmo=0;
    public float throwForce = 10f;
    
    public GameObject grenadePrefab;
    public GameObject throwAbleSpawn;
    public float forceMultiplier=0;
    public float forceMultiplierLimit=3f;

    public int maxLethal=3;
    public int lethalCount;
    public ThrowAbles.ThrowableType equippedLethalType;

    public GameObject smokeGrenadePrefab;
    public int maxTactical=2;
    public int tacticalCount;
    public ThrowAbles.ThrowableType equippedTacticalType;
    

    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
        equippedLethalType=ThrowAbles.ThrowableType.None;
        equippedTacticalType=ThrowAbles.ThrowableType.None;
    }

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
        foreach(GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
           SwitchActiveSlot(0); 
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
           SwitchActiveSlot(1); 
        }

        if (Input.GetKey(KeyCode.G)|| Input.GetKey(KeyCode.T))
        {
            
             forceMultiplier+=Time.deltaTime;
            if (forceMultiplier > forceMultiplierLimit)
            {
                forceMultiplier=forceMultiplierLimit;
            }
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            if (lethalCount > 0)
            {
                ThrowLethal();
            }
            forceMultiplier=0;
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            if (tacticalCount > 0)
            {
                ThrowTacical();
            }
            forceMultiplier=0;
        }
        
    }

    

    public void PickupWeapon(GameObject pickedUpWeapon)
    {
        AddWeaponIntoActiveSlot(pickedUpWeapon);
    }
    private void AddWeaponIntoActiveSlot(GameObject pickedUpWeapon)
    {   
        DropCurrentWeapon(pickedUpWeapon);
        Weapon weapon = pickedUpWeapon.GetComponentInParent<Weapon>();
        GameObject weaponRoot = weapon.gameObject;
        weaponRoot.transform.SetParent(activeWeaponSlot.transform, false);
        
        weaponRoot.transform.localPosition = new Vector3(weapon.spawnPosition.x,weapon.spawnPosition.y,weapon.spawnPosition.z);
        weaponRoot.transform.localRotation = Quaternion.Euler(weapon.spawnRotation);
        weaponRoot.transform.localScale = weapon.spawnScale;
        weapon.isActiveWeapon=true;
        weapon.animator.enabled =true;
    }
        private void DropCurrentWeapon(GameObject pickedUpWeapon)
        {
            if(activeWeaponSlot.transform.childCount>0)
            {
                GameObject weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;
                Weapon weaponScript = weaponToDrop.GetComponentInParent<Weapon>();
                if (weaponScript != null) 
                {
                    weaponScript.isActiveWeapon = false;
                    weaponScript.animator.enabled = false;
                }

                // Detach from slot
                weaponToDrop.transform.SetParent(null); // world root

                // Set world position/rotation to drop in front of player
                weaponToDrop.transform.position = pickedUpWeapon.transform.position; 
                weaponToDrop.transform.rotation = pickedUpWeapon.transform.rotation;
            }
        }

        public void SwitchActiveSlot(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon =false;
        }
        activeWeaponSlot = weaponSlots[slotNumber];
        
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon =true ;
        }
    }
        public void PickupAmmo(AmmoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoBox.AmmoType.pistolAmmo:
            totalPistolAmmo += ammo.ammoAmount;
            break;
            case AmmoBox.AmmoType.RifleAmmo:
            totalRifleAmmo += ammo.ammoAmount;
            break;
            
        }
    }
        public void PickupThrowable(ThrowAbles throwAbles)
    {
        switch (throwAbles.throwableType)
        {
            case ThrowAbles.ThrowableType.Grenade:
            PickupThrowableLethal(ThrowAbles.ThrowableType.Grenade);
            break;
            case ThrowAbles.ThrowableType.Smoke:
            PickupThrowableTactical(ThrowAbles.ThrowableType.Smoke);
            break;
        }
    }

   
    
    private void PickupThrowableLethal(ThrowAbles.ThrowableType lethal)
    {
        if(equippedLethalType==lethal || equippedLethalType == ThrowAbles.ThrowableType.None)
        {
            equippedLethalType=lethal;
            if (lethalCount < maxLethal)
            {
                lethalCount++;
                Destroy(InteractionManager.Instance.hoveredThrowAble.gameObject);
                HUDManager.Instance.UpdateThrowables();
            }
            else
            {
                print("Lethal Limit Reached");
            }
        }
        else
        {
            // Cannot pickup other lethals 
            // Option to swap Lethals
        }
        
    }
    private void PickupThrowableTactical(ThrowAbles.ThrowableType tactical)
    {
        if(equippedTacticalType==tactical || equippedTacticalType == ThrowAbles.ThrowableType.None)
        {
            equippedTacticalType=tactical;
            if (tacticalCount < maxTactical)
            {
                tacticalCount++;
                Destroy(InteractionManager.Instance.hoveredThrowAble.gameObject);
                HUDManager.Instance.UpdateThrowables();
            }
            else
            {
                print("Tactical Limit Reached");
            }
        }
        else
        {
            // Cannot pickup other Tacticals 
            // Option to swap Tacticals
        }
        
    }

    private void ThrowLethal()
    {
        GameObject lethalPrefab = GetThrowablePrefab(equippedLethalType);
        GameObject throwable = Instantiate(lethalPrefab, throwAbleSpawn.transform.position,Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward*(throwForce*forceMultiplier),ForceMode.Impulse);

        throwable.GetComponent<ThrowAbles>().hasBeenThrown = true;
        lethalCount-=1;
        if (lethalCount <= 0)
        {
            equippedLethalType=ThrowAbles.ThrowableType.None;
        }
        HUDManager.Instance.UpdateThrowables();
    }

     private void ThrowTacical()
    {
        GameObject tacticalPrefab = GetThrowablePrefab(equippedTacticalType);
        GameObject throwable = Instantiate(tacticalPrefab, throwAbleSpawn.transform.position,Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward*(throwForce*forceMultiplier),ForceMode.Impulse);

        throwable.GetComponent<ThrowAbles>().hasBeenThrown = true;
        tacticalCount-=1;
        if (tacticalCount <= 0)
        {
            equippedTacticalType=ThrowAbles.ThrowableType.None;
        }
        HUDManager.Instance.UpdateThrowables();
    }

    public GameObject GetThrowablePrefab(ThrowAbles.ThrowableType equippedType)
    {
        switch (equippedType)
        {
            case ThrowAbles.ThrowableType.Grenade:
            return grenadePrefab;
            case ThrowAbles.ThrowableType.Smoke:
            return smokeGrenadePrefab;
        }
        return new();
    }

    public int CheckAmmoLeftFor(Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.Ak47:
            return totalRifleAmmo;
            
            case Weapon.WeaponModel.Pistol:
            return totalPistolAmmo;

            default:
            return 0;
        }
    }
    

    internal void DecreaseTotalAmmo(int bulletsToDecrease, Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.Pistol:
            totalPistolAmmo -= bulletsToDecrease;
            break;
            case Weapon.WeaponModel.Ak47:
            totalRifleAmmo -= bulletsToDecrease;
            break;
            
        }
        
    }
}
