using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // Tính hướng từ người đánh (hitbox) đến enemy
                Vector2 attackDir = (collision.transform.position - transform.position).normalized;
                enemyHealth.TakeDamage(damage, attackDir);
            }
        }
    }
}
