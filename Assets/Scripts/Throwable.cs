using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public float delay = 3f;
    public float damageRadius = 20f;
    public float explosionForce = 1200f;

    private float countdown;
    private bool hasExploded;
    public bool hasBeenThrown;

    public enum ThrowableType
    {
        Grenade,
        Smoke,
    }

    public ThrowableType type;

    private void Start()
    {
        countdown = delay;
    }

    private void Update()
    {
        if (hasBeenThrown)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0 && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    private void Explode()
    {
        GetThrowableEffect();
        
        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (type)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;
            case ThrowableType.Smoke:
                SmokeEffect();
                break;
        }
    }

    private void SmokeEffect()
    {
        GameObject smokeEffect = GlobalReferences.instance.smokeExplosionEffect;
        Instantiate(smokeEffect, transform.position, transform.rotation);
        
        SoundManager.instance.throwableChannel.PlayOneShot(SoundManager.instance.grenadeExplosion);
        

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Rigidbody>())
            {
                //apply blindness to enemies
            }
            
        }
    }

    private void GrenadeEffect()
    {
        GameObject explosionEffect = GlobalReferences.instance.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);
        
        SoundManager.instance.throwableChannel.PlayOneShot(SoundManager.instance.grenadeExplosion);
        

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Rigidbody>())
            {
                hit.GetComponent<Rigidbody>().AddExplosionForce(explosionForce,transform.position,damageRadius);
            }
            //apply damage to enemies
            if (hit.GetComponent<Enemy>())
            {
                hit.GetComponent<Enemy>().TakeDamage(100);
            }
        }
    }
}
