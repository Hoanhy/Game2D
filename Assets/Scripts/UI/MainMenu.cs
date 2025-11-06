using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // [Gán trong Inspector] Kéo Panel Main Menu (chứa các nút Play, Exit) vào đây.
    [SerializeField] private GameObject mainMenuPanelUI;

    void Awake()
    {
        //Tạm dừng thời gian game trước khi bất kỳ script Start() nào chạy
        Time.timeScale = 0f;
    }

    void Start()
    {
        // Đảm bảo Main Menu được hiển thị khi bắt đầu game
        if (mainMenuPanelUI != null)
        {
            mainMenuPanelUI.SetActive(true);
        }
    }

    public void PlayGame()
    {
        // 1. BẬT LẠI thời gian game khi nút Play được nhấn
        Time.timeScale = 1f;

        // 2. Ẩn Main Menu
        if (mainMenuPanelUI != null)
        {
            mainMenuPanelUI.SetActive(false);
        }
    }

    public void ExitGame()
    {
        Debug.Log("Quit game!");
        Application.Quit();
    }
}