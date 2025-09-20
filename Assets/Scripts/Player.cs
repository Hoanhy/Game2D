using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float Movespeed = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 lastMoveDirection = Vector2.down;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {

    }


    void FixedUpdate()
    {
        if (!animator.GetBool("isAttacking"))
        {
            MovePlayer();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    void MovePlayer()
    {
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        rb.linearVelocity = playerInput.normalized * Movespeed;

        if (playerInput != Vector2.zero)
        {
            lastMoveDirection = playerInput;
        }

        animator.SetFloat("Xinput", playerInput != Vector2.zero ? playerInput.x : lastMoveDirection.x);
        animator.SetFloat("Yinput", playerInput != Vector2.zero ? playerInput.y : lastMoveDirection.y);
        animator.SetFloat("Speed", playerInput.magnitude);

        
    }
}
