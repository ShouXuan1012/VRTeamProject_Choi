using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<QuestData> quests;
    [SerializeField] private QuestUIManager uiManager;

    private void OnEnable()
    {
        QuestEvents.OnBusBoarded += HandleBusBoarded;
        QuestEvents.OnFoodPurchased += HandleFoodPurchased;
        QuestEvents.OnPhotoTaken += HandlePhotoTaken;
        QuestEvents.OnMuseumEntered += HandleMuseumEntered;
        QuestEvents.OnBuskingDonated += HandleBuskingDonated;
        QuestEvents.OnBasketballScored10 += HandleBasketballScored10;
    }

    private void OnDisable()
    {
        QuestEvents.OnBusBoarded -= HandleBusBoarded;
        QuestEvents.OnFoodPurchased -= HandleFoodPurchased;
        QuestEvents.OnPhotoTaken -= HandlePhotoTaken;
        QuestEvents.OnMuseumEntered -= HandleMuseumEntered;
        QuestEvents.OnBuskingDonated -= HandleBuskingDonated;
        QuestEvents.OnBasketballScored10 -= HandleBasketballScored10;
    }

    private void HandleBusBoarded() => TryComplete("버스 탑승해보기");
    private void HandleFoodPurchased() => TryComplete("시장에서 먹을 것 사보기");
    private void HandlePhotoTaken() => TryComplete("포토존에서 사진 찍어보기");
    private void HandleMuseumEntered() => TryComplete("박물관에 들어가서 관람하기");
    private void HandleBuskingDonated() => TryComplete("버스킹 후원하기");
    private void HandleBasketballScored10() => TryComplete("농구게임 10점 이상 달성");

    private void TryComplete(string questTitle)
    {
        Debug.Log($"TryComplete 호출됨: {questTitle}");
        QuestData quest = quests.Find(q => q.questTitle == questTitle);        
        if (quest != null && !quest.isComplete)
        {
            quest.isComplete = true;
            Debug.Log($"[퀘스트 완료] {quest.questTitle}");
            uiManager?.RefreshUI(); // UI 갱신
        }
    }

    public List<QuestData> GetAllQuests() => quests;
}
