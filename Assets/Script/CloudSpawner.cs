using UnityEngine;
using System.Collections;

public class CloudSpawner : MonoBehaviour
{
    public GameObject cloudPrefab; // Assign your cloud prefab here
    public Transform spawnPoint; // Assign a point to spawn clouds from (e.g., left edge of the screen)
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 5f;
    public int maxClouds = 3;

    private int currentClouds = 0;

    void Start()
    {
        StartCoroutine(SpawnClouds());
    }

    IEnumerator SpawnClouds()
    {
        while (true)
        {
            if (currentClouds < maxClouds)
            {
                SpawnCloud();
                currentClouds++;
            }
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        }
    }

    void SpawnCloud()
    {
        Vector3 spawnPosition = spawnPoint.position;
        spawnPosition.y = Random.Range(-80f, -6f);  // Adjust based on your screen bounds
        Instantiate(cloudPrefab, spawnPosition, Quaternion.identity);
    }

    public void CloudDestroyed()
    {
        currentClouds--;
    }
}
