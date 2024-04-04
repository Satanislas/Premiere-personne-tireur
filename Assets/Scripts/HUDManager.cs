using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    [Header("Ammo")] 
    public TextMeshProUGUI magazine;
    public TextMeshProUGUI totalMag;
    public Image ammoType;
    public Sprite pistolAmmo;
    public Sprite M4Ammo;


    [Header("Weapon")] 
    public GameObject littleDot;
    public Image currentWeapon;
    public Sprite pistolImage;
    public Sprite M4Image;

    [Header("Throwable")] 
    public Image lethal;
    public TextMeshProUGUI lethalAmount;
    public Image tactical;
    public TextMeshProUGUI tacticalAmount;
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
        lethalAmount.text = WeaponManager.instance.grenades.ToString();
        tacticalAmount.text = WeaponManager.instance.smokes.ToString();
        
        Weapon activeWeapon = WeaponManager.instance.GetCurrentWeapon();
        if (activeWeapon is null)
        {
            magazine.text = "";
            totalMag.text = "";
            return;
        }

        magazine.text = activeWeapon.bulletsleft.ToString();
        totalMag.text = activeWeapon.totalAmmo.ToString();
        switch (activeWeapon.model)
        {
            case Weapon.WeaponModel.Pistol:
                ammoType.sprite = pistolAmmo;
                currentWeapon.sprite = pistolImage;
                break;
            case Weapon.WeaponModel.M4:
                ammoType.sprite = M4Ammo;
                currentWeapon.sprite = M4Image;
                break;
        }

        
    }
}
