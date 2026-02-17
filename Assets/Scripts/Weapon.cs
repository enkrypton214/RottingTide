using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    public int weaponDamage;
    //Shooting
    public bool isShooting, readyToShoot;
    bool allowReset=true;
    public float shootingdelay;

    //Burst
    public int bulletPerBurst = 3;
    public int burstBulletsLeft;

    //Spread
    public float spreadIntensity;
    public float hipSpreadIntensity;
    public float adsSpreadIntensity;

    //Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity=30f;
    public float bulletLifetime =2f;

    //Reload
    public float reloadTime;
    public int magazineSize,bulletsLeft;
    public bool isReloading;

    //Muzzle and Recoil
    public GameObject muzzleEffect;
    internal Animator animator;

    //SpawnPosition

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
    public Vector3 spawnScale;
    public enum WeaponModel
    {
        Pistol,
        Ak47
    }
    public WeaponModel thisWeaponModel;
    public enum ShootingMode
    {
        Single,Burst,Auto
    };   
    public ShootingMode currentShootingMode;

    bool isADS;
    public Camera MainCamera;

    private void Awake()
    {
        readyToShoot=true;
        burstBulletsLeft=bulletPerBurst;
        animator= GetComponent<Animator>();
        bulletsLeft=magazineSize;
        spreadIntensity= hipSpreadIntensity;
    }
    void Update()
    {   if(isActiveWeapon){

        foreach (Transform child in transform)
            {
                child.gameObject.layer=LayerMask.NameToLayer("WeaponRender");
            }

            if (Input.GetMouseButtonDown(1))
            {
                EnterADS();
            }
            if (Input.GetMouseButtonUp(1))
            {
                ExitADS();
            }

        GetComponent<Outline>().enabled = false;    
        if(bulletsLeft==0 & isShooting)
        {
            SoundManager.Instance.emptyMagazinePistol.Play();
        }
        if (currentShootingMode==ShootingMode.Auto)
        {
            //holidng mouse0
            isShooting=Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentShootingMode==ShootingMode.Single || currentShootingMode==ShootingMode.Burst)
        {
            //tapping mouse0
            isShooting=Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (readyToShoot && isShooting && bulletsLeft>0&&isReloading==false)
        {
            burstBulletsLeft= bulletPerBurst;
            FireWeapon();
        }
        //Reload on R
        if(Input.GetKey(KeyCode.R) && bulletsLeft<magazineSize && isReloading == false && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel)>0)
        {
            Reload();
        }
        //Automatic reload
        if(isShooting==false && readyToShoot && bulletsLeft <=0 && isReloading==false && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel)>0)
        {
            Reload();
        }

        
        
    }
    else
        {
        foreach (Transform child in transform)
            {
                child.gameObject.layer=LayerMask.NameToLayer("Default");
            }
        }
    }   

    private void EnterADS()
    {
        animator.SetTrigger("enterADS");
                isADS = true;
                Camera.main.fieldOfView = 45;
                HUDManager.Instance.middleDot.SetActive(false);
                spreadIntensity=adsSpreadIntensity;
    }

    private void ExitADS()
    {
        
                animator.SetTrigger("exitADS");
                isADS = false;
                Camera.main.fieldOfView = 60;
                HUDManager.Instance.middleDot.SetActive(true);
                spreadIntensity=hipSpreadIntensity;
            
    }
    private void FireWeapon()
    {
        bulletsLeft--;
        muzzleEffect.GetComponent<ParticleSystem>().Play(); 
        if(isADS)
        {
            animator.SetTrigger("RecoilADS");
        }
        else
        {
        animator.SetTrigger("Recoil");    
        }
        // SoundManager.Instance.shootingSoundPistol.Play();
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot=false;
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        //Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position,Quaternion.identity);

        //putting weapon damage
        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage=weaponDamage;

        //point towards shooting direction
        bullet.transform.forward=shootingDirection;
        //Shoot Bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection*bulletVelocity,ForceMode.Impulse);
        //Destroy Bullet
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletLifetime));
        //check if we are done shooting
        if (allowReset)
        {
            Invoke("ResetShot",shootingdelay);
            allowReset=false;
        } 
        if(currentShootingMode ==ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon",shootingdelay);
        }
    }

    private void Reload()
    {
        
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);
        animator.SetTrigger("Reload");
        isReloading = true;
        Invoke("ReloadCompleted",reloadTime);
    }


    

    private void ReloadCompleted()
{
    int bulletsNeeded = magazineSize - bulletsLeft;
    int availableAmmo = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
    int bulletsToReload = Mathf.Min(bulletsNeeded, availableAmmo);

    bulletsLeft += bulletsToReload;

    WeaponManager.Instance.DecreaseTotalAmmo(bulletsToReload, thisWeaponModel);

    isReloading = false;
}
    private void ResetShot()
    {
        readyToShoot=true;
        allowReset=true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {   //shooting from middle of screen to check where we are pointing
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray,out hit))
        {   //shooting something
            targetPoint = hit.point;
        }
        else
        {   //shooting air
            targetPoint= ray.GetPoint(100);
        }
        Vector3 direction = targetPoint - bulletSpawn.position;
        //setting up spread
        float z=UnityEngine.Random.Range(-spreadIntensity,spreadIntensity);
        float y=UnityEngine.Random.Range(-spreadIntensity,spreadIntensity);
        //return direction and spread
        return direction + new Vector3(0,y,z);
    }

     private IEnumerator  DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay) ;
        Destroy(bullet);
    }
}
