using UnityEngine;

public class enemySlime : MonoBehaviour
{
    [Header("Thuộc tính di chuyển")]
    public float speed = 2f;
    public float moveTime = 3f;
    public float stopTime = 1f;

    [Header("Phát hiện Player")]
    public float chaseRange = 5f;
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;

    private float timer;
    private bool movingRight = true;
    private bool isStopped = false;
    private Animator animator;
    private Transform player;
    private float lastAttackTime;

    private enum State { Patrol, Chase, Attack }
    private State currentState = State.Patrol;

    void Start()
    {
        timer = moveTime;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Xác định state
        if (distance <= attackRange)
            currentState = State.Attack;
        else if (distance <= chaseRange)
            currentState = State.Chase;
        else
            currentState = State.Patrol;

        // Gọi hành vi
        switch (currentState)
        {
            case State.Patrol: Patrol(); break;
            case State.Chase: Chase(); break;
            case State.Attack: Attack(distance); break;
        }
    }

    void Patrol()
    {
        animator.SetBool("isAttacking", false);

        if (isStopped)
        {
            animator.SetBool("isRunning", false);
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                movingRight = !movingRight;
                isStopped = false;
                timer = moveTime;
            }
            return;
        }

        animator.SetBool("isRunning", true);

        timer -= Time.deltaTime;
        Vector2 dir = movingRight ? Vector2.right : Vector2.left;
        transform.Translate(dir * speed * Time.deltaTime);

        if (timer <= 0f)
        {
            isStopped = true;
            timer = stopTime;
        }

        transform.localScale = new Vector3(movingRight ? -1 : 1, 1, 1);
    }

    void Chase()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isRunning", true);

        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    void Attack(float distance)
    {
        // Nếu player chạy xa -> thoát Attack
        if (distance > attackRange)
        {
            animator.SetBool("isAttacking", false);
            return;
        }

        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", true);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("Monster Attack Player!");
            // player.TakeDamage(damage);
            lastAttackTime = Time.time;
        }
    }
}
