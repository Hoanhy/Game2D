using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHPUI : MonoBehaviour
{
    public Image heartIcon;
    public TextMeshProUGUI hpText;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    public void UpdateHeartUI(int currentHP, int maxHP)
    {
        int maxHearts = maxHP / 2; // tổng số tim

        // Số tim còn nguyên (ceil để nửa tim vẫn được tính trong icon, nhưng text giữ nguyên tim đầy)
        int fullHearts = Mathf.CeilToInt(currentHP / 2f);

        // Text hiển thị số tim (không giảm khi còn nửa tim)
        hpText.text = "x" + fullHearts.ToString();

        // Icon hiển thị full / half / empty
        if (currentHP <= 0)
            heartIcon.sprite = emptyHeart;
        else if (currentHP % 2 == 1)
            heartIcon.sprite = halfHeart;
        else
            heartIcon.sprite = fullHeart;
    }
}
