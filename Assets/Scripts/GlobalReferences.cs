using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences instance;

    public GameObject bulletImpactEffect;

    public GameObject grenadeExplosionEffect;
    public GameObject smokeExplosionEffect;
    
    private void Start()
    {
        if (instance is not null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
