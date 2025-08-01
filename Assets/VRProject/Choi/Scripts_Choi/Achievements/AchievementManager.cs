using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    [Header("모든 업적 데이터")]
    [SerializeField] private List<AchievementData> achievements;

    private void Awake()
    {
        // 싱글톤 중복 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시 유지 (필요시)
    }

    /// <summary>
    /// 업적 조건 진행도 증가
    /// </summary>
    public void AddProgress(EAchievementType type, int amount)
    {
        AchievementData data = achievements.Find(a => a.achievementType == type);
        if (data == null || data.isUnlocked) return;

        data.currentAmount += amount;
        var uiManager = FindObjectOfType<AchievementUIManager>();
        uiManager?.RefreshUI();
        if (data.currentAmount >= data.goalAmount)
        {
            data.isUnlocked = true;
            Debug.Log($"칭호 획득: <color=yellow>{data.titleName}</color>");
            uiManager?.RefreshUI();
            // TODO: UI 연동, Firestore 저장 등
        }
    }

    /// <summary>
    /// 현재 업적 데이터를 외부에서 읽을 수 있도록 제공
    /// </summary>
    public List<AchievementData> GetAllAchievements()
    {
        return achievements;
    }

    /// <summary>
    /// 업적을 강제로 잠금 해제 (디버그용)
    /// </summary>
    public void UnlockAchievement(EAchievementType type)
    {
        AchievementData data = achievements.Find(a => a.achievementType == type);
        if (data != null && !data.isUnlocked)
        {
            data.currentAmount = data.goalAmount;
            data.isUnlocked = true;
            Debug.Log($"[디버그] {data.titleName} 강제 해금");
        }
    }
}
