using UnityEngine;

// Gắn script này vào một Trigger Checkpoint
public class Checkpoint : MonoBehaviour
{
    [Header("Effects")]
    public GameObject popupTextPrefab; // Kéo Prefab chữ vào đây
    public Vector3 textOffset = new Vector3(0, 1.5f, 0); // Chỉnh độ cao của chữ
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ĐÃ CHẠM CHECKPOINT! ĐANG LƯU GAME...");
            // 1. Lưu game (Code cũ)
            SaveManager.Instance.SaveGame(true);

            // 2. Tắt Collider (Code cũ)
            GetComponent<Collider2D>().enabled = false;

            if (popupTextPrefab != null)
            {
                // Tạo ra chữ tại vị trí Checkpoint + độ lệch (Offset)
                Instantiate(popupTextPrefab, transform.position + textOffset, Quaternion.identity);
            }
            // ---------------------------------

            Debug.Log("ĐÃ CHẠM CHECKPOINT!");
        }
    }
}