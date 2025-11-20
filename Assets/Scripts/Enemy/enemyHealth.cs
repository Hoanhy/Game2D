using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Chỉ số máu")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("Knockback khi bị đánh")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    [Header("Loot Drop")]
    public GameObject healthPotionPrefab; // Kéo Prefab bình máu vào đây
    [Range(0, 100)] public float dropChance = 40f; // Tỷ lệ rơi (ví dụ 40%)

    private Animator animator;
    private Rigidbody2D rb;
    public bool isKnockedBack = false;

    [HideInInspector]
    public EnemySpawner spawner; // gán khi spawn

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void TakeDamage(int amount, Vector2 attackDirection)
    {
        if (isKnockedBack) return;

        currentHealth -= amount;
        Debug.Log($"Enemy nhận sát thương: {amount} | HP còn lại: {currentHealth}");
        

        if (currentHealth <= 0)
            Die();
        else
        {
            if (animator != null)
                animator.SetTrigger("Hit");

            StartCoroutine(ApplyKnockback(attackDirection));
        }
    }

    private System.Collections.IEnumerator ApplyKnockback(Vector2 direction)
    {
        isKnockedBack = true;
        //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackDuration);
        rb.linearVelocity = Vector2.zero;
        //rb.constraints = RigidbodyConstraints2D.FreezeAll;
        isKnockedBack = false;
    }

    void Die()
    {
        if (animator != null)
            animator.SetTrigger("Die");

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Enemy>().enabled = false;
        GetComponent<EnemyAttack>().enabled = false;
        EnemyState state = GetComponent<EnemyState>();
        if (state != null && SaveManager.Instance != null)
        {
            // Ghi tên mình vào sổ tử
            SaveManager.Instance.AddDeadEnemy(state.enemyID);
        }
        if (healthPotionPrefab != null)
        {
            // Random.Range(0f, 100f) sẽ trả về một số từ 0 đến 100
            // Nếu số đó nhỏ hơn dropChance (ví dụ 30), thì rơi đồ
            if (Random.Range(0f, 100f) <= dropChance)
            {
                // Sinh ra bình máu tại vị trí của quái
                Instantiate(healthPotionPrefab, transform.position, Quaternion.identity);
                Debug.Log("Enemy đã rơi ra bình máu!");
            }
        }
        // --- Bổ sung: báo EnemySpawner ---
        if (spawner != null)
            spawner.EnemyDied();

        Destroy(gameObject, 1.5f);
    }
}
