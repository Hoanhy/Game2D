using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Thuộc tính di chuyển")]
    public float speed = 2f;          // tốc độ di chuyển
    public float moveTime = 3f;       // thời gian đi 1 hướng
    public float stopTime = 1f;       // thời gian đứng yên

    [Header("Phát hiện Player")]
    public float chaseRange = 5f;     // phạm vi đuổi
    public float attackRange = 1f;    // phạm vi tấn công
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

        // ✅ kiểm tra state theo khoảng cách
        if (distance <= attackRange)
            currentState = State.Attack;
        else if (distance <= chaseRange)
            currentState = State.Chase;
        else
            currentState = State.Patrol;

        // Gọi hành vi theo state
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack(distance); // ✅ truyền distance vào Attack
                break;
        }
    }

    void Patrol()
    {
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

        // lật hình
        transform.localScale = new Vector3(movingRight ? 5 : -5, 5, 5);
    }

    void Chase()
    {
        animator.SetBool("isRunning", true);

        Vector2 dir = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        // lật hình theo hướng player
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(5, 5, 5);
        else
            transform.localScale = new Vector3(-5, 5, 5);
    }

    void Attack(float distance)
    {
        //nếu player chạy xa hơn attackRange -> thoát Attack
        if (distance > attackRange)
            return;

        animator.SetBool("isRunning", false);
        animator.SetTrigger("attack");

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("Monster Attack Player!");
            // player.TakeDamage(damage);
            lastAttackTime = Time.time;
        }
    }
}
