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
        int fullHearts = currentHP / 2;
        bool hasHalf = currentHP % 2 != 0;

        hpText.text = "x" + fullHearts.ToString();

        if (currentHP <= 0) heartIcon.sprite = emptyHeart;
        else if (hasHalf) heartIcon.sprite = halfHeart;
        else heartIcon.sprite = fullHeart;
    }
}
