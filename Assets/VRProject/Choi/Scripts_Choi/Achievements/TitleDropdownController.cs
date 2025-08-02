using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleDropdownController : MonoBehaviour
{
    [SerializeField] private Dropdown titleDropdown;
    private string currentTitle;
    public string CurrentTitle => currentTitle;

    private List<AchievementData> unlockedTitles;

    private void Start()
    {
        titleDropdown.onValueChanged.AddListener(OnTitleSelected);
        RefreshDropdown();

        // 업적 완료 이벤트 리스너 등록
        AchievementManager.Instance.OnAchievementUnlocked += RefreshDropdown;
    }

    private void RefreshDropdown()
    {
        unlockedTitles = AchievementManager.Instance
            .GetAllAchievements()
            .FindAll(a => a.isUnlocked);

        List<string> options = new List<string>();
        foreach (var achievement in unlockedTitles)
        {
            options.Add(achievement.titleName);
        }

        titleDropdown.ClearOptions();
        titleDropdown.AddOptions(options);
    }

    private async void OnTitleSelected(int index)
    {
        currentTitle = unlockedTitles[index].titleName;
        Debug.Log("선택한 칭호: " + currentTitle);

        // DB 반영
        CurrentUserManager.Instance.SetTitleName(currentTitle);
        await CurrentUserManager.Instance.UpdateTitleName(currentTitle);
    }
}
