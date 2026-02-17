
using UnityEngine;

public class GlobalRefrences : MonoBehaviour
{
    public static GlobalRefrences Instance {get;set;}
    public GameObject bulletImpactEffect;
    public GameObject grenadeExplosionEffect;
    public GameObject smokeGrenadeEffect;

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
}
