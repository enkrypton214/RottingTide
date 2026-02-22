using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieSpawnController : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int initialZombiesPerWave = 5;
    public int currentZombiesPerWave;

    public float spawnDelay = 0.5f;

    public int currentWave = 0;
    public float waveCooldown = 10f;

    private bool inCooldown;
    public float cooldownCounter=0;

    public List<Enemy> currentZombiesAlive;

    public TextMeshProUGUI WaveOverUI;
    public TextMeshProUGUI cooldownCounterUI;
    public TextMeshProUGUI currentWaveUI;

    public List<GameObject> spawnPoints;
    private void Start()
    {
        currentZombiesPerWave = initialZombiesPerWave;
        StartNextWave();
    }



    private void StartNextWave()
    {
        currentZombiesAlive.Clear();
        currentWave++;
        currentWaveUI.text = "Wave "+ currentWave;
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i =0; i<currentZombiesPerWave;i++)
        {
            int randomIndex = Random.Range(0,spawnPoints.Count);
            Transform spawnPoint = spawnPoints[randomIndex].transform;

            UnityEngine.Vector3 spawnOffset = new UnityEngine.Vector3(Random.Range(-1f,1f),0,Random.Range(-1f,1f));
            UnityEngine.Vector3 spawnPosition = spawnPoint.position + spawnOffset;

            
            var zombie = Instantiate(zombiePrefab,spawnPosition,Quaternion.identity);
            Enemy enemyScript = zombie.GetComponent<Enemy>();

            currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);    
        }

    }

    private void Update()
    {
        List<Enemy> zombieToRemove =  new List<Enemy>();
        foreach (Enemy zombie in currentZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombieToRemove.Add(zombie);
            }
        }

        foreach (Enemy zombie in zombieToRemove)
        {
           currentZombiesAlive.Remove(zombie);
        }
        zombieToRemove.Clear();

        if (currentZombiesAlive.Count==0 && inCooldown == false)
        {
            StartCoroutine(WaveCooldown());
        }

        if (inCooldown)
        {
            cooldownCounter-= Time.deltaTime;
        }
        else
        {
            cooldownCounter= waveCooldown;
        }
        cooldownCounterUI.text=cooldownCounter.ToString("F0");
    }

    private IEnumerator WaveCooldown()
    {
        inCooldown= true;
        WaveOverUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(waveCooldown);
        inCooldown = false;
        WaveOverUI.gameObject.SetActive(false);

        currentZombiesPerWave+=2;
        StartNextWave();
    }
}

