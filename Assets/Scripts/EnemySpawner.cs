using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate = 2f;
    public float spawnRadius = 2f;
    private float spawnTimer;

    void Start()
    {
        spawnTimer = spawnRate;
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnRate;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    void SpawnEnemy()
    {
        Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
        Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
    }
}
