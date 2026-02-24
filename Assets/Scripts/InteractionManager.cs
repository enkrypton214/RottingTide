
using UnityEditor;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private Camera cam;
    public static InteractionManager Instance {get;set;}
    public Weapon hoveredWeapon=null;
    public AmmoBox hoveredAmmoBox =null;
    public ThrowAbles hoveredThrowAble =null;


    private void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
    }
    else
    {
        Instance = this;
    }

    cam = Camera.main; // cache camera
}

    private void Update()
    {
            if (hoveredWeapon != null && (hoveredWeapon.isActiveWeapon == false || !hoveredWeapon.gameObject.activeInHierarchy))
            {
                Outline outline = hoveredWeapon.GetComponentInParent<Outline>();
                if (outline != null)
                    outline.enabled = false;

                hoveredWeapon = null;
            }

            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
            GameObject objectHitByRaycast= hit.transform.gameObject;
            if (objectHitByRaycast.GetComponentInParent<Weapon>() && objectHitByRaycast.GetComponentInParent<Weapon>().isActiveWeapon ==false)
            {

                if(hoveredWeapon)
                {
                    hoveredWeapon.GetComponentInParent<Outline>().enabled = false;
                }

                hoveredWeapon = objectHitByRaycast.gameObject.GetComponentInParent<Weapon>();
                if (hoveredWeapon != null)
                {
                   Outline outline = hoveredWeapon.GetComponentInParent<Outline>();
                   if (outline != null)
                       outline.enabled = true;
                }
                if(Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject);
                }
            }   
            else
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponentInParent<Outline>().enabled=false;
                }
            }

            if (objectHitByRaycast.GetComponentInParent<AmmoBox>())
            
            {
                if(hoveredAmmoBox)
                {
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }

                hoveredAmmoBox = objectHitByRaycast.gameObject.GetComponent<AmmoBox>();
                {
                   Outline outline = hoveredAmmoBox.GetComponentInParent<Outline>();
                   if (outline != null)
                       outline.enabled = true;
                }
                if(Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupAmmo(hoveredAmmoBox);
                    Destroy(objectHitByRaycast.gameObject);
                }
            }   
            else
            {
                if (hoveredAmmoBox)
                {
                    hoveredAmmoBox.GetComponentInParent<Outline>().enabled=false;
                }
            }

            if (objectHitByRaycast.GetComponentInParent<ThrowAbles>())
            
            {

                if(hoveredThrowAble)
                {
                    hoveredThrowAble.GetComponent<Outline>().enabled = false;
                }
                hoveredThrowAble = objectHitByRaycast.gameObject.GetComponent<ThrowAbles>();
                {
                   Outline outline = hoveredThrowAble.GetComponentInParent<Outline>();
                   if (outline != null)
                       outline.enabled = true;
                }
                if(Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupThrowable(hoveredThrowAble);
                }
            }   
            else
            {
                if (hoveredThrowAble)
                {
                    hoveredThrowAble.GetComponentInParent<Outline>().enabled=false;
                }
            }
        
        }

        
    }
}
