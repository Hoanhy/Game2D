using UnityEngine;
using System.Collections;

public class InventoryUI : MonoBehaviour
{
    [Header("Inventory Panel")]
    public GameObject inventoryPanel; // Kéo panel chính vào đây trong Inspector
    public float animationDuration = 0.3f; // thời gian lướt
    public float slideOffsetX = 800f; // khoảng cách panel ở ngoài bên phải

    private bool isOpen = false;

    private Vector3 shownPosition; // vị trí panel khi hiển thị
    private Vector3 hiddenPosition; // vị trí panel ẩn ngoài màn hình

    void Start()
    {
        // Lưu vị trí gốc của panel để lướt về đúng chỗ
        shownPosition = inventoryPanel.transform.localPosition;

        // Tính vị trí ẩn (bên phải)
        hiddenPosition = shownPosition + new Vector3(slideOffsetX, 0, 0);

        // Bắt đầu ẩn panel hoàn toàn
        inventoryPanel.SetActive(false);
        Time.timeScale = 1f;

        // Giữ chuột luôn hiển thị khi chơi (vì nhân vật đánh theo hướng chuột)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        // Nhấn phím E để mở / đóng kho đồ
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            // Bật panel và bắt đầu slide từ bên phải
            inventoryPanel.SetActive(true);
            inventoryPanel.transform.localPosition = hiddenPosition;
            StartCoroutine(AnimateSlide(hiddenPosition, shownPosition));

            // Dừng game khi mở kho đồ
            Time.timeScale = 0f;
        }
        else
        {
            // Slide ra bên phải, sau đó ẩn panel
            StartCoroutine(AnimateSlide(shownPosition, hiddenPosition, () => inventoryPanel.SetActive(false)));

            // Tiếp tục game khi đóng kho đồ
            Time.timeScale = 1f;
        }

        // Giữ chuột hiển thị
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    IEnumerator AnimateSlide(Vector3 from, Vector3 to, System.Action onComplete = null)
    {
        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            inventoryPanel.transform.localPosition = Vector3.Lerp(from, to, elapsed / animationDuration);
            elapsed += Time.unscaledDeltaTime; // dùng unscaled để animation chạy khi Time.timeScale = 0
            yield return null;
        }
        inventoryPanel.transform.localPosition = to;
        onComplete?.Invoke();
    }
}
