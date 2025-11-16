using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 6;
    public int currentHP;

    // Tham chiếu đến script UI của bạn
    public PlayerHPUI healthUI;
    // Tham chiếu đến script Player (script di chuyển)
    [SerializeField] private Player playerController;

    void Start()
    {
        
    }

    // Đây là hàm để các kịch bản khác (như kẻ thù) gọi
    public void TakeDamage(int damage)
    {
        if (playerController != null && playerController.IsHit())
        {
            return; // Không nhận sát thương nếu đang nhấp nháy/bất tử
        }

        currentHP -= damage;
        FindFirstObjectByType<CameraShake>().ShakeCamera(10f, 0.3f);
        // Đảm bảo máu không tụt xuống dưới 0
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        Debug.Log("Player HP: " + currentHP);

        if (playerController != null)
        {
            playerController.StartHitEffect();
        }

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
        Debug.Log("Player đã chết! (gọi từ PlayerHealth.cs)");

        // SỬA: Gọi hàm Die() bên script Player để xử lý animation/hủy đối tượng
        if (playerController != null)
        {
            playerController.Die();
        }
        else
        {
            // Nếu không tìm thấy script Player, thì tự hủy
            //Destroy(gameObject);
        }
    }
}
