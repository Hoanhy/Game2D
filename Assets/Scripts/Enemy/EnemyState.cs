using UnityEngine;

public class EnemyState : MonoBehaviour
{
    // ID định danh. QUAN TRỌNG: Mỗi con quái trong màn chơi phải có ID KHÁC NHAU!
    public string enemyID;

    private EnemyHealth healthScript;

    void Awake()
    {
        healthScript = GetComponent<EnemyHealth>();

        // Tự động tạo ID ngẫu nhiên nếu bạn quên đặt (chỉ dùng khi test)
        if (string.IsNullOrEmpty(enemyID))
        {
            enemyID = System.Guid.NewGuid().ToString();
        }
    }

    // Hàm này để SaveManager gọi: "Này, đưa dữ liệu đây để tao lưu"
    public EnemyData GetData()
    {
        // Kiểm tra xem quái đã chết chưa
        // (Dựa trên máu <= 0 hoặc object bị tắt)
        bool isDead = (healthScript != null && healthScript.currentHealth <= 0);

        int hp = (healthScript != null) ? healthScript.currentHealth : 0;

        return new EnemyData(enemyID, transform.position, hp, isDead);
    }

    // Hàm này để SaveManager gọi: "Này, đây là dữ liệu cũ của mày, cập nhật đi"
    public void LoadData(EnemyData data)
    {
        // 1. Nếu đã chết trong file save -> Tắt luôn
        if (data.isDead)
        {
            gameObject.SetActive(false); // Hoặc Destroy(gameObject);
            return;
        }

        // 2. Cập nhật vị trí
        transform.position = data.position.ToVector2();

        // 3. Cập nhật máu
        if (healthScript != null)
        {
            healthScript.currentHealth = data.health;
        }
    }

    // Nút bấm tiện lợi trong Inspector để tạo ID
    [ContextMenu("Generate Random ID")]
    private void GenerateID()
    {
        enemyID = System.Guid.NewGuid().ToString();
    }
}