using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AchievementUIManager : MonoBehaviour
{
    [Header("ÇÁ¸®ÆÕ & ºÎ¸ð")]
    [SerializeField] private GameObject achievementItemPrefab;
    [SerializeField] private Transform contentParent;

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
        }
    }
}
