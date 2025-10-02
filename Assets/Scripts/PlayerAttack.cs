using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public float attackCooldown = 0.3f;
    private float attackTime;
    private Vector2 lastMoveDirection;

    


    private void Start()
    {
        animator = GetComponent<Animator>();
 
    }
    void Update()
    {
        attackTime -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && attackTime <= 0 && !animator.GetBool("isAttacking"))
        {
            animator.SetBool("isAttacking", true);
            rb.linearVelocity = Vector2.zero;
            attackTime = attackCooldown;
   
           
        }

        lastMoveDirection = new Vector2(animator.GetFloat("Xinput"), animator.GetFloat("Yinput"));
    
    }
    
   
    public void ResetAttack()
    {
        animator.SetBool("isAttacking", false);
    }
}
