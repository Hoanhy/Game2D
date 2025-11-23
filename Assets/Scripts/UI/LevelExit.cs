using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic; // Cần có để dùng List

public class LevelExit : MonoBehaviour
{
    [Header("Cài đặt UI")]
    public GameObject endGamePanel;
    public string menuSceneName = "MenuGame";

    [Header("Điều kiện mở cổng")]
    public List<string> requiredSpawnerIDs;

    private bool isGameEnded = false;

    private void Update()
    {
        if (isGameEnded)
        {
            if (Input.anyKeyDown)
            {
                LoadMenu();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isGameEnded)
        {
            // 1. KIỂM TRA ĐIỀU KIỆN
            if (CheckAllSpawnersFinished())
            {
                // Nếu đủ điều kiện -> Thắng
                isGameEnded = true;
                ShowEndGameUI();
            }
            else
            {
                // Nếu chưa đủ -> Thông báo cho người chơi
                Debug.Log("Cổng đang khóa! Cần diệt hết quái.");

            }
        }
    }

    // Hàm kiểm tra danh sách
    bool CheckAllSpawnersFinished()
    {
        // Nếu không yêu cầu gì (List trống) -> Luôn đúng
        if (requiredSpawnerIDs == null || requiredSpawnerIDs.Count == 0)
            return true;

        // Nếu SaveManager chưa sẵn sàng -> Coi như chưa xong
        if (SaveManager.Instance == null)
            return false;

        foreach (string id in requiredSpawnerIDs)
        {
            // Hỏi SaveManager
            bool isFinished = SaveManager.Instance.IsSpawnerFinished(id);

            // --- THÊM DÒNG NÀY ĐỂ SOI LỖI ---
            Debug.Log($"Kiểm tra ID: '{id}' - Trạng thái: {(isFinished ? "ĐÃ XONG" : "CHƯA XONG")}");
            // --------------------------------

            if (!isFinished)
            {
                return false; // Phát hiện 1 cái chưa xong -> Khóa cổng
            }
        }

        // Nếu duyệt hết mà không bị return false -> Đã xong tất cả
        return true;
    }

    void ShowEndGameUI()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}