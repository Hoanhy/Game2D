using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    public GameObject wallGroup;       // Nhóm tường
    public EnemySpawner spawner;       // EnemySpawner

    private bool triggered = false;
    private Collider2D triggerCollider; // Tham chiếu đến collider

    private void Start()
    {
        if (wallGroup != null)
            wallGroup.SetActive(false); // tắt tường lúc đầu
        // Nếu spawner đã xong (check qua ID trong SaveManager), thì tắt Trigger này luôn
        if (spawner != null && SaveManager.Instance != null)
        {
            if (SaveManager.Instance.IsSpawnerFinished(spawner.spawnerID))
            {
                Debug.Log("SpawnTrigger: Spawner này đã xong, tự hủy trigger.");
                triggered = true;
                if (triggerCollider != null) triggerCollider.enabled = false; // Tắt va chạm
                if (wallGroup != null) wallGroup.SetActive(false); // Đảm bảo tường tắt
                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player"))
        {
            triggered = true;

            if (wallGroup != null)
                wallGroup.SetActive(true); // bật tường

            if (spawner != null)
            {
                spawner.StartSpawn();
            }
            else
            {
                Debug.LogWarning("SpawnTrigger: EnemySpawner chưa gán hoặc đã bị hủy!");
                // Nếu không có spawner thì mở tường ra lại để không bị kẹt
                if (wallGroup != null) wallGroup.SetActive(false);
            }
        }
    }

    // Gọi khi EnemySpawner thông báo đã clear hết enemy
    public void ClearWall()
    {
        if (wallGroup != null)
            wallGroup.SetActive(false); // tắt tường
        if (triggerCollider != null)
            triggerCollider.enabled = false;
    }
}
