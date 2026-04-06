using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public bool HasLoadedData = false;

    private Player player;
    private PlayerHealth playerHealth;

    private GameProgress currentGameData;
    private string saveFilePath;
    private string saveFileName = "progress.json";


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = FindFirstObjectByType<Player>();
        playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (player != null)
        {
            Debug.Log("SaveManager đã tìm thấy Player! Đang chờ 1 frame để tải dữ liệu...");
            StartCoroutine(ApplyDataAfterDelay());
        }
        else
        {
            Debug.Log("SaveManager: Không tìm thấy Player/PlayerHealth.");
        }
    }

    private IEnumerator ApplyDataAfterDelay()
    {
        yield return new WaitForEndOfFrame();
        LoadGameDataIntoScene();
    }

    // =========================
    // NEW GAME (ĐÃ THÊM XÓA JSON)
    // =========================
    public void StartNewGame()
    {
        // Xóa file save cũ nếu tồn tại
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Đã xóa file save cũ!");
        }

        // Tạo dữ liệu mới
        currentGameData = new GameProgress();
        HasLoadedData = false;

        if (player != null)
        {
            currentGameData.lastPlayerPosition = new SerializableVector2(player.transform.position);
        }

        Debug.Log("Bắt đầu game mới!");
    }

    public void SaveGame(bool updatePlayerPos = true)
    {
        if (player == null || playerHealth == null)
        {
            Debug.LogWarning("Không tìm thấy Player, không thể lưu!");
            return;
        }

        currentGameData.currentHealth = playerHealth.currentHP;

        if (updatePlayerPos)
        {
            currentGameData.lastPlayerPosition = new SerializableVector2(player.transform.position);
        }

        currentGameData.enemies.Clear();

        EnemyState[] allEnemies = FindObjectsByType<EnemyState>(FindObjectsSortMode.None);

        foreach (EnemyState enemy in allEnemies)
        {
            currentGameData.enemies.Add(enemy.GetData());
        }

        if (InventoryManager.Instance != null)
        {
            currentGameData.inventory = InventoryManager.Instance.GetInventoryData();
        }

        string json = JsonUtility.ToJson(currentGameData, true);
        string base64Json = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

        File.WriteAllText(saveFilePath, base64Json);

        Debug.Log("Game đã được lưu tại: " + saveFilePath);
    }

    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            StartNewGame();
            return;
        }

        try
        {
            string base64Json = File.ReadAllText(saveFilePath);
            byte[] bytes = Convert.FromBase64String(base64Json);
            string json = Encoding.UTF8.GetString(bytes);

            currentGameData = JsonUtility.FromJson<GameProgress>(json);
            HasLoadedData = true;
        }
        catch (Exception e)
        {
            Debug.LogError("File save bị hỏng! Lỗi: " + e.Message);
            StartNewGame();
        }
    }

    public void AddDeadEnemy(string enemyID)
    {
        if (currentGameData != null && !currentGameData.deadEnemyIDs.Contains(enemyID))
        {
            currentGameData.deadEnemyIDs.Add(enemyID);
        }
    }

    private void LoadGameDataIntoScene()
    {
        if (player != null && playerHealth != null && currentGameData != null)
        {
            playerHealth.currentHP = currentGameData.currentHealth;
            player.transform.position = currentGameData.lastPlayerPosition.ToVector2();

            if (playerHealth.healthUI != null)
                playerHealth.healthUI.UpdateHeartUI(playerHealth.currentHP, playerHealth.maxHP);

            EnemyState[] allEnemiesInScene = FindObjectsByType<EnemyState>(FindObjectsSortMode.None);

            foreach (EnemyState enemyScript in allEnemiesInScene)
            {
                if (currentGameData.deadEnemyIDs.Contains(enemyScript.enemyID))
                {
                    enemyScript.gameObject.SetActive(false);
                    continue;
                }

                EnemyData savedData = currentGameData.enemies.Find(x => x.enemyID == enemyScript.enemyID);
                if (savedData != null)
                {
                    enemyScript.LoadData(savedData);
                }
            }

            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.LoadInventory(currentGameData.inventory);
            }

            Debug.Log("Dữ liệu game đã được tải!");
        }
    }

    public bool IsSpawnerFinished(string spawnerID)
    {
        if (currentGameData != null)
        {
            return currentGameData.finishedSpawnerIDs.Contains(spawnerID);
        }
        return false;
    }

    public void MarkSpawnerFinished(string spawnerID)
    {
        if (currentGameData != null && !currentGameData.finishedSpawnerIDs.Contains(spawnerID))
        {
            currentGameData.finishedSpawnerIDs.Add(spawnerID);
        }
    }

    public bool IsItemCollected(string itemID)
    {
        if (currentGameData != null)
        {
            return currentGameData.collectedItemIDs.Contains(itemID);
        }
        return false;
    }

    public void MarkItemCollected(string itemID)
    {
        if (currentGameData != null && !currentGameData.collectedItemIDs.Contains(itemID))
        {
            currentGameData.collectedItemIDs.Add(itemID);
            SaveGame(false);
        }
    }

    public bool CheckForSaveFile()
    {
        return File.Exists(saveFilePath);
    }
}