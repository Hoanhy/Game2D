using UnityEngine;
using TMPro;
using System.Collections;

public class PopupText : MonoBehaviour
{
    public float moveSpeed = 1f; // Tốc độ bay lên
    public float fadeTime = 1.5f;  // Thời gian biến mất

    private TextMeshPro textMesh;
    private Color originalColor;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        originalColor = textMesh.color;
    }

    void Start()
    {
        // Tự động bắt đầu hiệu ứng khi được sinh ra
        StartCoroutine(FadeAndFloat());
    }

    private IEnumerator FadeAndFloat()
    {
        float timer = 0;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;

            // 1. Bay lên từ từ
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;

            // 2. Mờ dần (Giảm Alpha)
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeTime);
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null;
        }

        // 3. Biến mất hoàn toàn thì tự hủy
        Destroy(gameObject);
    }
}