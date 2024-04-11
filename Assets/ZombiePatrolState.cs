using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolState : StateMachineBehaviour
{
    private float timer;
    public float patrolingTime;

    private Transform player;
    private NavMeshAgent nav;

    public float detectionArea = 18f;
    private float patrolSpeed = 2f;

    private List<Transform> waypoints = new List<Transform>();
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = animator.GetComponent<NavMeshAgent>();

        nav.speed = patrolSpeed;
        timer = 0f;

        GameObject waypoint = GameObject.FindGameObjectWithTag("Waypoint");
        foreach (Transform t in waypoint.transform)
        {
            waypoints.Add(t);
        }

        Vector3 nextPosition = waypoints[Random.Range(0, waypoints.Count)].position;
        nav.SetDestination(nextPosition);
    }

   
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SoundManager.instance.ZombieChannel.isPlaying == false)
        {
            SoundManager.instance.ZombieChannel.clip = SoundManager.instance.ZWalking;
            SoundManager.instance.ZombieChannel.PlayDelayed(1f);
        }
        
        if (nav.remainingDistance <= nav.stoppingDistance)
        {
            nav.SetDestination(waypoints[Random.Range(0, waypoints.Count)].position);
        }

        timer += Time.deltaTime;
        if (timer > patrolingTime)
        {
            animator.SetBool("isWalking",false);
        }
        
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionArea)
        {
            animator.SetBool("isChasing",true);
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nav.SetDestination(animator.transform.position);
        
        SoundManager.instance.ZombieChannel.Stop();
    }
}
