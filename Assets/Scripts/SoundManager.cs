using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    
    public AudioSource EmptyMagazine;
    public AudioSource ShootingChannel;

    [Header("pistol")] 
    public AudioClip pistolShootingClip;
    public AudioSource pistolReloadSound;
    
    
     [Header("M4")]
    public AudioSource M4ReloadSound;
    public AudioClip M4ShootingClip;

    [Header("Throwables")] public AudioSource throwableChannel;
    public AudioClip grenadeExplosion;

    [Header("Zombie")] public AudioSource ZombieChannel;
    public AudioSource ZombieChannel2;
    public AudioClip ZWalking;
    public AudioClip ZChase;
    public AudioClip ZAttack;
    public AudioClip ZHurt;
    public AudioClip ZDeath;

    [Header("Player")] public AudioSource playerChannel;
    public AudioClip playerHit;
    public AudioClip playerDeath;
    
    
    
    
    
    private void Awake()
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

    public void PlayShootingSound(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol:
                ShootingChannel.PlayOneShot(pistolShootingClip);
                break;
            case Weapon.WeaponModel.M4:
                ShootingChannel.PlayOneShot(M4ShootingClip);
                break;
        }
    }

    public void playReloadSound(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol:
                pistolReloadSound.Play();
                break;
            case Weapon.WeaponModel.M4:
                M4ReloadSound.Play();
                break;
        }
    }
}
