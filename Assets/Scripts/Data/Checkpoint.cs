using UnityEngine;

// Gắn script này vào một Trigger Checkpoint
public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ĐÃ CHẠM CHECKPOINT! ĐANG LƯU GAME...");
            // Gọi SaveGame với true (CÓ lưu vị trí)
            SaveManager.Instance.SaveGame(true);

            // Tự tắt để không lưu liên tục
            GetComponent<Collider2D>().enabled = false;
        }
    }
}