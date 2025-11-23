using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    // Kéo cái InventoryPanel (Cha của các Slot) vào đây
    public Transform slotParent;

    // Danh sách dữ liệu ảo (Quản lý thứ tự tại đây)
    // (Cấu trúc InventoryData lấy từ file GameData.cs của bạn)
    private List<InventoryData> items = new List<InventoryData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        // Đợi 1 frame để đảm bảo mọi thứ load xong
        StartCoroutine(InitializeInventory());
    }
    System.Collections.IEnumerator InitializeInventory()
    {
        yield return null; // Chờ 1 frame

        // Kiểm tra xem Slot 1 có đồ chưa
        // (Lấy slot đầu tiên trong danh sách con)
        if (slotParent.childCount > 0)
        {
            InventoryItem firstSlot = slotParent.GetChild(0).GetComponent<InventoryItem>();

            // Nếu Slot 1 chưa có đồ (quantity == 0) VÀ đây là game mới (chưa có dữ liệu load)
            if (firstSlot != null && firstSlot.quantity == 0)
            {
                // Kiểm tra thêm: Nếu SaveManager đang có dữ liệu load dở thì đừng tạo đồ mới
                // (Tránh trường hợp Load Game mà lại bị đè Kiếm vào)
                bool isLoadingSave = false;
                if (SaveManager.Instance != null && SaveManager.Instance.HasLoadedData)
                {
                    isLoadingSave = true;
                }

                if (!isLoadingSave)
                {
                    Debug.Log("Túi trống (New Game) -> Tạo Kiếm mặc định.");
                    AddItem("Sword");
                }
            }
        }
    }
    public void AddItem(string itemName)
    {
        // 1. Kiểm tra cộng dồn (Cho Potion)
        if (itemName == "Potion")
        {
            foreach (var item in items)
            {
                if (item.itemName == itemName)
                {
                    item.quantity++;
                    RefreshUI(); // Vẽ lại giao diện
                    return;
                }
            }
        }

        // 2. Tạo dữ liệu mới
        InventoryData newItem = new InventoryData(itemName, 1);

        // 3. LOGIC SẮP XẾP (Cái bạn cần nằm ở đây)
        if (itemName == "Axe")
        {
            // Nếu là Rìu -> Chèn vào vị trí số 0 (Đầu tiên)
            // Kiếm và Máu sẽ tự động bị đẩy lùi xuống
            items.Insert(0, newItem);
        }
        else if (itemName == "Sword")
        {
            // Kiếm thì chèn vào đầu (nếu chưa có Rìu) hoặc sau Rìu
            // Để đơn giản: Cứ cho Kiếm vào cuối danh sách nếu không phải Rìu
            items.Add(newItem);
        }
        else
        {
            // Các món khác (Potion) thêm vào cuối
            items.Add(newItem);
        }

        // 4. Cập nhật lên các Slot thật
        RefreshUI();
    }

    // Hàm giảm số lượng khi dùng
    public void ReduceItem(string itemName)
    {
        InventoryData itemToRemove = null;
        foreach (var item in items)
        {
            if (item.itemName == itemName)
            {
                item.quantity--;
                if (item.quantity <= 0) itemToRemove = item;
                break;
            }
        }

        if (itemToRemove != null) items.Remove(itemToRemove);

        RefreshUI();
    }

    // --- HÀM QUAN TRỌNG: VẼ LẠI UI ---
    // Hàm này sẽ lấy danh sách items và điền vào các Slot (1), Slot (2)...
    void RefreshUI()
    {
        // Lấy tất cả các ô Slot con
        // (Lưu ý: Phải đảm bảo tất cả Slot đều có script InventoryItem)
        InventoryItem[] slots = slotParent.GetComponentsInChildren<InventoryItem>(true);

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count)
            {
                // Nếu có dữ liệu -> Điền vào Slot
                slots[i].SetSlotData(items[i].itemName, items[i].quantity);
            }
            else
            {
                // Nếu hết dữ liệu -> Làm trống Slot
                slots[i].ClearSlot();
            }
        }
    }

    // Các hàm Save/Load của bạn có thể gọi:
    public List<InventoryData> GetInventoryData()
    {
        List<InventoryData> dataList = new List<InventoryData>();

        foreach (Transform child in slotParent)
        {
            InventoryItem item = child.GetComponent<InventoryItem>();

            // Lưu tất cả slot nào CÓ SỐ LƯỢNG > 0
            if (item != null && item.quantity > 0)
            {
                Debug.Log("Đang lưu món đồ: " + item.itemName);
                dataList.Add(new InventoryData(item.itemName, item.quantity));
            }
        }
        return dataList;
    }

    public void LoadInventory(List<InventoryData> savedData)
    {
        // 1. Xóa sạch túi trước khi load
        foreach (Transform child in slotParent)
        {
            InventoryItem slot = child.GetComponent<InventoryItem>();
            if (slot != null) slot.ClearSlot();
        }

        // 2. Load lại từng món
        foreach (InventoryData data in savedData)
        {
            AddItem(data.itemName);
        }
    }
}