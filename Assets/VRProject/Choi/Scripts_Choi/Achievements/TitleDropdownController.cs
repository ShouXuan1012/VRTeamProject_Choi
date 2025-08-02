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
        currentTitle = CurrentUserManager.Instance.CurrentUserData.titleName;

        // 초기화 후 리스너 등록(초기화에서 값 변경 시에도 OnTitleSelected가 호출되면 안되므로)
        RefreshDropdown();
        titleDropdown.onValueChanged.AddListener(OnTitleSelected);

        // 업적 완료 이벤트 리스너 등록
        AchievementManager.Instance.OnAchievementUnlocked += AddDropdown;
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

        int selectedIndex = unlockedTitles.FindIndex(a => a.titleName == currentTitle);
        titleDropdown.value = selectedIndex >= 0 ? selectedIndex : 0;
        titleDropdown.RefreshShownValue();
    }
    private void AddDropdown(AchievementData newAchievement)
    {
        if (!newAchievement.isUnlocked) return;

        // 중복 방지
        if (unlockedTitles.Exists(a => a.titleName == newAchievement.titleName)) return;

        unlockedTitles.Add(newAchievement);
        titleDropdown.options.Add(new Dropdown.OptionData(newAchievement.titleName));

        // 칭호가 1개일 경우 해당 칭호를 현재 칭호로 설정
        if (unlockedTitles.Count == 1)
        {
            currentTitle = newAchievement.titleName;
            titleDropdown.value = 0;
            titleDropdown.RefreshShownValue();
        }
    }

    private void OnTitleSelected(int index)
    {
        currentTitle = unlockedTitles[index].titleName;
        Debug.Log("선택한 칭호: " + currentTitle);

        // 데이터 반영
        AchievementManager.Instance.UpdateTitle(currentTitle);
    }
}
