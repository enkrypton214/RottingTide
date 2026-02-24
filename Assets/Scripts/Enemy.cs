
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
   private int HP=100;
   private Animator animator;
   private NavMeshAgent navMeshAgent;
   public bool isDead;
   public List<GameObject> drops;
   private GameObject toDrop;
   
   private void Start()
    {
        animator= GetComponent<Animator>();
        navMeshAgent= GetComponent<NavMeshAgent>();

    }

    public void TakeDamage(int damageAmount)
    {
        HP-=damageAmount;
        if (HP <= 0)
        {
            isDead = true;
            int randomValue = Random.Range(0,2);
            if (randomValue==0)
            {
                animator.SetTrigger("Die1");
                Destroy(gameObject,4f);
            }
            else
            {
                animator.SetTrigger("Die2");
                Destroy(gameObject,4f);    
            }
            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieDeath);

            DropItem();
            
        }
        else
        {
            animator.SetTrigger("Dmg");
            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieHurt);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position,3f);//Attack
        Gizmos.color=Color.blue;
        Gizmos.DrawWireSphere(transform.position,60f);//StartChase
        Gizmos.color=Color.green;
        Gizmos.DrawWireSphere(transform.position,61f);//StopChase
    }

public void DropItem()
{
    CapsuleCollider col = GetComponent<CapsuleCollider>();
    if(col != null) col.enabled = false;

    float dropChance = 0.3f;
    if (Random.value > dropChance) return;

    if (drops.Count == 0) return;
    int dropIndex = Random.Range(0, drops.Count);
    GameObject toDrop = drops[dropIndex];

    Vector3 spawnPos = transform.position + new Vector3(0,0.2f,0);
    Instantiate(toDrop, spawnPos, Quaternion.identity);


}

}
