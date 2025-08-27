using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;          // tốc độ di chuyển
    public float moveTime = 3f;       // thời gian đi 1 hướng
    public float stopTime = 1f;       // thời gian đứng yên

    private float timer;
    private bool movingRight = true;
    private bool isStopped = false;
    private Animator animator;

    void Start()
    {
        timer = moveTime;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isStopped)
        {
            // đang dừng thì chuyển Idle
            if (animator != null)
                animator.SetBool("isRunning", false);

            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                // hết dừng → quay đầu và đi tiếp
                movingRight = !movingRight;
                isStopped = false;
                timer = moveTime;
            }
            return;
        }

        // đang di chuyển thì bật Run
        if (animator != null)
            animator.SetBool("isRunning", true);

        timer -= Time.deltaTime;
        if (movingRight)
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        else
            transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (timer <= 0f)
        {
            // khi hết thời gian di chuyển thì dừng lại
            isStopped = true;
            timer = stopTime;
        }

        // lật hình theo hướng
        transform.localScale = new Vector3(movingRight ? 5 : -5, 5, 5);
    }
}
