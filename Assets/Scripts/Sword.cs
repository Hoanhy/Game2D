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
        Swing();

    }
    void RotateTowardsMouse()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        Vector3 dir = mouseWorld - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotateOffset);
        if (angle > 90 || angle < -90)
        {
            // Lật sprite theo chiều dọc
            spriteRenderer.flipY = true;
        }
        else
        {
            // Nếu ở bên phải, không lật
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
            RotateTowardsMouse();
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
            transform.rotation = Quaternion.Euler(0, 0, lockedAngle);
            yield return null;
        }
        spriteRenderer.enabled = false;
        if (hitbox != null) hitbox.enabled = false;
        swingBlock = false;

    }
}
