using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdleState : StateMachineBehaviour
{
    float timer;
    public float idletime=0f;
    Transform player;
    public float detectionAreaRadius=60f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       timer = 0;
       player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Patrol Logic
        timer += Time.deltaTime;
        if (timer > idletime)
        {
            animator.SetBool("IsPatrolling",true);
        }
        //Chase State Enter
        float distanceFromPlayer = Vector3.Distance(player.position,animator.transform.position);
        if (distanceFromPlayer < detectionAreaRadius)
        {
            animator.SetBool("IsChasing",true);
        }
    }
    
}
