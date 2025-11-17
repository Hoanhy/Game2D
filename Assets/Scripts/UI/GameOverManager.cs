using UnityEngine;
using UnityEngine.SceneManagement; // Cần để chuyển cảnh

public class GameOverManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel; // Kéo cái Panel "You Died" vào đây

    // Hàm này sẽ được gọi khi Player chết
    public void TriggerGameOver()
    {
        // 1. Nếu biến đang rỗng, đi tìm nó
        if (gameOverPanel == null)
        {
            // Tìm đối tượng CHA (đang BẬT) có tag là "GameOverUI"
            GameObject parentObject = GameObject.FindGameObjectWithTag("GameOverUI");

            if (parentObject != null)
            {
                // Tìm đứa CON tên là "GameOverPanel" bên trong nó (kể cả khi nó đang TẮT)
                Transform childPanel = parentObject.transform.Find("GameOverPanel");

                if (childPanel != null)
                {
                    gameOverPanel = childPanel.gameObject;
                }
                else
                {
                    Debug.LogError("Tìm thấy Tag GameOverUI nhưng không thấy con tên là 'GameOverPanel'!");
                }
            }
            else
            {
                Debug.LogError("Lỗi: Không tìm thấy đối tượng nào có Tag 'GameOverUI' đang bật!");
                return;
            }
        }

        // 2. Bật nó lên
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Hàm này gán cho nút "Main Menu"
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Trả lại thời gian bình thường trước khi chuyển cảnh
        SceneManager.LoadScene("MenuGame"); // Đảm bảo tên Scene Menu của bạn đúng y hệt
    }

    // (Tùy chọn) Hàm chơi lại màn chơi hiện tại
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}