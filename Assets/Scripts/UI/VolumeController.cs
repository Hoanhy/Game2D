using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();

        // 1. Cập nhật vị trí Slider theo âm lượng hiện tại
        if (AudioManager.Instance != null)
        {
            slider.value = AudioManager.Instance.musicVolume;
        }

        // 2. Đăng ký sự kiện: Khi kéo slider -> Gọi hàm OnSliderChanged
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    // Hàm này chạy mỗi khi bạn kéo thanh trượt
    void OnSliderChanged(float val)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetVolume(val);
        }
    }
}