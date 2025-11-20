using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Transform slotParent;
    public GameObject slotPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AddItem(string itemName)
    {
        // --- BƯỚC 1: KIỂM TRA XEM ĐÃ CÓ CHƯA ---
        // Duyệt qua tất cả các ô đang có trong túi
        foreach (Transform child in slotParent)
        {
            InventoryItem itemScript = child.GetComponent<InventoryItem>();

            // Nếu tìm thấy vật phẩm cùng tên
            if (itemScript != null && itemScript.itemName == itemName)
            {
                itemScript.quantity++; // Tăng số lượng
                itemScript.UpdateQuantityUI(); // Cập nhật text
                Debug.Log("Đã cộng dồn " + itemName);
                return; // Dừng hàm luôn, không tạo mới nữa
            }
        }

        // --- BƯỚC 2: NẾU CHƯA CÓ THÌ TẠO MỚI ---
        Sprite itemIcon = Resources.Load<Sprite>("Icons/" + itemName);

        if (itemIcon != null)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotParent);
            newSlot.GetComponent<Image>().sprite = itemIcon;

            InventoryItem newItemScript = newSlot.GetComponent<InventoryItem>();
            if (newItemScript != null)
            {
                newItemScript.itemName = itemName; // Lưu tên để lần sau tìm thấy
                newItemScript.quantity = 1;
                newItemScript.UpdateQuantityUI(); // Cập nhật text lần đầu

                if (itemName == "Potion")
                {
                    newItemScript.isPotion = true;
                    newItemScript.healAmount = 1;
                }
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy icon: " + itemName);
        }
    }
    // --- HÀM 1: LẤY DỮ LIỆU ĐỂ LƯU (SaveManager sẽ gọi) ---
    public List<InventoryData> GetInventoryData()
    {
        List<InventoryData> dataList = new List<InventoryData>();

        // Duyệt qua tất cả các ô Slot đang có
        foreach (Transform child in slotParent)
        {
            InventoryItem item = child.GetComponent<InventoryItem>();
            if (item != null)
            {
                // Lưu tên và số lượng vào danh sách
                dataList.Add(new InventoryData(item.itemName, item.quantity));
            }
        }
        return dataList;
    }

    // --- HÀM 2: TẢI DỮ LIỆU VÀO TÚI (SaveManager sẽ gọi) ---
    public void LoadInventory(List<InventoryData> savedData)
    {
        // 1. Xóa sạch túi đồ hiện tại (để tránh bị trùng đồ cũ)
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }

        // 2. Tạo lại từng món đồ từ dữ liệu đã lưu
        foreach (InventoryData data in savedData)
        {
            // Load hình ảnh
            Sprite itemIcon = Resources.Load<Sprite>("Icons/" + data.itemName);

            if (itemIcon != null)
            {
                // Tạo Slot
                GameObject newSlot = Instantiate(slotPrefab, slotParent);
                newSlot.GetComponent<Image>().sprite = itemIcon;

                // Cài đặt thông số (Tên, Số lượng)
                InventoryItem itemScript = newSlot.GetComponent<InventoryItem>();
                if (itemScript != null)
                {
                    itemScript.itemName = data.itemName;
                    itemScript.quantity = data.quantity;
                    itemScript.UpdateQuantityUI(); // Cập nhật số hiển thị (ví dụ "x5")

                    // Cài đặt chức năng riêng (ví dụ Potion)
                    if (data.itemName == "Potion")
                    {
                        itemScript.isPotion = true;
                        itemScript.healAmount = 1;
                    }
                }
            }
        }
    }
}