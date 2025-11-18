using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    [SerializeField] private GameObject pauseMenuUI;       // UI panel chính
    [SerializeField] private GameObject settingsPanelUI;   // UI panel settings


    void Start()
    {
        // Kiểm tra UI gán chưa
        if (pauseMenuUI == null)
            Debug.LogError("Pause Menu UI chưa được gán trong Inspector!");

        if (settingsPanelUI == null)
            Debug.LogWarning("Settings Panel UI chưa được gán (tùy chọn)!");

        // Đảm bảo game không bị pause khi bắt đầu
        GameIsPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    void Update()
    {
        // Nếu Túi Đồ đang mở, thì bấm Esc KHÔNG ĐƯỢC hiện Pause Menu
        if (InventoryUI.InventoryIsOpen)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                ResumeGame(); // Nếu đang pause thì bấm Esc để tiếp tục
            }
            else
            {
                PauseGame(); // Nếu đang chơi thì bấm Esc để pause
            }
        }
    }


    public void ResumeGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            if (settingsPanelUI != null) settingsPanelUI.SetActive(false);

            Time.timeScale = 1f;
            AudioListener.pause = false;
            GameIsPaused = false;

            
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
            AudioListener.pause = true;
            GameIsPaused = true;

        }
        else
        {
            Debug.LogError("Pause Menu UI chưa được gán trong Inspector!");
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
        GameIsPaused = false;
        AudioListener.pause = false;
        SceneManager.LoadScene("MenuGame");
    }
}
