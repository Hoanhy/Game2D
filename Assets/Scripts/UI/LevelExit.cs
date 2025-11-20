using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [Header("Cài đặt UI")]
    public GameObject demoEndPanel; // Kéo cái Panel "DemoEndPanel" (đang tắt) vào đây

    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu Player chạm vào cổng và chưa kích hoạt lần nào
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;

            if (demoEndPanel != null)
            {
                // Bật Panel lên. 
                // Script "DemoEndController" (gắn trên Panel) sẽ tự động chạy:
                // dừng thời gian, làm chữ nhấp nháy và lắng nghe phím bấm.
                demoEndPanel.SetActive(true);
            }
            else
            {
                Debug.LogError("Quên kéo DemoEndPanel vào script LevelExit rồi bạn ơi!");
            }
        }
    }
}