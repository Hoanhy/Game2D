using UnityEngine;

[RequireComponent(typeof(EnemyHealth), typeof(EnemyAttack))]
public class Enemy : MonoBehaviour
{
    [Header("Thuộc tính di chuyển")]
    public float speed = 2f;
    public float moveTime = 3f;
    public float stopTime = 1f;

    [Header("Giới hạn vùng di chuyển")]
    public float patrolRadius = 5f;
    private Vector2 spawnPos;

    [Header("Phát hiện Player")]
    public float chaseRange = 5f;
    public float attackRange = 1f;

    private float timer;
    private bool isStopped = false;
    private Animator animator;
    private Transform player;
    private Vector2 moveDir;

    private EnemyAttack enemyAttack;
    private Rigidbody2D rb;
    private EnemyHealth enemyHealth;

    private enum State { Patrol, Chase, Attack }
    private State currentState = State.Patrol;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyAttack = GetComponent<EnemyAttack>();
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        spawnPos = transform.position;
        timer = Random.Range(1f, moveTime);
        isStopped = Random.value > 0.5f;
        PickRandomDirection();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
            currentState = State.Attack;
        else if (distance <= chaseRange)
            currentState = State.Chase;
        else
            currentState = State.Patrol;
        if (enemyHealth != null && enemyHealth.isKnockedBack)
        {
            return;
        }

        switch (currentState)
        {
            case State.Patrol: Patrol(); break;
            case State.Chase: Chase(); break;
            case State.Attack: enemyAttack.DoAttack(player, attackRange);
                rb.linearVelocity = Vector2.zero;
                enemyAttack.DoAttack(player, attackRange);
                break;
        }
    }

    void Patrol()
    {
        animator.SetBool("isAttacking", false);

        if (isStopped)
        {
            animator.SetBool("isRunning", false);
            rb.linearVelocity = Vector2.zero;
            timer -= Time.fixedDeltaTime;
            if (timer <= 0f)
            {
                isStopped = false;
                timer = moveTime;
                PickRandomDirection();
            }
            return;
        }

        animator.SetBool("isRunning", true);

        timer -= Time.fixedDeltaTime;
        Vector2 currentPos = rb.position;
        Vector2 nextPos = (Vector2)transform.position + moveDir * speed * Time.deltaTime;

        if (Vector2.Distance(nextPos, spawnPos) > patrolRadius)
        {
            moveDir = (spawnPos - currentPos).normalized;
            // Tính lại hướng mới ngay lập tức để không bị kẹt
            nextPos = currentPos + moveDir * speed * Time.fixedDeltaTime;
        }

        rb.MovePosition(nextPos);

        if (timer <= 0f)
        {
            isStopped = true;
            timer = stopTime;
        }

        if (moveDir.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveDir.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void Chase()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isRunning", true);

        float chaseSpeed = speed * 1.5f;
        Vector2 targetPos = Vector2.MoveTowards(rb.position, player.position, chaseSpeed * Time.fixedDeltaTime);
        rb.MovePosition(targetPos);

        Vector2 dir = (player.position - transform.position).normalized;
        if (dir.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dir.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void PickRandomDirection()
    {
        moveDir = Random.insideUnitCircle.normalized;
    }
}
