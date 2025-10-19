using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Tấn công")]
    public float attackCooldown = 1.5f;
    public int damage = 1;

    private Animator animator;
    private float lastAttackTime;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void DoAttack(Transform player, float attackRange)
    {
        float distance = Vector2.Distance(transform.position, player.position);

        // Nếu Player ở ngoài tầm đánh
        if (distance > attackRange)
        {
            animator.SetBool("isAttacking", false);
            return;
        }

        // Trong tầm đánh → chuyển sang animation Attack
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", true);

        // Cooldown chỉ kiểm soát việc gây sát thương
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("Enemy Attack!");

            // Nếu Player có script máu thì gọi ở đây
            player.GetComponent<PlayerHealth>().TakeDamage(damage);

            lastAttackTime = Time.time;
        }
    }
}
