using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float Movespeed = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 lastMoveDirection = Vector2.down;
    public PlayerAttack playerAttack;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
       
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
        
        if (playerInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (playerInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        animator.SetFloat("Xinput", playerInput != Vector2.zero ? playerInput.x : lastMoveDirection.x);
        animator.SetFloat("Yinput", playerInput != Vector2.zero ? playerInput.y : lastMoveDirection.y);
        animator.SetFloat("Speed", playerInput.magnitude);
    }
    
    public void TakeDamage()
    {
        Die();
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
