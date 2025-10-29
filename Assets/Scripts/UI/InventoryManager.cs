using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Transform slotParent;      // SlotParent chứa các slot
    public GameObject slotPrefab;     // Prefab 1 ô slot

    void Start()
    {
        AddDefaultWeapon();
    }

    void AddDefaultWeapon()
    {
        // Load icon từ Resources
        Sprite swordIcon = Resources.Load<Sprite>("Icons/Item1");
        if (swordIcon != null)
        {
            // Tạo slot mới trong SlotParent
            GameObject slot = Instantiate(slotPrefab, slotParent);
            // Gán icon vào Image component của slot
            slot.GetComponent<Image>().sprite = swordIcon;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy icon kiếm trong Resources/Icons/");
        }
    }
}
