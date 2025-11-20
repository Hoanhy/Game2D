using System.Collections.Generic;
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
    // danh sách kẻ thù
    public List<EnemyData> enemies = new List<EnemyData>();
    public List<string> finishedSpawnerIDs = new List<string>();
    public List<string> deadEnemyIDs = new List<string>();
    public List<InventoryData> inventory = new List<InventoryData>();
    // Đây là "Hàm khởi tạo" (Constructor)
    // Nó đặt các giá trị mặc định khi người chơi "New Game"
    public GameProgress()
    {
        this.currentHealth = 6; // (Ví dụ: máu tối đa)
        this.lastPlayerPosition = new SerializableVector2(8, -5); // Vị trí bắt đầu
    }
}

// Lớp phụ để "dịch" Vector2, vì JsonUtility không hiểu Vector2

public class EnemyData
{
    public string enemyID; // ID riêng biệt (ví dụ: "Slime_1")
    public SerializableVector2 position;
    public int health;
    public bool isDead;

    // Hàm khởi tạo nhanh
    public EnemyData(string id, Vector2 pos, int hp, bool dead)
    {
        this.enemyID = id;
        this.position = new SerializableVector2(pos);
        this.health = hp;
        this.isDead = dead;
    }
}
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
[System.Serializable]
public class InventoryData
{
    public string itemName;
    public int quantity;

    public InventoryData(string name, int qty)
    {
        this.itemName = name;
        this.quantity = qty;
    }
}