using UnityEngine;
using UnityEngine.UI; // Cần thiết để điều khiển Nút (Button)
using UnityEngine.SceneManagement; // Cần thiết để tải màn

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Buttons")]
    public GameObject continueButton; // Kéo nút "Tiếp tục" vào đây
    public GameObject newGameButton;    // Kéo nút "Chơi mới" vào đây

    [Header("Scene Settings")]
    public string gameplaySceneName = "Game"; // Tên màn chơi của bạn

    void Start()
    {
        // 1. Kiểm tra xem file save có tồn tại không
        // Chúng ta gọi hàm vừa tạo trong SaveManager.
        // Phải đảm bảo GameManager (chứa SaveManager) đã có trong Scene này.
        bool saveFileExists = SaveManager.Instance.CheckForSaveFile();

        // 2. Ẩn/Hiện nút dựa trên yêu cầu của bạn
        // (Nếu có save: chỉ hiện "Continue")
        // (Nếu không có save: chỉ hiện "New Game")

        // CHỈ ẨN/HIỆN NÚT CONTINUE
        continueButton.SetActive(saveFileExists);

        // NÚT NEW GAME LUÔN LUÔN HIỆN
        newGameButton.SetActive(true);
    }

    // 3. Hàm để gán cho sự kiện OnClick() của nút "Continue"
    public void OnContinueClicked()
    {
        // Tải dữ liệu trước, sau đó tải màn chơi
        // (SaveManager sẽ tự động áp dụng dữ liệu khi màn chơi được tải xong)
        SaveManager.Instance.LoadGame();
        SceneManager.LoadScene(gameplaySceneName);
    }

    // 4. Hàm để gán cho sự kiện OnClick() của nút "New Game"
    public void OnNewGameClicked()
    {
        // Tạo file save mới, sau đó tải màn chơi
        SaveManager.Instance.StartNewGame();
        SceneManager.LoadScene(gameplaySceneName);
    }
    public void OnQuitClicked()
    {
        Debug.Log("Đang thoát game...");

        // Hàm Application.Quit() sẽ đóng ứng dụng
        Application.Quit();
    }
}