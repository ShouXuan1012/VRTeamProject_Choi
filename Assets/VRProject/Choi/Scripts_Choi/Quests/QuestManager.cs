using System.Collections.Generic;
using UnityEngine;


public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<QuestData> quests;
    [SerializeField] private QuestUIManager uiManager;

    private void OnEnable()
    {
        foreach (var quest in quests)
        {
            QuestEvents.Subscribe(quest.questType, () => TryComplete(quest.questType));
        }
    }

    private void OnDisable()
    {
        foreach (var quest in quests)
        {
            QuestEvents.Unsubscribe(quest.questType, () => TryComplete(quest.questType));
        }
    }

    private void TryComplete(EQuestType questType)
    {
        var quest = quests.Find(q => q.questType == questType);
        if (quest != null && !quest.isComplete)
        {
            quest.isComplete = true;
            Debug.Log($"[퀘스트 완료] {quest.questTitle}");
            uiManager?.RefreshUI();

            AchievementManager.Instance.AddProgress(EAchievementType.AllQuestsCompleted, 1);
        }
    }

    public List<QuestData> GetAllQuests() => quests;
}
