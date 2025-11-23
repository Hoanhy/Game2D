using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Header("Cài đặt ID (BẮT BUỘC PHẢI CÓ)")]
    public string itemID;
    [Header("Loại vũ khí")]
    public bool isAxe = true;

    private void Start()
    {
        // 1. Kiểm tra ngay khi game bắt đầu
        if (SaveManager.Instance != null)
        {
            // Nếu trong file save nói là "Đã nhặt ID này rồi"
            if (SaveManager.Instance.IsItemCollected(itemID))
            {
                Debug.Log($"Vật phẩm {itemID} đã bị nhặt trước đó. Tự hủy.");
                Destroy(gameObject); // Biến mất ngay lập tức
            }
        }
    }
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

            if (playerAttack != null && isAxe) playerAttack.EquipAxe();
            // Báo cho SaveManager biết: "Tôi đã bị nhặt!"
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.MarkItemCollected(itemID);
            }

            // Hủy vật phẩm
            Destroy(gameObject);
        }
    }
    // Nút tạo ID nhanh
    [ContextMenu("Generate ID")]
    private void GenerateID()
    {
        itemID = System.Guid.NewGuid().ToString();
    }
}