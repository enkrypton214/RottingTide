
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
   private int HP=100;
   private Animator animator;
   private NavMeshAgent navMeshAgent;
   public bool isDead;
   
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
            int randomValue = Random.Range(0,2);
            if (randomValue==0)
            {
                animator.SetTrigger("Die1");
            }
            else
            {
                animator.SetTrigger("Die2");    
            }
            isDead = true;
            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieDeath);
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
        Gizmos.DrawWireSphere(transform.position,46f);//StartChase
        Gizmos.color=Color.green;
        Gizmos.DrawWireSphere(transform.position,45f);//StopChase
    }

}
