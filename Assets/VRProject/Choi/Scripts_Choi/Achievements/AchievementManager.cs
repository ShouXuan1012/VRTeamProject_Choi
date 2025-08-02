using Photon.Pun;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    // 업적 완료 이벤트
    public event System.Action<AchievementData> OnAchievementUnlocked;

    public static AchievementManager Instance { get; private set; }

    [Header("모든 업적 데이터")]
    [SerializeField] private List<AchievementData> achievements;

    private List<AchievementDataForDB> achievementProgresses;

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

    private void Start()
    {
        // DB 값으로 초기화
        achievementProgresses = CurrentUserManager.Instance.CurrentUserData.achievementProgresses;
        if (achievementProgresses == null)
        {
            achievementProgresses = new List<AchievementDataForDB>();
            CurrentUserManager.Instance.SetAchievementProgresses(achievementProgresses);
        }

        foreach (var achievement in achievements)
        {
            var progress = achievementProgresses.Find(a => a.achievementType == achievement.achievementType);
            if (progress != null)
            {
                achievement.currentAmount = progress.currentAmount;
                achievement.isUnlocked = progress.isUnlocked;
            }
            else
            {
                achievement.currentAmount = 0;
                achievement.isUnlocked = false;

                // 업적 진행 정보가 없으면 새로 추가
                AchievementDataForDB newAchievementData = new AchievementDataForDB
                {
                    achievementType = achievement.achievementType,
                    titleName = achievement.titleName,
                    description = achievement.description,
                    goalAmount = achievement.goalAmount,
                    currentAmount = achievement.currentAmount,
                    isUnlocked = achievement.isUnlocked
                };
                achievementProgresses.Add(newAchievementData);
            }
        }
    }

    /// <summary>
    /// 업적 조건 진행도 증가
    /// </summary>
    public async void AddProgress(EAchievementType type, int amount)
    {
        AchievementData data = achievements.Find(a => a.achievementType == type);
        if (data == null || data.isUnlocked) return;

        data.currentAmount += amount;
        var uiManager = FindObjectOfType<AchievementUIManager>();
        uiManager?.RefreshUI();

        // 유저 데이터 업데이트
        var progress = achievementProgresses.Find(a => a.achievementType == type);
        if (progress != null)
        {
            progress.currentAmount = data.currentAmount;
        }

        if (data.currentAmount >= data.goalAmount)
        {
            data.isUnlocked = true;
            Debug.Log($"칭호 획득: <color=yellow>{data.titleName}</color>");
            uiManager?.RefreshUI();

            // 유저 데이터 업데이트
            if (progress != null)
            {
                progress.isUnlocked = true;
            }

            // 완료된 업적이 하나만 있다면 칭호 업데이트
            if (achievementProgresses.FindAll(a => a.isUnlocked).Count == 1)
            {
                UpdateTitle(data.titleName);
            }

            OnAchievementUnlocked?.Invoke(data);
        }

        CurrentUserManager.Instance.SetAchievementProgresses(achievementProgresses);
        await CurrentUserManager.Instance.UpdateAchievementProgresses(achievementProgresses);
    }

    public async void UpdateTitle(string titleName)
    {
        CurrentUserManager.Instance.SetTitleName(titleName);
        await CurrentUserManager.Instance.UpdateTitleName(titleName);

        // 포톤 유저 정보 업데이트
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
        {
            { "TitleName", titleName }
        });
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
