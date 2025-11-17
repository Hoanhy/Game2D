using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Chỉ số máu")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("Knockback khi bị đánh")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isKnockedBack = false;

    [HideInInspector]
    public EnemySpawner spawner; // gán khi spawn

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackDuration);
        rb.linearVelocity = Vector2.zero;
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

        // --- Bổ sung: báo EnemySpawner ---
        if (spawner != null)
            spawner.EnemyDied();

        Destroy(gameObject, 1.5f);
    }
}
