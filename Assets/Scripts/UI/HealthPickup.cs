using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Gọi InventoryManager để thêm "Potion" vào túi
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.AddItem("Potion"); // "Potion" là tên file ảnh trong Resources

                Debug.Log("Đã nhặt bình máu vào túi!");

                // Hủy vật phẩm dưới đất
                Destroy(gameObject);
            }
        }
    }
}