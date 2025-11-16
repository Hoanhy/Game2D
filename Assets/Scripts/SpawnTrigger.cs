using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    public GameObject wallGroup;       // Nhóm tường
    public EnemySpawner spawner;       // EnemySpawner

    private bool triggered = false;

    private void Start()
    {
        if (wallGroup != null)
            wallGroup.SetActive(false); // tắt tường lúc đầu
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
                spawner.StartSpawn();      // bắt đầu spawn enemy
            else
                Debug.LogWarning("SpawnTrigger: EnemySpawner chưa gán!");
        }
    }

    // Gọi khi EnemySpawner thông báo đã clear hết enemy
    public void ClearWall()
    {
        if (wallGroup != null)
            wallGroup.SetActive(false); // tắt tường
    }
}
