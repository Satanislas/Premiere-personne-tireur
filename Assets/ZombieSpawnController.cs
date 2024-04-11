using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class ZombieSpawnController : MonoBehaviour
{
    public int initialZperWave;
    public int currentZperWave;

    public float spawnDelay = 0.5f;
    public static int currentWave = 0;
    public float WaveCD = 5f;

    public bool inCooldown;
    public float cooldownCounter = 0;

    public  List<Enemy> currentZombiesAlive;
    [SerializeField] private Object zombiePrefab;

    public TextMeshProUGUI waveText;

    private void Start()
    {
        currentZperWave = initialZperWave;
        currentZombiesAlive = new List<Enemy>();
        currentWave = 0;
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombiesAlive.Clear();

        currentWave ++;
        waveText.text = "Wave : " + currentWave;
        if (currentWave > SaveLoadManager.instance.LoadHighScore())
        {
            SaveLoadManager.instance.SaveHighScore(currentWave);
        }
        
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZperWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPos = transform.position + spawnOffset;

            var Z = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);

            Enemy enemy = Z.GetComponent<Enemy>();
            
            currentZombiesAlive.Add(enemy);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        List<Enemy> zombiesToRemove = new List<Enemy>();
        foreach (var z in currentZombiesAlive)
        {
            if (z.isDead)
            {
                zombiesToRemove.Add(z);
            }
        }

        foreach (var z in zombiesToRemove)
        {
            currentZombiesAlive.Remove(z);
        }
        
        zombiesToRemove.Clear();
        if (currentZombiesAlive.Count == 0 && inCooldown == false)
        {

            inCooldown = true;
            StartCoroutine(WaveCooldown());
        }

        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            cooldownCounter = WaveCD;
        }
    }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;
        yield return new WaitForSeconds(WaveCD);

        inCooldown = false;
        currentZperWave *= 2;
        StartNextWave();
    }
}
