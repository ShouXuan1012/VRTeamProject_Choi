using System.Collections.Generic;
using UnityEngine;

public class AchievementUIManager : MonoBehaviour
{
    [SerializeField] private GameObject achievementItemPrefab;
    [SerializeField] private Transform contentParent;

    private List<AchievementUIController> uiControllers = new();

    private void Start()
    {
        LoadAndGenerateAchievements();
    }

    private void LoadAndGenerateAchievements()
    {
        AchievementData[] allAchievements = Resources.LoadAll<AchievementData>("Achievements");

        foreach (var data in allAchievements)
        {
            GameObject prefab = Instantiate(achievementItemPrefab, contentParent);
            var controller = prefab.GetComponent<AchievementUIController>();
            controller.Setup(data);
            uiControllers.Add(controller);
        }
    }

    public void RefreshUI()
    {
        foreach (var controller in uiControllers)
        {
            controller.Refresh(); // 이걸 만들자
        }
    }
}
