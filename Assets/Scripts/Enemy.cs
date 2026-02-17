
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
   private int HP=100;
   private Animator animator;
   private NavMeshAgent navMeshAgent;
   
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
        
        }
        else
        {
            animator.SetTrigger("Dmg");
        }
    }

    private void Update()
    {
        if (navMeshAgent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("isWalking",true);
        }
        else
        {
            animator.SetBool("isWalking",false);
        } 
    }

}
