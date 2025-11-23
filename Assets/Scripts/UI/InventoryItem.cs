using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ItemType { Consumable, Weapon }
public class InventoryItem : MonoBehaviour
{
    [Header("Thông tin vật phẩm")]
    public string itemName; // Tên để phân biệt (ví dụ "Potion")
    public int quantity = 1; // Số lượng hiện có
    public ItemType itemType;

    [Header("Cài đặt")]
    public bool isPotion = false;
    public int healAmount = 1;

    [Header("UI")]
    public TextMeshProUGUI amountText; // Kéo cái Text số lượng vào đây

    // Hàm cập nhật số lượng lên màn hình
    public void UpdateQuantityUI()
    {
        if (amountText != null)
        {
            if (quantity > 1)
                amountText.text = quantity.ToString(); // Hiện số nếu > 1
            else
                amountText.text = ""; // Ẩn số nếu chỉ có 1
        }
    }

    // Hàm này sẽ được gọi khi bấm vào nút trên UI
    public void UseItem()
    {
        if (isPotion)
        {
            PlayerAttack playerAttack = FindFirstObjectByType<PlayerAttack>();
            PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();

            if (itemType == ItemType.Weapon)
            {
                if (playerAttack != null)
                {
                    if (itemName == "Sword")
                    {
                        playerAttack.EquipSword();
                        Debug.Log("Đã trang bị Kiếm!");
                    }
                    else if (itemName == "Axe")
                    {
                        playerAttack.EquipAxe();
                        Debug.Log("Đã trang bị Rìu!");
                    }
                    // Vũ khí dùng xong KHÔNG biến mất, nên không có Destroy
                }
            }

            // --- TRƯỜNG HỢP 2: LÀ BÌNH MÁU ---
            else if (itemType == ItemType.Consumable) // Potion
            {
                if (playerHealth != null)
                {
                    if (playerHealth.currentHP < playerHealth.maxHP)
                    {
                        playerHealth.Heal(healAmount);

                        quantity--;
                        if (quantity <= 0) Destroy(gameObject);
                        else UpdateQuantityUI();
                    }
                }
            }
        }
    }
}