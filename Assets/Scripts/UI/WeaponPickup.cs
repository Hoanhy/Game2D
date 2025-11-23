using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Header("Loại vũ khí")]
    public bool isAxe = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (InventoryManager.Instance != null)
            {
                // Thêm Rìu vào túi (Code InventoryManager sẽ lo việc xếp chỗ)
                InventoryManager.Instance.AddItem("Axe");
            }
            // Tìm script PlayerAttack trên người chơi
            PlayerAttack playerAttack = other.GetComponent<PlayerAttack>();

            if (playerAttack != null)
            {
                if (isAxe)
                {
                    // Gọi hàm trang bị Rìu
                    playerAttack.EquipAxe();
                }

                // (Bạn có thể thêm hiệu ứng âm thanh nhặt đồ ở đây)

                // Hủy vật phẩm dưới đất
                Destroy(gameObject);
            }
        }
    }
}