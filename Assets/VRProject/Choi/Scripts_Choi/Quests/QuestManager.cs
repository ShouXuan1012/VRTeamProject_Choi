using System.Collections.Generic;
using UnityEngine;


public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<QuestData> quests;
    [SerializeField] private QuestUIManager uiManager;

    private List<QuestDataForDB> questProgresses;

    private void OnEnable()
    {
        foreach (var quest in quests)
        {
            QuestEvents.Subscribe(quest.questType, () => TryComplete(quest.questType));
        }

        questProgresses = CurrentUserManager.Instance.CurrentUserData.questProgresses;
        if (questProgresses == null)
        {
            questProgresses = new List<QuestDataForDB>();
            CurrentUserManager.Instance.SetQuestProgresses(questProgresses);
        }

        foreach (var quest in quests)
        {
            var progress = questProgresses.Find(qp => qp.questType == quest.questType);
            if (progress != null)
            {
                quest.isComplete = progress.isComplete;
            }
            else
            {
                quest.isComplete = false;

                // 퀘스트 진행 정보가 없으면 추가
                QuestDataForDB newQuestData = new QuestDataForDB
                {
                    questTitle = quest.questTitle,
                    questType = quest.questType,
                    isComplete = quest.isComplete
                };
                questProgresses.Add(newQuestData);
            }
        }
    }

    private void OnDisable()
    {
        foreach (var quest in quests)
        {
            QuestEvents.Unsubscribe(quest.questType, () => TryComplete(quest.questType));
        }
    }

    private async void TryComplete(EQuestType questType)
    {
        var quest = quests.Find(q => q.questType == questType);
        if (quest != null && !quest.isComplete)
        {
            quest.isComplete = true;
            Debug.Log($"[퀘스트 완료] {quest.questTitle}");
            uiManager?.RefreshUI();

            // 유저 데이터 업데이트
            var progress = questProgresses.Find(qp => qp.questType == questType);
            if (progress != null)
            {
                progress.isComplete = true;
            }

            CurrentUserManager.Instance.SetQuestProgresses(questProgresses);
            await CurrentUserManager.Instance.UpdateQuestProgresses(questProgresses);

            await AchievementManager.Instance.AddProgress(EAchievementType.AllQuestsCompleted, 1);
        }
    }
}
