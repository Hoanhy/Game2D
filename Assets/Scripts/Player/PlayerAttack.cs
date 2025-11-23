using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [Header("Weapon Objects")]
    public GameObject swordObject; // Kéo GameObject Sword (con của Player) vào đây
    public GameObject axeObject;   // Kéo GameObject Axe (con của Player) vào đây

    [Header("Attack Stats")]
    public float defaultAttackCooldown = 0.3f; // Cooldown của Kiếm
    public float axeAttackCooldown = 0.8f;     // Cooldown của Rìu (chậm hơn)

    private float currentAttackCooldown;       // Cooldown hiện tại đang dùng
    private float attackTime;
    private Vector2 lastMoveDirection;

    // Biến kiểm tra đang cầm gì
    private bool hasAxe = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Khởi tạo mặc định: Cầm Kiếm
        currentAttackCooldown = defaultAttackCooldown;
        EquipSword();
    }

    void Update()
    {
        attackTime -= Time.deltaTime;

        // SỬA: Dùng 'currentAttackCooldown' thay vì biến cố định
        if (Input.GetMouseButtonDown(0) && attackTime <= 0 && !animator.GetBool("isAttacking"))
        {
            // Nếu đang cầm Rìu, gọi hàm chém của Rìu (script Sword trên cây Rìu)
            if (hasAxe && axeObject != null)
            {
                // Gọi script tấn công riêng của Rìu (nếu bạn dùng script Sword.cs cho cả 2)
                Sword axeScript = axeObject.GetComponent<Sword>();
                if (axeScript != null) axeScript.Swing();

                // Đặt thời gian hồi chiêu
                attackTime = currentAttackCooldown;

                // Cần set bool này để Player đứng yên (nếu logic di chuyển check nó)
                animator.SetBool("isAttacking", true);
                rb.linearVelocity = Vector2.zero;

                // Coroutine để reset trạng thái isAttacking sau khi chém xong
                StartCoroutine(AttackTowardsMouse());
            }
            // Nếu đang cầm Kiếm
            else if (!hasAxe && swordObject != null)
            {
                Sword swordScript = swordObject.GetComponent<Sword>();
                if (swordScript != null) swordScript.Swing();

                attackTime = currentAttackCooldown;
                animator.SetBool("isAttacking", true);
                rb.linearVelocity = Vector2.zero;
                StartCoroutine(AttackTowardsMouse());
            }
           
        }

        lastMoveDirection = new Vector2(animator.GetFloat("Xinput"), animator.GetFloat("Yinput"));
    }

    // --- HÀM MỚI: ĐỔI VŨ KHÍ SANG RÌU ---
    public void EquipAxe()
    {
        hasAxe = true;

        // 1. Bật/Tắt hiển thị
        if (swordObject != null) swordObject.SetActive(false);
        if (axeObject != null) axeObject.SetActive(true);

        // 2. Thay đổi chỉ số (Cooldown lâu hơn)
        currentAttackCooldown = axeAttackCooldown;

        Debug.Log("Đã trang bị Rìu! Cooldown hiện tại: " + currentAttackCooldown);
    }

    // Hàm trang bị kiếm (mặc định)
    public void EquipSword()
    {
        hasAxe = false;
        if (swordObject != null) swordObject.SetActive(true);
        if (axeObject != null) axeObject.SetActive(false);
        currentAttackCooldown = defaultAttackCooldown;
    }

    // Coroutine phụ để reset trạng thái đứng yên
    

    // ... (Giữ nguyên các hàm AttackTowardsMouse và SetAttackDirection cũ để phòng hờ) ...
    IEnumerator AttackTowardsMouse()
    {
        animator.SetBool("isAttacking", true);
        rb.linearVelocity = Vector2.zero;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 attackDir = (mousePos - transform.position).normalized;
        SetAttackDirection(attackDir);
        yield return new WaitForSeconds(currentAttackCooldown); // Sửa thành current
        animator.SetBool("isAttacking", false);
    }

    void SetAttackDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            animator.Play("Attack_side");
            spriteRenderer.flipX = dir.x < 0;
        }
        else
        {
            if (dir.y > 0) animator.Play("Attack_up");
            else animator.Play("Attack_side");
        }
    }
    public void ResetAttack()
    {
        animator.SetBool("isAttacking", false);
    }
}