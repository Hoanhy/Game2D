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

    private enum State { Patrol, Chase, Attack }
    private State currentState = State.Patrol;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyAttack = GetComponent<EnemyAttack>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        spawnPos = transform.position;
        timer = Random.Range(1f, moveTime);
        isStopped = Random.value > 0.5f;
        PickRandomDirection();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
            currentState = State.Attack;
        else if (distance <= chaseRange)
            currentState = State.Chase;
        else
            currentState = State.Patrol;

        switch (currentState)
        {
            case State.Patrol: Patrol(); break;
            case State.Chase: Chase(); break;
            case State.Attack: enemyAttack.DoAttack(player, attackRange); break;
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
                isStopped = false;
                timer = moveTime;
                PickRandomDirection();
            }
            return;
        }

        animator.SetBool("isRunning", true);

        timer -= Time.deltaTime;
        Vector2 nextPos = (Vector2)transform.position + moveDir * speed * Time.deltaTime;

        if (Vector2.Distance(nextPos, spawnPos) > patrolRadius)
        {
            moveDir = (spawnPos - (Vector2)transform.position).normalized;
        }

        transform.position = nextPos;

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
        Vector2 dir = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

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
