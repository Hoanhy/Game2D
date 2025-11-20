using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    [Header("Thông tin vật phẩm")]
    public string itemName; // Tên để phân biệt (ví dụ "Potion")
    public int quantity = 1; // Số lượng hiện có

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
            PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();

            if (playerHealth != null)
            {
                if (playerHealth.currentHP < playerHealth.maxHP)
                {
                    playerHealth.Heal(healAmount);
                    Debug.Log("Đã uống thuốc!");

                    // --- LOGIC MỚI: GIẢM SỐ LƯỢNG ---
                    quantity--;

                    if (quantity <= 0)
                    {
                        Destroy(gameObject); // Hết thì xóa
                    }
                    else
                    {
                        UpdateQuantityUI(); // Còn thì cập nhật số
                    }
                    // --------------------------------
                }
                else
                {
                    Debug.Log("Máu đang đầy!");
                }
            }
        }
    }
}