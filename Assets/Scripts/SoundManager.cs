
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {get;set;}
    
    public AudioSource shootingChannel;
    public AudioSource reloadChannel;
    public AudioSource throwablesChannel;

    //Pistol
    public AudioClip reloadPistol;
    public AudioClip pistolShot;

    //Ak47
    public AudioClip reloadAk47;
    public AudioClip ak47Shot;
    public AudioSource emptyMagazinePistol;


    //Grenade
    public AudioClip grenadeSound;
    public AudioClip SmokeGrenadeSound;
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

    public void PlayShootingSound(WeaponModel currentWeapon)
    {
        switch (currentWeapon)
        {
            case WeaponModel.Pistol:
                shootingChannel.PlayOneShot(pistolShot);
                break;

            case WeaponModel.Ak47:
                shootingChannel.PlayOneShot(ak47Shot);
                break;
        }
        
    }
    
    public void PlayReloadSound(WeaponModel currentWeapon)
    {
        switch (currentWeapon)
        {
            case WeaponModel.Pistol:
                reloadChannel.PlayOneShot(reloadPistol);
                break;

            case WeaponModel.Ak47:
                reloadChannel.PlayOneShot(reloadAk47);
                break;
        }
        
    }
}
