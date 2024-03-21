using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    private Camera playerCamera;
    private Animator anim;
    public ParticleSystem muzzleEffect;
    
    [Header("Shooting")]
    private bool isShooting, readyToShoot;
    private bool allowReset = true;
    public float shootingDelay = 2f;
    public float spreadIntensity;
    public ShootingMode currentShootingMode;
    public int bulletsPerBurst = 3;
    private int burstBulletsLeft;
    
    
    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifetime = 3f;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto,
    }

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        playerCamera = Camera.main;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isShooting = currentShootingMode switch
        {
            ShootingMode.Auto => Input.GetKey(KeyCode.Mouse0), //can hold it down
            ShootingMode.Single or ShootingMode.Burst => Input.GetKeyDown(KeyCode.Mouse0), //only one click
            _ => isShooting
        };

        if (readyToShoot && isShooting)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        readyToShoot = false;
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        
        //instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        
        //poiting the bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;
        
        //shoot
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity,ForceMode.Impulse);
        muzzleEffect.Play();
        anim.SetTrigger("Fire");
        SoundManager.instance.pistolShootingSound.Play();
        
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
        float x = Random.Range(-spreadIntensity, spreadIntensity);
        float y = Random.Range(-spreadIntensity, spreadIntensity);
        
        return direction + new Vector3(x, y, 0f);
    }
}
