using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            print("hit " + other.gameObject.name + "!");
            
            CreateBulletImpactEffect(other);
            
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            CreateBulletImpactEffect(other);
            
            Destroy(gameObject);
        }


        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);

            CreateBloodEffect(other);
            
            Destroy(gameObject);
        }
    }

    private void CreateBloodEffect(Collision hit)
    {
        ContactPoint contact = hit.contacts[0];

        GameObject blood = Instantiate(GlobalReferences.instance.ZombieBloodEffect,contact.point,Quaternion.LookRotation(contact.normal));
        
        blood.transform.SetParent(hit.transform);
    }

    void CreateBulletImpactEffect(Collision hit)
    {
        ContactPoint contact = hit.contacts[0];

        GameObject hole = Instantiate(GlobalReferences.instance.bulletImpactEffect,contact.point,Quaternion.LookRotation(contact.normal));
        
        hole.transform.SetParent(hit.transform);
    }
}
