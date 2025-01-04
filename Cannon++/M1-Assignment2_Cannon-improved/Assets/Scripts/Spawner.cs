using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab; // The enemy prefab to spawn
    [SerializeField] private float minSpawnInterval = 1f; // Minimum time between spawns
    [SerializeField] private float maxSpawnInterval = 5f; // Maximum time between spawns
    [SerializeField] private int maxEnemies = 10; // Maximum number of enemies in the scene
    [SerializeField] private Vector3 spawnArea = new Vector3(10f, 0f, 10f); // Size of spawn area

    private int currentEnemyCount = 0;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
                currentEnemyCount++;
            }

            // Wait for a random time between the min and max spawn intervals
            float spawnDelay = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnEnemy()
    {
        // Random position within the spawn area
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
            spawnArea.y, // Keep y consistent for ground-level spawning
            Random.Range(-spawnArea.z / 2, spawnArea.z / 2)
        );

        // Spawn the enemy
        Instantiate(enemyPrefab, transform.position + randomPosition, Quaternion.identity);
    }

    public void DecreaseEnemyCount()
    {
        if (currentEnemyCount > 0)
        {
            currentEnemyCount--;
        }
    }

    // Setters for minSpawnInterval, maxSpawnInterval, and maxEnemies
    public void SetMinSpawnInterval(float value)
    {
        minSpawnInterval = Mathf.Max(0f, value); // Ensure the value is non-negative
    }

    public void SetMaxSpawnInterval(float value)
    {
        maxSpawnInterval = Mathf.Max(minSpawnInterval, value); // Ensure max interval is >= min interval
    }

    public void SetMaxEnemies(int value)
    {
        maxEnemies = Mathf.Max(0, value); // Ensure the number of enemies is non-negative
    }
}
