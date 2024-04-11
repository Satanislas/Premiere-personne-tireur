using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChaseState : StateMachineBehaviour
{
    private NavMeshAgent nav;
    private Transform player;

    public float chaseSpeed = 6f;
    public float stopChasinDistance = 21;
    public float attackingDistance = 2.5f;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = animator.GetComponent<NavMeshAgent>();

        nav.speed = chaseSpeed;
    }

   
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SoundManager.instance.ZombieChannel.isPlaying == false)
        {
            SoundManager.instance.ZombieChannel.clip = SoundManager.instance.ZChase;
            SoundManager.instance.ZombieChannel.PlayDelayed(1f);
        }
        
        nav.SetDestination(player.position);
        animator.transform.LookAt(player);

        float distanceFromPlayer = Vector3.Distance(
            new Vector3(player.position.x,animator.transform.position.y,player.position.z), 
            animator.transform.position
            );

        if (distanceFromPlayer > stopChasinDistance)
        {
            animator.SetBool("isChasing",false);
        }
        
        if (distanceFromPlayer < attackingDistance)
        {
            animator.SetBool("isAttacking",true);
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nav.SetDestination(animator.transform.position);
        
        SoundManager.instance.ZombieChannel.Stop();
    }
}