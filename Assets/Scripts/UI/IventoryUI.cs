using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Inventory Panel")]
    public GameObject inventoryPanel; // Kéo panel chính vào đây trong Inspector

    private bool isOpen = false;

    void Start()
    {
        // Đảm bảo kho đồ ẩn khi bắt đầu game
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        // Nhấn phím E để mở / đóng kho đồ
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);
    }
}
