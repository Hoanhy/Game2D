using UnityEngine;

public class EnemySlime : MonoBehaviour
{
    [Header("Chỉ số cơ bản")]
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private float speed = 1f;

    [Header("Di chuyển")]
    [SerializeField] private float moveTime = 2f;
    [SerializeField] private float stopTime = 1f;

    [Header("Chase Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float chaseRange = 4f;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool movingRight = true;
    private bool isStopped = false;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = moveTime;
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Xác định hướng di chuyển
        if (player != null && Vector2.Distance(transform.position, player.position) < chaseRange)
        {
            movement = ((Vector2)player.position - rb.position).normalized * speed;
        }
        else
        {
            if (isStopped)
            {
                movement = Vector2.zero;
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    movingRight = !movingRight;
                    isStopped = false;
                    timer = moveTime;
                }
            }
            else
            {
                movement = (movingRight ? Vector2.right : Vector2.left) * speed;
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    isStopped = true;
                    timer = stopTime;
                }
            }
        }

        // Cập nhật hướng sprite
        if (movement.x != 0)
            transform.localScale = new Vector3(movement.x > 0 ? 1 : -1, 1, 1);

        // Cập nhật animator dựa trên movement
        if (animator != null)
            animator.SetFloat("Speed", movement.magnitude);
    }

    void FixedUpdate()
    {
        // Di chuyển vật lý
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
    }
}
