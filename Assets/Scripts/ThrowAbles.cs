
using Unity.VisualScripting;
using UnityEngine;

public class ThrowAbles : MonoBehaviour
{
    public float delay=3f;
    public float damageRadius=20f;
    
    public float explosionForce=1200f;

    float countdown;
    public bool hasExploded=false;
    public bool hasBeenThrown = false;

    public enum ThrowableType{
        None,
        Grenade,
        Smoke,
        Flash,
    }
    public ThrowableType throwableType;


    private void Start()
    {
        countdown= delay;
    }

    private void Update()
    {
        if (hasBeenThrown)
        {
            countdown-=Time.deltaTime;
            if(countdown<0 && !hasExploded)
            {
                Explode();
                hasExploded=true;
            }
        }
    }
    private void Explode()
    {
        GetThrowableEffect();   
        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case ThrowableType.Grenade:
                {
                    GreanadeEffect();
                    break;
                }
                case ThrowableType.Smoke:
                {
                    SmokeGreanadeEffect();
                    break;
                }
        }
    }

    private void GreanadeEffect()
    {
        //visual effect
        GameObject explosionEffect= GlobalRefrences.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect,transform.position, transform.rotation);

        //play Sound
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.grenadeSound);


        //Physical Effect
        Collider[] colliders = Physics.OverlapSphere(transform.position,damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce,transform.position,damageRadius);
            }

            if (objectInRange.gameObject.GetComponent<Enemy>())
            {
                 if(objectInRange.gameObject.GetComponent<Enemy>().isDead == false)            
                {
                    objectInRange.gameObject.GetComponent<Enemy>().TakeDamage(100);
                }
            }
        }
    }   

     private void SmokeGreanadeEffect()
    {
        //visual effect
        GameObject smokeEffect= GlobalRefrences.Instance.smokeGrenadeEffect;
        Instantiate(smokeEffect,transform.position, transform.rotation);

        //play Sound
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.SmokeGrenadeSound);


        //Physical Effect
        Collider[] colliders = Physics.OverlapSphere(transform.position,damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //apply blidness to enemies

            }

            //Apply dmg here
        }
    }
}


