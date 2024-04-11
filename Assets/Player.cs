using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int HP = 100;
    public bool isDead;
    public GameObject bloodEffect;

    public TextMeshProUGUI healthUI;
    public GameObject gameOverUI;

    private void Start()
    {
        healthUI.text = HP + " HP";
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        HP -= amount;
        healthUI.text = HP + " HP";
        if (HP <= 0)
        {
            isDead = true;
           print("Player Dead");
           SoundManager.instance.playerChannel.PlayOneShot(SoundManager.instance.playerDeath);
           PlayerDead();

        }
        else
        {
           print("Player Hit");
           SoundManager.instance.playerChannel.PlayOneShot(SoundManager.instance.playerHit);
           StartCoroutine(BloodyScreenEffect());
        }
    }

    private void PlayerDead()
    {
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<MouseMovement>().enabled = false;

        GetComponentInChildren<Animator>().enabled = true;
        healthUI.enabled = false;
        GetComponent<ScreenFader>().StartFade();
        StartCoroutine(ShowGameOverUI());
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(4f);
        gameOverUI.SetActive(true);
    }

    private IEnumerator BloodyScreenEffect()
    {
        if (bloodEffect.activeInHierarchy == false)
        {
            bloodEffect.SetActive(true);
        }

        var image = bloodEffect.GetComponentInChildren<Image>();
 
        // Set the initial alpha value to 1 (fully visible).
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;
 
        float duration = 3f;
        float elapsedTime = 0f;
 
        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp.
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
 
            // Update the color with the new alpha value.
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;
 
            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;
 
            yield return null; ; // Wait for the next frame.
        }
        
        if (bloodEffect.activeInHierarchy)
        {
                bloodEffect.SetActive(false);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            TakeDamage(other.gameObject.GetComponent<ZombieHand>().Damage);
        }
    }
}
