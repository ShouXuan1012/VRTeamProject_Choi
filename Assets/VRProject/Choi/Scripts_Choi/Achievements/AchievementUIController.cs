using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementUIController : MonoBehaviour
{
    [Header("UI 바인딩")]
    public Text achievementNameText;
    public Text titleText;
    public Text currentAmountText;
    public Text goalAmountText;
    public Slider progressSlider;
    public GameObject lockIcon; // 잠금 상태 아이콘 등

    private AchievementData data;

    public void Setup(AchievementData achievementData)
    {
        data = achievementData;

        achievementNameText.text = data.description;
        titleText.text = data.titleName;

        currentAmountText.text = $"{data.currentAmount}";
        goalAmountText.text = $"{data.goalAmount}";
        progressSlider.maxValue = data.goalAmount;
        progressSlider.value = data.currentAmount;

        lockIcon.SetActive(!data.isUnlocked);
    }
}
