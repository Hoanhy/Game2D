using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour
{
 
    public Animator animator;
    public float swingCooldown = 0.3f;
    private bool swingBlock;
    private SpriteRenderer spriteRenderer;
    private Collider2D hitbox;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer.enabled = false;
        if (hitbox != null) hitbox.enabled = false;
    }


    void Update()
    {
        Swing();
    }
    
    public void Swing()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (swingBlock)
                return;
            animator.SetTrigger("Swing");
            spriteRenderer.enabled = true;
            if (hitbox != null) hitbox.enabled = true;
            
            swingBlock = true;
            StartCoroutine(SwingCooldown());
        }
            
    }
    private IEnumerator SwingCooldown()
    {
        yield return new WaitForSeconds(swingCooldown);
        spriteRenderer.enabled = false;
        if (hitbox != null) hitbox.enabled = false;
        swingBlock = false;
    }
}
