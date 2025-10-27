using UnityEngine;

using UnityEngine.InputSystem;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private float Movespeed = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 lastMoveDirection = Vector2.down;
    public PlayerAttack playerAttack;

    [SerializeField] private float dashSpeed = 15f;     
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.5f;

    private bool isDashing = false; 
    private bool canDash = true;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
       
    }

    private void Update()
    {
        // Nếu nhấn phím Jump (Space), có thể lướt, VÀ không đang tấn công
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
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

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        animator.SetBool("isDashing", true); // Tùy chọn: Thêm param "isDashing" trong Animator

        // Lấy hướng di chuyển hiện tại, nếu đứng yên thì dùng hướng cuối cùng
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 dashDirection = playerInput.normalized;
        if (dashDirection == Vector2.zero)
        {
            dashDirection = lastMoveDirection.normalized;
        }

        // Đặt vận tốc lướt
        rb.linearVelocity = dashDirection * dashSpeed;

        // Chờ hết thời gian lướt
        yield return new WaitForSeconds(dashDuration);

        // Kết thúc lướt
        isDashing = false;
        animator.SetBool("isDashing", false);
        rb.linearVelocity = Vector2.zero; // Dừng người chơi lại ngay lập tức

        // Chờ hết thời gian hồi chiêu
        yield return new WaitForSeconds(dashCooldown);

        canDash = true; // Cho phép lướt trở lại
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
