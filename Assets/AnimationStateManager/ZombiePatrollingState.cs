using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrollingState : StateMachineBehaviour
{
    float timer;
    public float patrollingTime=5f;
    Transform player;
    NavMeshAgent agent;
    public float detectionAreaRadius=60f;
    public float patrolSpeed = 2f;
    List<Transform> waypointList = new List<Transform>();
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       player = GameObject.FindGameObjectWithTag("Player").transform;
       agent = animator.GetComponent<NavMeshAgent>();
       agent.speed= patrolSpeed;
       timer=0;

       //Get list and move to Waypoint1
       GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoint");
       foreach(Transform t in waypointCluster.transform)
        {
            waypointList.Add(t);
        }
        Vector3 nextPosition = waypointList[Random.Range(0,waypointList.Count)].position;
        agent.SetDestination(nextPosition);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SoundManager.Instance.zombieChannel.isPlaying==false)
        {
            SoundManager.Instance.zombieChannel.clip=SoundManager.Instance.zombieWalking;
            SoundManager.Instance.zombieChannel.PlayDelayed(1f);
        }


       //Check weather enemy reached waypoint
       if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(waypointList[Random.Range(0,waypointList.Count)].position);
        }

        //if patrol over idle
        timer += Time.deltaTime;
        if (timer > patrollingTime)
        {
            animator.SetBool("IsPatrolling",false);
        }
        //Chase State Enter
        float distanceFromPlayer = Vector3.Distance(player.position,animator.transform.position);
        if (distanceFromPlayer < detectionAreaRadius)
        {
            animator.SetBool("IsChasing",true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
       SoundManager.Instance.zombieChannel.Stop();
    }

    
}
