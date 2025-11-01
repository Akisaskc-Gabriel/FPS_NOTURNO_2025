using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int[] enemiesPerWave;
    public float timeBetweenWaves = 5f;
    int currentwave = 0;
    bool spawning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab,
                    spawnPoints[spawnIndex].position,
                    Quaternion.identity);
    }

    IEnumerator SpawnWaves()
    {
        while (currentwave < enemiesPerWave.Length)
        {
            spawning = true;
            for (int i = 0; i < enemiesPerWave[currentwave]; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.5f);
            }
        }
        yield return new WaitForSeconds(timeBetweenWaves);
    }

}
