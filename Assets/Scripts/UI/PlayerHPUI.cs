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

    public int maxHP = 6;
    public int currentHP = 6;

    void Update()
    {
        UpdateHeartUI();
    }

    void UpdateHeartUI()
    {
        int hearts = Mathf.CeilToInt(maxHP / 2f);
        int fullHearts = currentHP / 2;
        bool hasHalf = currentHP % 2 != 0;

        hpText.text = "x" + fullHearts.ToString();

        if (currentHP <= 0) heartIcon.sprite = emptyHeart;
        else if (hasHalf) heartIcon.sprite = halfHeart;
        else heartIcon.sprite = fullHeart;
    }
}
