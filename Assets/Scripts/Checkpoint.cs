using UnityEngine;

// Gắn script này vào một Trigger Checkpoint
public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ĐÃ CHẠM CHECKPOINT! ĐANG LƯU GAME...");
            SaveManager.Instance.SaveGame();
        }
    }
}