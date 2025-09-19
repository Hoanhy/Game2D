using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Chỉ số máu")]
    public int maxHealth = 50;
    private int currentHealth;

    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Enemy nhận sát thương: " + amount + " | HP còn lại: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (animator != null)
                animator.SetTrigger("Hit");
        }
    }

    void Die()
    {
        Debug.Log("Enemy chết!");
        if (animator != null)
            animator.SetTrigger("Die");

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Enemy>().enabled = false;
        GetComponent<EnemyAttack>().enabled = false;

        Destroy(gameObject, 2f); // chờ animation chết xong
    }
}
