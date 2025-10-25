using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public float attackCooldown = 0.3f;
    private float attackTime;
    private Vector2 lastMoveDirection;




    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        attackTime -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && attackTime <= 0 && !animator.GetBool("isAttacking"))
        {
            animator.SetBool("isAttacking", true);
            rb.linearVelocity = Vector2.zero;
            attackTime = attackCooldown;
            StartCoroutine(AttackTowardsMouse());

        }

        lastMoveDirection = new Vector2(animator.GetFloat("Xinput"), animator.GetFloat("Yinput"));

    }

    IEnumerator AttackTowardsMouse()
    {

        animator.SetBool("isAttacking", true);
        rb.linearVelocity = Vector2.zero;

        // Lấy hướng con trỏ chuột
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector2 attackDir = (mousePos - transform.position).normalized;

        // Đặt hướng animation
        SetAttackDirection(attackDir);

        // Thời gian tấn công
        yield return new WaitForSeconds(attackCooldown);

        animator.SetBool("isAttacking", false);

    }

    void SetAttackDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            // Tấn công trái/phải
            animator.Play("Attack_side");
            spriteRenderer.flipX = dir.x < 0;
        }
        else
        {
            if (dir.y > 0)
                animator.Play("Attack_up");
            else
                animator.Play("Attack_side");
        }
    }

    public void ResetAttack()
    {
        animator.SetBool("isAttacking", false);
    }
}
