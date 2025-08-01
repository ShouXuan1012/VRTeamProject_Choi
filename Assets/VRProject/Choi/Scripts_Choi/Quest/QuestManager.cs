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
            Debug.Log($"[Äù½ºÆ® ¿Ï·á] {quest.questTitle}");
            uiManager?.RefreshUI();
        }
    }

    private void TryComplete(string questTitle)
    {
        Debug.Log($"TryComplete È£ÃâµÊ: {questTitle}");
        QuestData quest = quests.Find(q => q.questTitle == questTitle);
        if (quest != null && !quest.isComplete)
        {
            quest.isComplete = true;
            Debug.Log($"[Äù½ºÆ® ¿Ï·á] {quest.questTitle}");
            uiManager?.RefreshUI(); // UI °»½Å
        }
    }

    public List<QuestData> GetAllQuests() => quests;
}
