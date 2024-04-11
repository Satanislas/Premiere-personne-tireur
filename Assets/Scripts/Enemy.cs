using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public int HP = 100;
    private Animator anim;
    private NavMeshAgent nav;

    public bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        HP -= amount;
        if (HP <= 0)
        {
            isDead = true;
            anim.SetTrigger(Random.Range(0,2) == 1 ? "Die1" : "Die2");
            
            SoundManager.instance.ZombieChannel.PlayOneShot(SoundManager.instance.ZDeath);
          
        }
        else
        {
            anim.SetTrigger("Damage");
            
            SoundManager.instance.ZombieChannel2.PlayOneShot(SoundManager.instance.ZHurt);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,2.5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,10f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,21f);
        
        
    }
}
