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
    private void Start()
    {
        // --- TẠO ITEM MẶC ĐỊNH LÚC ĐẦU GAME ---
        // Chỉ tạo nếu đây là New Game (kiểm tra SaveManager nếu cần)
        // Nếu bạn đã có hệ thống Load Game, phần này có thể bỏ qua
        // hoặc chỉ chạy khi inventory trống.
        if (slotParent.childCount == 0)
        {
            AddItem("Sword");  // Mặc định Slot 1
   

        }   
    }
    public void AddItem(string itemName)
    {
        // 1. Kiểm tra cộng dồn (Chỉ áp dụng cho Potion)
        if (itemName == "Potion")
        {
            foreach (Transform child in slotParent)
            {
                InventoryItem itemScript = child.GetComponent<InventoryItem>();
                if (itemScript != null && itemScript.itemName == itemName)
                {
                    itemScript.quantity++;
                    itemScript.UpdateQuantityUI();
                    Debug.Log("Đã cộng dồn Potion thành: " + itemScript.quantity);
                    return;
                }
            }
        }

        // 2. Load hình ảnh (Thêm debug để kiểm tra lỗi)
        string path = "Icons/" + itemName;
        Sprite itemIcon = Resources.Load<Sprite>(path);

        if (itemIcon == null)
        {
            Debug.LogError("LỖI TO: Không tìm thấy ảnh tại đường dẫn: Resources/" + path + ". Kiểm tra lại tên file hoặc thư mục!");
            return;
        }

        // 3. Tạo Slot
        if (slotPrefab == null || slotParent == null)
        {
            Debug.LogError("LỖI: Quên kéo SlotPrefab hoặc SlotParent vào InventoryManager rồi!");
            return;
        }

        GameObject newSlot = Instantiate(slotPrefab, slotParent);
        newSlot.GetComponent<Image>().sprite = itemIcon;

        InventoryItem newItemScript = newSlot.GetComponent<InventoryItem>();
        if (newItemScript != null)
        {
            newItemScript.itemName = itemName;
            newItemScript.quantity = 1;
            newItemScript.UpdateQuantityUI();

            // --- LOGIC SẮP XẾP VỊ TRÍ ---
            if (itemName == "Potion")
            {
                newItemScript.itemType = ItemType.Consumable;
                newItemScript.healAmount = 2;
                // Potion mặc định sinh ra ở cuối cùng -> Đúng ý bạn (Slot 3)
            }
            else if (itemName == "Sword")
            {
                newItemScript.itemType = ItemType.Weapon;
                // Kiếm sinh ra đầu game -> Mặc định Slot 1
            }
            else if (itemName == "Axe")
            {
                newItemScript.itemType = ItemType.Weapon;

                // Rìu chen lên đầu tiên -> Đẩy Kiếm xuống Slot 2, Máu xuống Slot 3
                newSlot.transform.SetAsFirstSibling();
                Debug.Log("Đã nhặt Rìu và đưa lên đầu túi!");
            }
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