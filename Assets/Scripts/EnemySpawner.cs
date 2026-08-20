using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab; // Prefab quái
    [SerializeField] private float spawnRate = 1.5f;   // Tốc độ sinh quái (1.5s / 1 con)

    [Header("Spawn Area Boundaries")]
    [SerializeField] private float minX = -8f; // Mép trái Map
    [SerializeField] private float maxX = 8f;  // Mép phải Map
    [SerializeField] private float minY = -4f; // Mép dưới Map
    [SerializeField] private float maxY = 4f;  // Mép trên Map

    private float nextSpawnTime = 0f;

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        // Tọa độ ngẫu nhiên trong khoảng Map
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

        // Sinh quái ra tại tọa độ ngẫu nhiên
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}