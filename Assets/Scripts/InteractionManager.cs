using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager instance;

    public GameObject weapon;
    public GameObject AmmoBox;
    public GameObject throwable;
    
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

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.CompareTag("Weapon"))
            {
                if (weapon) weapon.GetComponent<Outline>().enabled = false;
                
                weapon = hit.transform.gameObject;
                weapon.GetComponent<Outline>().enabled = true;
                
                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.instance.PickupWeapon(weapon);
                }
            }
            else
            {
                if (weapon)
                {
                    weapon.GetComponent<Outline>().enabled = false;
                }
            }
            
            //Ammo Box
            if (hit.transform.gameObject.CompareTag("AmmoBox"))
            {
                if (AmmoBox) AmmoBox.GetComponent<Outline>().enabled = false;
                AmmoBox = hit.transform.gameObject;
                Debug.Log("AmmoBox selected");
                AmmoBox.GetComponent<Outline>().enabled = true;
                
                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.instance.PickupAmmo(AmmoBox);
                }
            }
            else
            {
                if (AmmoBox)
                {
                    AmmoBox.GetComponent<Outline>().enabled = false;
                }
            }
            
            //Throwable
            if (hit.transform.GetComponent<Throwable>())
            {
                if (throwable) throwable.GetComponent<Outline>().enabled = false;
                throwable = hit.transform.gameObject;
                Debug.Log("throwable selected");
                throwable.GetComponent<Outline>().enabled = true;
                
                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.instance.PickupThrowable(throwable.GetComponent<Throwable>());
                }
            }
            else
            {
                if (throwable)
                {
                    throwable.GetComponent<Outline>().enabled = false;
                }
            }
        }

        
    }
}
