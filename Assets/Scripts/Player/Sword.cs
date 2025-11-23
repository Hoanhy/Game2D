using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour
{
    public Animator animator;
    public float swingCooldown = 0.3f;
    private bool swingBlock;
    private SpriteRenderer spriteRenderer;
    private Collider2D hitbox;
    public float rotateOffset = 0f;
    private float lockedAngle;
    private Camera cam;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer.enabled = false;
        cam = Camera.main;
        if (hitbox != null) hitbox.enabled = false;
    }

    void Update()
    {
        // SỬA: Luôn xoay theo chuột nếu KHÔNG đang chém
        if (!swingBlock)
        {
            RotateTowardsMouse();
        }

        // Kiểm tra click chuột để chém
        Swing();
    }

    void RotateTowardsMouse()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        Vector3 dir = mouseWorld - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + rotateOffset);

        // Logic lật hình để không bị ngược khi xoay sang trái
        if (angle > 90 || angle < -90)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
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
            lockedAngle = transform.rotation.eulerAngles.z;

            // XÓA: Không cần gọi RotateTowardsMouse() ở đây nữa 
            // vì Update đã làm việc đó liên tục rồi.

            StartCoroutine(SwingCooldown());
        }
    }

    private IEnumerator SwingCooldown()
    {
        lockedAngle = transform.eulerAngles.z;
        float timer = 0f;
        while (timer < swingCooldown)
        {
            timer += Time.deltaTime;
            // Giữ chặt góc quay (khóa cứng) trong khi chém
            transform.rotation = Quaternion.Euler(0, 0, lockedAngle);
            yield return null;
        }

        spriteRenderer.enabled = false;
        if (hitbox != null) hitbox.enabled = false;
        swingBlock = false; // Mở khóa để Update tiếp tục xoay theo chuột
    }
}