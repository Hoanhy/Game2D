using UnityEngine;
using System.Collections;
using System.IO;                 // Dùng để Đọc/Ghi file
using System.Text;               // Dùng cho Mã hóa
using System;                    // Dùng cho Mã hóa
using UnityEngine.SceneManagement; // Dùng để phát hiện khi tải màn

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance; // Singleton
    public bool HasLoadedData = false; // Đánh dấu xem đã load file chưa

    // Các tham chiếu này sẽ được gán tự động
    private Player player;
    private PlayerHealth playerHealth;

    private GameProgress currentGameData;
    private string saveFilePath;
    private string saveFileName = "progress.json"; // Tên file save


    void Awake()
    {
        // 1. Thiết lập Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại khi qua màn
        }
        else
        {
            Destroy(gameObject); // Hủy bản sao
            return;
        }

        // 2. Xác định đường dẫn lưu file
        saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);
        
    }

    // 3. Đăng ký lắng nghe sự kiện
    private void OnEnable()
    {
        // Bảo Unity: "Khi nào tải màn xong, hãy gọi hàm OnSceneLoaded"
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Hủy đăng ký khi bị hủy
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 4. TỰ ĐỘNG TÌM PLAYER
    // Hàm này tự chạy MỖI KHI tải màn mới
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Tự động tìm Player trong màn mới
        player = FindFirstObjectByType<Player>();
        playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (player != null)
        {
            Debug.Log("SaveManager đã tìm thấy Player! Đang chờ 1 frame để tải dữ liệu...");
            // SỬA: Gọi Coroutine thay vì gọi trực tiếp
            StartCoroutine(ApplyDataAfterDelay());
        }
        else
        {
            Debug.Log("SaveManager: Không tìm thấy Player/PlayerHealth (Có thể đây là Main Menu).");
        }
    }
    private IEnumerator ApplyDataAfterDelay()
    {
        // Chờ đến cuối frame (sau khi PlayerHealth.Start() đã chạy
        // và sau khi Unity áp dụng giá trị Inspector)
        yield return new WaitForEndOfFrame();

        // Bây giờ, chúng ta gọi hàm LoadGameData
        // Code này sẽ là code chạy cuối cùng, ghi đè lên HP=6
        LoadGameDataIntoScene();
    }

    // ----------------------------------------------------
    // CÁC HÀM CÔNG KHAI (PUBLIC)
    // ----------------------------------------------------

    public void StartNewGame()
    {
        // Tạo dữ liệu mới tinh
        currentGameData = new GameProgress();
        HasLoadedData = false; // Đây là game mới
        if (player != null)
        {
            currentGameData.lastPlayerPosition = new SerializableVector2(player.transform.position);
        }
    }
    public void SaveGame(bool updatePlayerPos = true)
    {
        // Kiểm tra xem có Player không (phòng khi ở Menu)
        if (player == null || playerHealth == null)
        {
            Debug.LogWarning("Không tìm thấy Player, không thể lưu!");
            return;
        }

        // 1. CẬP NHẬT DỮ LIỆU
        // Lấy dữ liệu mới nhất từ game vào đối tượng
        currentGameData.currentHealth = playerHealth.currentHP; // (Bạn cần đảm bảo biến currentHP là public trong PlayerHealth)
        if (updatePlayerPos)
        {
            currentGameData.lastPlayerPosition = new SerializableVector2(player.transform.position);
        }
        currentGameData.enemies.Clear(); // Xóa danh sách cũ để tránh trùng lặp

        // Tìm tất cả các con quái có gắn script EnemyState
        EnemyState[] allEnemies = FindObjectsByType<EnemyState>(FindObjectsSortMode.None);

        foreach (EnemyState enemy in allEnemies)
        {
            // Lấy dữ liệu từ từng con và nhét vào danh sách lưu trữ
            currentGameData.enemies.Add(enemy.GetData());
        }
        if (InventoryManager.Instance != null)
        {
            currentGameData.inventory = InventoryManager.Instance.GetInventoryData();
        }

        // 2. CHUYỂN SANG JSON
        string json = JsonUtility.ToJson(currentGameData, true);

        // 3. MÃ HÓA (Base64)
        string base64Json = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

        // 4. GHI FILE
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
            // 1. ĐỌC FILE
            string base64Json = File.ReadAllText(saveFilePath);

            // 2. GIẢI MÃ
            byte[] bytes = Convert.FromBase64String(base64Json);
            string json = Encoding.UTF8.GetString(bytes);

            // 3. CHUYỂN SANG ĐỐI TƯỢNG
            currentGameData = JsonUtility.FromJson<GameProgress>(json);
            HasLoadedData = true; // Đánh dấu là đã load dữ liệu

            // 4. ÁP DỤNG
            // (Hàm này sẽ chờ OnSceneLoaded tìm Player rồi mới chạy)

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
            // Lưu ý: Ta chưa cần SaveGame ngay, đợi chạm Checkpoint lưu cũng được
            // Hoặc nếu muốn chắc ăn thì gọi SaveGame(false) tại đây.
        }
    }
    // HÀM NỘI BỘ: Áp dụng dữ liệu vào game
    private void LoadGameDataIntoScene()
    {
        // Chỉ áp dụng nếu 3 thứ này đều tồn tại
        if (player != null && playerHealth != null && currentGameData != null)
        {
            // 1. Load Player
            playerHealth.currentHP = currentGameData.currentHealth;
            player.transform.position = currentGameData.lastPlayerPosition.ToVector2();
            if (playerHealth.healthUI != null)
                playerHealth.healthUI.UpdateHeartUI(playerHealth.currentHP, playerHealth.maxHP);

            // --- 2. LOAD ENEMY (MỚI) ---
            // Tìm tất cả quái vừa được sinh ra trong màn chơi mới
            EnemyState[] allEnemiesInScene = FindObjectsByType<EnemyState>(FindObjectsSortMode.None);

            // Lặp qua từng con quái thực tế trong game
            foreach (EnemyState enemyScript in allEnemiesInScene)
            {
                // A. KIỂM TRA XEM NÓ CÓ TRONG "SỔ TỬ" KHÔNG?
                if (currentGameData.deadEnemyIDs.Contains(enemyScript.enemyID))
                {
                    // Nếu có ID trong danh sách chết -> Tắt ngay lập tức
                    enemyScript.gameObject.SetActive(false);
                    continue; // Bỏ qua, không cần load máu me gì nữa
                }

                // B. Nếu còn sống, load máu và vị trí (Code cũ)
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
    // 1. Kiểm tra xem Spawner này đã xong chưa
    public bool IsSpawnerFinished(string spawnerID)
    {
        if (currentGameData != null)
        {
            return currentGameData.finishedSpawnerIDs.Contains(spawnerID);
        }
        return false;
    }

    // 2. Đánh dấu Spawner là đã xong và Lưu game
    public void MarkSpawnerFinished(string spawnerID)
    {
        if (currentGameData != null && !currentGameData.finishedSpawnerIDs.Contains(spawnerID))
        {
            currentGameData.finishedSpawnerIDs.Add(spawnerID);

        }
    }
    // 1. Kiểm tra xem món đồ này đã bị nhặt chưa?
    public bool IsItemCollected(string itemID)
    {
        if (currentGameData != null)
        {
            return currentGameData.collectedItemIDs.Contains(itemID);
        }
        return false;
    }

    // 2. Đánh dấu món đồ là đã nhặt
    public void MarkItemCollected(string itemID)
    {
        if (currentGameData != null && !currentGameData.collectedItemIDs.Contains(itemID))
        {
            currentGameData.collectedItemIDs.Add(itemID);

            // Quan trọng: Lưu game ngay (nhưng KHÔNG lưu vị trí, chỉ lưu trạng thái đồ)
            SaveGame(false);
        }
    }
    public bool CheckForSaveFile()
    {
        return File.Exists(saveFilePath);
    }
}