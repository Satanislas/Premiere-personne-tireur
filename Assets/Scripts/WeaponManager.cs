using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    public List<GameObject> Slots;
    private List<bool> SlotIsActive;
    private int activeSlot;
    private Weapon currentWeapon;

    [Header("Throwable")] 
    public int grenades;
    public int smokes;
    public float throwForce = 10f;
    public GameObject grenadePrefab;
    public GameObject smokePrefab;
    public GameObject throwableSpawn;
    public float forceMultiplier = 0f;
    public float forceMultiplierLimit = 2f;
    
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
        
        activeSlot = 0;
        SlotIsActive = new List<bool>();
        for (int i = 0; i < Slots.Count; i++)
        {
            SlotIsActive.Add(false);
        }
    }

    public void PickupWeapon(GameObject weapon)
    {
        switch (weapon.name)
        {
            case "pistol":
                SlotIsActive[0] = true;
                SwapSlot(0);
                break;
            case "M4":
                SlotIsActive[1] = true;
                SwapSlot(1);
                break;
        }
        Destroy(weapon);
    }

    public void PickupAmmo(GameObject ammo)
    {
        foreach (var slot in Slots)
        {
            Weapon weapon = slot.GetComponentInChildren<Weapon>();
            weapon.totalAmmo += weapon.magazineSize;
            if (weapon.totalAmmo > weapon.maxAmmoAmmount) weapon.totalAmmo = weapon.maxAmmoAmmount;
        }
        Destroy(ammo);
    }

    public void SwapSlot(int newSlot)
    {
        Slots[activeSlot].SetActive(false);
        activeSlot = newSlot;
        Slots[activeSlot].SetActive(true);
        currentWeapon = GetCurrentWeapon();
    }

    private void Update()
    {
        if (currentWeapon is not null && currentWeapon.isReloading) return;
        
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            int i = 1;
            while (!SlotIsActive[(activeSlot + i) % SlotIsActive.Count])
            {
                i++;
                if(i == SlotIsActive.Count) return;
            }
            
            SwapSlot((activeSlot + i) % SlotIsActive.Count);
        }
        
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            int i = 1;
            while (!SlotIsActive[(SlotIsActive.Count + activeSlot - i) % SlotIsActive.Count])
            {
                i++;
                if(i == SlotIsActive.Count) return;
            }

            SwapSlot((SlotIsActive.Count + activeSlot - i) % SlotIsActive.Count);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(SlotIsActive[0]) SwapSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(SlotIsActive[1]) SwapSlot(1);
        }

        if (Input.GetKey(KeyCode.G))
        {
            if (forceMultiplier < forceMultiplierLimit)
                forceMultiplier += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            if (grenades > 0)
            {
                ThrowLethal();
            }

            forceMultiplier = 0f;
        }
        
        if (Input.GetKey(KeyCode.T))
        {
            if (forceMultiplier < forceMultiplierLimit)
                forceMultiplier += Time.deltaTime;
        }
        
        if (Input.GetKeyUp(KeyCode.T))
        {
            if (smokes > 0)
            {
                ThrowUtil();
            }

            forceMultiplier = 0f;
        }
    }

    
    public Weapon GetCurrentWeapon()
    {
        if (!SlotIsActive[activeSlot]) return null;
        return Slots[activeSlot].GetComponentInChildren<Weapon>();
    }

    public void PickupThrowable(Throwable throwable)
    {
        switch (throwable.type)
        {
            case Throwable.ThrowableType.Grenade:
                PickUpGrenade();
                break;
            case Throwable.ThrowableType.Smoke:
                smokes++;
                break;
        }
        Destroy(throwable.gameObject);
    }

    private void PickUpGrenade()
    {
        grenades++;
    }
    
    private void ThrowLethal()
    {
        var lethalPrefab = grenadePrefab;

        GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position,
            throwableSpawn.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();
        
        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier),ForceMode.Impulse);
        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        grenades--;
    }

    private void ThrowUtil()
    {
        var utilPrefab = smokePrefab;

        GameObject throwable = Instantiate(utilPrefab, throwableSpawn.transform.position,
            throwableSpawn.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();
        
        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier),ForceMode.Impulse);
        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        smokes--;
    }

}
