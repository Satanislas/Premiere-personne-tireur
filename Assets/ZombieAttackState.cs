using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackState : StateMachineBehaviour
{
    private Transform player;
    private NavMeshAgent nav;

    public float stopAttacking = 2.5f;
    
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = animator.GetComponent<NavMeshAgent>();
    }

   
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SoundManager.instance.ZombieChannel.isPlaying == false)
        {
            SoundManager.instance.ZombieChannel.PlayOneShot(SoundManager.instance.ZAttack);
        }
        
        animator.transform.LookAt(player);
        
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        if (distanceFromPlayer > stopAttacking)
        {
            animator.SetBool("isAttacking",false);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - nav.transform.position;
        nav.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = nav.transform.eulerAngles.y;
        nav.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
