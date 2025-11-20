using UnityEngine;
using TMPro; // Cần để chỉnh màu chữ
using UnityEngine.SceneManagement; // Cần để chuyển cảnh

public class DemoEndController : MonoBehaviour
{
    [Header("Cài đặt")]
    public TextMeshProUGUI blinkText; // Kéo cái chữ "Nhấn bất kỳ..." vào đây
    public string menuSceneName = "MenuGame"; // Tên Scene Menu của bạn
    public float blinkSpeed = 2f; // Tốc độ nhấp nháy

    // Biến để tránh việc vừa bật lên đã bị tắt ngay do click chuột thừa
    private float delayTimer = 0f;

    void OnEnable()
    {
        // Khi Panel được bật lên, reset timer
        delayTimer = 0f;

        // Dừng thời gian game lại (nếu chưa dừng)
        Time.timeScale = 0f;
    }

    void Update()
    {
        // 1. HIỆU ỨNG NHẤP NHÁY (Breath Effect)
        // Dùng UnscaledTime vì Time.timeScale đang bằng 0
        if (blinkText != null)
        {
            // Mathf.Sin tạo ra giá trị từ -1 đến 1. Ta chuyển nó thành 0.2 đến 1 để không bị mờ hẳn.
            float alpha = (Mathf.Sin(Time.unscaledTime * blinkSpeed) + 1f) / 2f;

            // Giữ nguyên màu gốc, chỉ đổi độ trong suốt (Alpha)
            Color originalColor = blinkText.color;
            blinkText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        }

        // 2. LOGIC THOÁT GAME
        // Chờ khoảng 0.5s sau khi hiện bảng rồi mới cho bấm (tránh bấm nhầm)
        delayTimer += Time.unscaledDeltaTime;

        if (delayTimer > 0.5f)
        {
            // Input.anyKeyDown bắt được cả Phím bàn phím + Chuột + Cảm ứng
            if (Input.anyKeyDown)
            {
                LoadMenu();
            }
        }
    }

    public void LoadMenu()
    {
        // Quan trọng: Phải trả lại thời gian trước khi đổi Scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}