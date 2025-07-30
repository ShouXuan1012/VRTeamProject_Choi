using UnityEngine;
using UnityEngine.UI;

public class QuestSlotUI : MonoBehaviour
{
    [SerializeField] private Text questText;
    [SerializeField] private GameObject statusQuestionMark;
    [SerializeField] private GameObject statusCheckMark;

    public void SetQuest(QuestData quest)
    {
        questText.text = quest.questTitle;

        statusQuestionMark.SetActive(!quest.isComplete);
        statusCheckMark.SetActive(quest.isComplete);
    }
}
