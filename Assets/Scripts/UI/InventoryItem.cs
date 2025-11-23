using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Enum để phân loại (nếu cần dùng)
public enum ItemType { None, Weapon, Consumable }

public class InventoryItem : MonoBehaviour
{
    [Header("UI References (Kéo thả vào)")]
    public Image iconImage;        // Ảnh con (ItemIcon)
    public TextMeshProUGUI amountText; // Chữ số lượng
    public Button button;          // Nút bấm (của chính Slot)

    [Header("Dữ liệu (Tự động điền)")]
    public string itemName;
    public int quantity;
    public ItemType itemType;
    public int healAmount;

    // --- HÀM 1: HIỂN THỊ ITEM ---
    public void SetSlotData(string name, int qty)
    {
        this.itemName = name;
        this.quantity = qty;

        // 1. Load ảnh từ Resources
        Sprite sprite = Resources.Load<Sprite>("Icons/" + name);

        if (sprite != null)
        {
            iconImage.sprite = sprite;
            iconImage.color = Color.white; // Hiện ảnh lên
            iconImage.enabled = true;
        }
        else
        {
            Debug.LogError("Không tìm thấy ảnh: Icons/" + name);
        }

        // 2. Cập nhật số lượng
        if (amountText != null)
            amountText.text = (quantity > 1) ? quantity.ToString() : "";

        // 3. Phân loại tự động dựa theo tên (để bấm nút dùng được)
        if (name == "Potion")
        {
            itemType = ItemType.Consumable;
            healAmount = 1;
        }
        else
        {
            itemType = ItemType.Weapon;
        }

        // 4. Bật nút bấm
        if (button != null) button.interactable = true;
    }

    // --- HÀM 2: XÓA SLOT (Làm trống) ---
    public void ClearSlot()
    {
        itemName = "";
        quantity = 0;
        itemType = ItemType.None;

        iconImage.sprite = null;
        iconImage.enabled = false; // Tắt ảnh đi
        if (amountText != null) amountText.text = "";
        if (button != null) button.interactable = false;
    }

    // --- HÀM DÙNG ITEM (Gắn vào nút OnClick) ---
    public void UseItem()
    {
        if (itemType == ItemType.None) return;

        PlayerAttack playerAttack = FindFirstObjectByType<PlayerAttack>();
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();

        // Dùng Vũ khí
        if (itemType == ItemType.Weapon)
        {
            if (playerAttack != null)
            {
                if (itemName == "Sword") playerAttack.EquipSword();
                else if (itemName == "Axe") playerAttack.EquipAxe();
            }
        }
        // Dùng Máu
        else if (itemType == ItemType.Consumable)
        {
            if (playerHealth != null && playerHealth.currentHP < playerHealth.maxHP)
            {
                playerHealth.Heal(healAmount);

                // Giảm số lượng trong Manager
                InventoryManager.Instance.ReduceItem(itemName);
            }
        }
    }
}