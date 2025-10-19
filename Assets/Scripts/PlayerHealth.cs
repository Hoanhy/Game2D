using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 6;
    private int currentHP;

    // Tham chiếu đến script UI của bạn
    public PlayerHPUI healthUI;

    void Start()
    {
        // Bắt đầu game với đầy máu
        currentHP = maxHP;

        // Cập nhật UI lần đầu tiên
        if (healthUI != null)
        {
            healthUI.UpdateHeartUI(currentHP, maxHP);
        }
    }

    // Đây là hàm để các kịch bản khác (như kẻ thù) gọi
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        // Đảm bảo máu không tụt xuống dưới 0
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        Debug.Log("Player HP: " + currentHP);

        // Yêu cầu UI cập nhật
        if (healthUI != null)
        {
            healthUI.UpdateHeartUI(currentHP, maxHP);
        }

        // Kiểm tra nếu chết
        if (currentHP <= 0)
        {
            Die();
        }
    }

    // Hàm hồi máu (nếu cần)
    public void Heal(int amount)
    {
        currentHP += amount;

        // Đảm bảo máu không vượt quá tối đa
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        Debug.Log("Player HP: " + currentHP);

        // Yêu cầu UI cập nhật
        if (healthUI != null)
        {
            healthUI.UpdateHeartUI(currentHP, maxHP);
        }
    }

    private void Die()
    {
        Debug.Log("Player đã chết!");
        // Thêm logic khi chết tại đây (ví dụ: chạy animation, tải lại màn chơi)
        // Destroy(gameObject);
    }
}
