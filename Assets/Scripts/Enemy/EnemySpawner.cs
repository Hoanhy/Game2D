using System;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Định danh (BẮT BUỘC PHẢI KHÁC NHAU)")]
    public string spawnerID;

    [Header("Thiết lập kẻ địch")]
    public GameObject enemyPrefab;

    [Header("Khu vực spawn (hình chữ nhật)")]
    public Vector2 spawnAreaSize = new Vector2(6f, 4f);

    [Header("Thiết lập wave")]
    public int enemiesPerWave = 5;
    public float timeBetweenWaves = 5f;
    public float timeBetweenSpawns = 1f;
    public int maxWaves = 3;

    [Header("Tham chiếu SpawnTrigger để tắt/bật tường")]
    public SpawnTrigger spawnTrigger;

    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool isSpawning = false;
    private bool isFinished = false;
    private void Start()
    {
        // Nếu chưa đặt tên ID, báo lỗi ngay
        if (string.IsNullOrEmpty(spawnerID))
        {
            Debug.LogError("Spawner này chưa có ID! Hãy đặt tên trong Inspector.");
            return;
        }

        if (SaveManager.Instance != null && SaveManager.Instance.IsSpawnerFinished(spawnerID))
        {
            Debug.Log($"Spawner {spawnerID} đã hoàn thành trước đó. Tự hủy.");

            // Đảm bảo tường được mở
            if (spawnTrigger != null) spawnTrigger.ClearWall();

            // Tắt script này hoặc hủy GameObject để không spawn nữa
            Destroy(gameObject);
            return;
        }
    }
    // --- Hiển thị vùng spawn ---
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaSize.x, spawnAreaSize.y, 0f));
    }

    // --- Trigger khi Player bước vào ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isSpawning && !isFinished)
        {
            Debug.Log("Player entered spawn area");

            if (spawnTrigger != null && spawnTrigger.wallGroup != null)
                spawnTrigger.wallGroup.SetActive(true);

            StartSpawn();
        }
    }

    public void StartSpawn()
    {
        if (!isSpawning)
            StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        isSpawning = true;

        while (currentWave < maxWaves)
        {
            currentWave++;
            enemiesAlive = enemiesPerWave;
            Debug.Log($"Wave {currentWave}/{maxWaves} bắt đầu!");

            // Spawn enemy trong wave
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            // Chờ hết enemy trong wave trước khi kết thúc wave
            while (enemiesAlive > 0)
                yield return null;

            Debug.Log($"Wave {currentWave} kết thúc. Nghỉ {timeBetweenWaves} giây...");
            if (currentWave < maxWaves)
                yield return new WaitForSeconds(timeBetweenWaves);
        }

        Debug.Log("Đã spawn đủ số wave!");
        isSpawning = false;
        isFinished = true;

        // Tắt tường sau wave cuối cùng
        if (spawnTrigger != null)
            spawnTrigger.ClearWall();
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.MarkSpawnerFinished(spawnerID);
        }
    }

    void SpawnEnemy()
    {
        Vector2 randomOffset = new Vector2(
            UnityEngine.Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            UnityEngine.Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f)
        );
        Vector2 spawnPos = (Vector2)transform.position + randomOffset;

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        // (Để tránh lỗi trùng ID khi save game giữa chừng)
        EnemyState enemyState = enemy.GetComponent<EnemyState>();
        if (enemyState != null)
        {
            // Đặt ID dựa trên Spawner ID + Wave + Số thứ tự
            // Ví dụ: "Spawner1_Wave2_Enemy5"
            enemyState.enemyID = $"{spawnerID}_W{currentWave}_E{enemiesAlive}";
        }

        EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
        if (eh != null)
            eh.spawner = this; // gán spawner để báo EnemyDied
    }

    // Enemy gọi khi chết
    public void EnemyDied()
    {
        enemiesAlive--;
        Debug.Log($"EnemyDied! Còn lại {enemiesAlive} enemies.");
    }
}
