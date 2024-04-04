using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    private Camera playerCamera;
    private Animator anim;
    public WeaponModel model;
    
    
   
    [Header("Shooting")]
    public float shootingDelay = 2f;
    public float spreadIntensity;
    public float ADSSpreadIntensity;
    public ShootingMode currentShootingMode;
    public int bulletsPerBurst = 3;
    private int burstBulletsLeft;
    private bool isShooting, readyToShoot;
    private bool allowReset = true;
    private bool ADS;
    
    
    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifetime = 3f;
    public ParticleSystem muzzleEffect;
    
    [Header("Reload")]
    public float reloadTime;
    public int magazineSize;
    internal int bulletsleft;
    internal bool isReloading;
    public int totalAmmo;
    public int maxAmmoAmmount;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto,
    }
    public enum WeaponModel
    {
        Pistol,
        M4,
    }

    private void Awake()
    {
        bulletsleft = magazineSize;
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        playerCamera = Camera.main;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletsleft == 0 && isShooting)
        {
            SoundManager.instance.EmptyMagazine.Play();
        }
        
        isShooting = currentShootingMode switch
        {
            ShootingMode.Auto => Input.GetKey(KeyCode.Mouse0), //can hold it down
            ShootingMode.Single or ShootingMode.Burst => Input.GetKeyDown(KeyCode.Mouse0), //only one click
            _ => isShooting
        };

        if (readyToShoot && isShooting && !isReloading && bulletsleft > 0)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }

        //manual reload
        if (Input.GetKeyDown(KeyCode.R) && bulletsleft < magazineSize && !isReloading && totalAmmo > 0)
        {
            Reload();
        }

        //automatic reload
        if (readyToShoot && !isShooting && !isReloading && bulletsleft <= 0)
        {
            //Reload();
        }
        
        //ADS
        if (Input.GetMouseButton(1))
        {
            anim.SetBool("ADS",true);
            ADS = true;
            HUDManager.instance.littleDot.SetActive(false);
        }
        else
        {
            anim.SetBool("ADS",false);
            ADS = false;
            HUDManager.instance.littleDot.SetActive(true);
        }
    }

    private void FireWeapon()
    {
        bulletsleft -= bulletsleft > 0 ? 1 : 0;
        readyToShoot = false;
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        
        //instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        
        //poiting the bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;
        
        //shoot
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity,ForceMode.Impulse);
        muzzleEffect.Play();
        if (ADS)
        {
            anim.SetTrigger("ADSFire");
        }
        else
        {
            anim.SetTrigger("Fire");
        }
        SoundManager.instance.PlayShootingSound(model);
        
        //destroy after some time
        Destroy(bullet,bulletPrefabLifetime);

        //check if done shooting
        if (allowReset)
        {
            Invoke(nameof(ResetShot),shootingDelay);
            allowReset = false;
        }
        
        //burst mode
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke(nameof(FireWeapon),shootingDelay);
        }
    }

    private int ammoToReload;
    private void Reload()
    {
        isReloading = true;
        totalAmmo -= magazineSize - bulletsleft;
        ammoToReload = magazineSize;
        if (totalAmmo < 0)
        {
            ammoToReload = magazineSize+totalAmmo;
            totalAmmo = 0;
        }
        SoundManager.instance.playReloadSound(model);
        anim.SetTrigger("Reload");
        Invoke("RealoadCompleted",reloadTime);
    }

    private void RealoadCompleted()
    {
        bulletsleft = ammoToReload;
        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        //ray from the middle of the screen to see where the player is aiming at
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        Vector3 targetpoint;

        if (Physics.Raycast(ray, out hit))
        {
            //hitting  something
            targetpoint = hit.point;
        }
        else
        {
            //shooting in the air
            targetpoint = ray.GetPoint(100);
        }
        
        //randomness
        Vector3 direction = targetpoint - bulletSpawn.position;
        float z = Random.Range(-(ADS ? ADSSpreadIntensity : spreadIntensity), ADS ? ADSSpreadIntensity : spreadIntensity);
        float y = Random.Range(-(ADS ? ADSSpreadIntensity : spreadIntensity), ADS ? ADSSpreadIntensity : spreadIntensity);
        
        return direction + new Vector3(0f, y, z);
    }
}
