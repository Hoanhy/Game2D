using UnityEngine;

// [System.Serializable] BẮT BUỘC phải có
// để JsonUtility có thể đọc/ghi lớp này.

[System.Serializable]
public class GameProgress
{
    // Thêm bất cứ thứ gì bạn muốn lưu ở đây
    public int currentHealth;
    public SerializableVector2 lastPlayerPosition;
    // public int currentGold;

    // Đây là "Hàm khởi tạo" (Constructor)
    // Nó đặt các giá trị mặc định khi người chơi "New Game"
    public GameProgress()
    {
        this.currentHealth = 6; // (Ví dụ: máu tối đa)
        this.lastPlayerPosition = new SerializableVector2(8, -5); // Vị trí bắt đầu
    }
}

// Lớp phụ để "dịch" Vector2, vì JsonUtility không hiểu Vector2
[System.Serializable]
public class SerializableVector2
{
    public float x;
    public float y;

    // Hàm chuyển từ Vector2 (mà Unity dùng)
    public SerializableVector2(Vector2 vec)
    {
        this.x = vec.x;
        this.y = vec.y;
    }

    // Hàm khởi tạo từ (float, float) (sửa lỗi trước của chúng ta)
    public SerializableVector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    // Hàm chuyển về lại Vector2
    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }
}