using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float Movespeed = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
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
        MovePlayer();
    }
    void MovePlayer()
    {
        float ho = Input.GetAxisRaw("Horizontal");
        float ve = Input.GetAxisRaw("Vertical");
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.linearVelocity = playerInput.normalized * Movespeed;
       
        animator.SetFloat("Xinput", ho);
        animator.SetFloat("Yinput", ve);
    }
}
