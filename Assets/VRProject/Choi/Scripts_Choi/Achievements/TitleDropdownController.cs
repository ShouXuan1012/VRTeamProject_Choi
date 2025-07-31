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

    private void OnTitleSelected(int index)
    {
        currentTitle = unlockedTitles[index].titleName;
        Debug.Log("선택한 칭호: " + currentTitle);

        // TODO: 이 값을 플레이어 정보에 반영하거나 저장
    }
}
