using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Toggle fullscreenToggle;

    private void Start()
    {
        // Slider âm nhạc
        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(SetMusicVolume);

        // Toggle fullscreen
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = Screen.fullScreen; // Đồng bộ trạng thái ban đầu
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        }
    }

    private void SetMusicVolume(float volume)
    {
        Debug.Log($"Âm lượng nhạc: {volume}");
        // TODO: Kết nối AudioMixer nếu có nhạc
    }

    private void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Debug.Log("FullScreen: " + isFullscreen);
    }
}
