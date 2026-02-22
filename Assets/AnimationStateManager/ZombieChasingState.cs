using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChasingState : StateMachineBehaviour
{
    
    Transform player;
    NavMeshAgent agent;
    public float chaseSpeed=12f;
    public float stopChasingDistance=61f;
    public float startAttackingDistance=3f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       player = GameObject.FindGameObjectWithTag("Player").transform;
       agent = animator.GetComponent<NavMeshAgent>();
       agent.speed=chaseSpeed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (SoundManager.Instance.zombieChannel.isPlaying==false)
        {
            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieChase); 
        }

        agent.SetDestination(player.position);
        animator.transform.LookAt(player);

        float distanceFromPlayer= Vector3.Distance(player.position,animator.transform.position); 
        //stop chase
        if (distanceFromPlayer > stopChasingDistance)
        {
            animator.SetBool("IsChasing",false);
        }
        //Attack
        if (distanceFromPlayer < startAttackingDistance)
        {
            animator.SetBool("IsAttacking",true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       agent.SetDestination(agent.transform.position);
       SoundManager.Instance.zombieChannel.Stop();
    }

    
}
