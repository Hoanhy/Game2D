using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Chỉ số máu")]
    public int maxHealth = 50;
    private int currentHealth;

    [Header("Knockback khi bị đánh")]
    public float knockbackForce = 5f; // lực đẩy lùi
    public float knockbackDuration = 0.2f; // thời gian bị đẩy lùi

    private Animator animator;
    private Rigidbody2D rb;
    private bool isKnockedBack = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Gọi khi enemy nhận sát thương
    public void TakeDamage(int amount, Vector2 attackDirection)
    {
        if (isKnockedBack) return; // tránh spam hit liên tục

        currentHealth -= amount;
        Debug.Log($"Enemy nhận sát thương: {amount} | HP còn lại: {currentHealth}");
        

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (animator != null)
                animator.SetTrigger("Hit");

            // Thêm hiệu ứng knockback
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
        Debug.Log("Enemy chết!");
        if (animator != null)
            animator.SetTrigger("Die");

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Enemy>().enabled = false;
        GetComponent<EnemyAttack>().enabled = false;

        Destroy(gameObject, 1.5f);
    }
}
