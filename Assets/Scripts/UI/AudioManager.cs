using UnityEngine;
using UnityEngine.SceneManagement; // Cần để biết đang ở màn nào

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music Clips")]
    public AudioClip menuTheme;     // Nhạc nền Menu
    public AudioClip gameplayTheme; // Nhạc nền Gameplay

    [Header("Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.5f;

    private AudioSource musicSource;

    private void Awake()
    {
        // 1. Setup Singleton (Giống SaveManager)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ cho nhạc sống qua màn

            // Tự tạo AudioSource nếu chưa có
            musicSource = GetComponent<AudioSource>();
            if (musicSource == null) musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            // Lấy giá trị đã lưu, nếu chưa có thì mặc định là 0.5
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            musicSource.volume = musicVolume;
        }
        else
        {
            Destroy(gameObject); // Hủy bản sao
        }
    }

    // 2. Đăng ký sự kiện chuyển màn
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 3. Hàm này chạy mỗi khi vào màn mới
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Kiểm tra tên màn chơi để chọn nhạc
        if (scene.name == "MenuGame") // <-- Thay đúng tên Scene Menu của bạn
        {
            PlayMusic(menuTheme);
        }
        else if (scene.name == "Game") // <-- Thay đúng tên Scene Gameplay của bạn
        {
            PlayMusic(gameplayTheme);
        }
    }

    // 4. Hàm phát nhạc thông minh (Không phát lại nếu đang chạy bài đó rồi)
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        // Nếu đang phát đúng bài này rồi thì thôi (để nhạc không bị ngắt đoạn)
        if (musicSource.clip == clip && musicSource.isPlaying)
        {
            return;
        }

        // Nếu khác bài, đổi bài và phát
        musicSource.clip = clip;
        musicSource.Play();
    }

    // Hàm chỉnh âm lượng (để làm menu Settings sau này)
    public void SetVolume(float volume)
    {
        // Cập nhật biến để lưu trữ
        musicVolume = volume;

        // Cập nhật ngay lập tức cho nhạc đang phát
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }

        // (Tùy chọn) Lưu lại cài đặt để lần sau vào game vẫn nhớ
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }
}