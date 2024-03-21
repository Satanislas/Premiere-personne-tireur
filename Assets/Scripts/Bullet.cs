using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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
    }

    void CreateBulletImpactEffect(Collision hit)
    {
        ContactPoint contact = hit.contacts[0];

        GameObject hole = Instantiate(GlobalReferences.instance.bulletImpactEffect,contact.point,Quaternion.LookRotation(contact.normal));
        
        hole.transform.SetParent(hit.transform);
    }
}
