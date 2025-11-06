using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;       // UI panel chính
    [SerializeField] private GameObject settingsPanelUI;   // UI panel settings

    private bool isPaused = false;

    void Start()
    {
        // Kiểm tra UI gán chưa
        if (pauseMenuUI == null)
            Debug.LogError("Pause Menu UI chưa được gán trong Inspector!");

        if (settingsPanelUI == null)
            Debug.LogWarning("Settings Panel UI chưa được gán (tùy chọn)!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            // Nếu đã pause rồi thì Esc không làm gì cả
            // Để người chơi chỉ dùng nút Resume hoặc CloseSettings
        }
    }


    public void ResumeGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            if (settingsPanelUI != null) settingsPanelUI.SetActive(false);

            Time.timeScale = 1f;
            isPaused = false;

            
        }
        else
        {
            Debug.LogWarning("Pause Menu UI chưa được gán trong Inspector!");
        }
    }

    public void PauseGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;

        }
        else
        {
            Debug.LogError("Pause Menu UI chưa được gán trong Inspector!");
        }
    }

    public void OpenSettings()
    {
        if (settingsPanelUI != null)
        {
            settingsPanelUI.SetActive(true);
            if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Bạn chưa gán Settings Panel UI!");
        }
    }

    public void CloseSettings()
    {
        if (settingsPanelUI != null)
        {
            settingsPanelUI.SetActive(false);
            if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Settings Panel UI chưa được gán!");
        }
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f; // Reset lại game time
        SceneManager.LoadScene("MainMenu");
    }
}
