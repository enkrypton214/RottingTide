
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage;
    void OnCollisionEnter( Collision objWeHit)
    {
        if (objWeHit.gameObject.CompareTag("Target"))
        {
            print("Hit"+objWeHit.gameObject.name +"!");
            CreateBullectImpactEffect(objWeHit);
            Destroy(gameObject);
        }
        if (objWeHit.gameObject.CompareTag("Wall"))
        {
            print("Hit a wall !");
            CreateBullectImpactEffect(objWeHit);
            Destroy(gameObject);
        }
        if (objWeHit.gameObject.CompareTag("Zombie"))
        {
            objWeHit.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }

    void CreateBullectImpactEffect(Collision objWeHit)
    {
        ContactPoint contact = objWeHit.contacts[0];
        GameObject hole = Instantiate(
            GlobalRefrences.Instance.bulletImpactEffect,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );
        hole.transform.SetParent(objWeHit.transform);
    }
}
