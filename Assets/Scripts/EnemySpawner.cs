using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Thiết lập kẻ địch")]
    public GameObject enemyPrefab;

    [Header("Khu vực spawn (hình chữ nhật)")]
    public Vector2 spawnAreaSize = new Vector2(6f, 4f);

    [Header("Thiết lập wave")]
    public int enemiesPerWave = 5;        // số lượng enemy mỗi đợt
    public float timeBetweenWaves = 5f;   // thời gian nghỉ giữa các đợt
    public float timeBetweenSpawns = 1f;  // thời gian giữa từng enemy trong 1 đợt
    public int maxWaves = 3;              // số lượng đợt tối đa

    private bool playerInRange = false;
    private bool isSpawning = false;
    private int currentWave = 0;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaSize.x, spawnAreaSize.y, 0f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isSpawning && currentWave < maxWaves)
        {
            playerInRange = true;
            StartCoroutine(SpawnWaves());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    IEnumerator SpawnWaves()
    {
        isSpawning = true;

        while (playerInRange && currentWave < maxWaves)
        {
            currentWave++;
            Debug.Log($"Wave {currentWave}/{maxWaves} bắt đầu!");

            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            Debug.Log($"Wave {currentWave} kết thúc. Nghỉ {timeBetweenWaves} giây...");
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        Debug.Log("Đã spawn đủ số wave! Ngừng spawn.");
        isSpawning = false;
    }

    void SpawnEnemy()
    {
        Vector2 randomOffset = new Vector2(
            Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f)
        );
        Vector2 spawnPos = (Vector2)transform.position + randomOffset;

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
