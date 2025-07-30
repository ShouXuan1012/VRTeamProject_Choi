using UnityEngine;

[CreateAssetMenu(menuName = "Quest/QuestData")]
public class QuestData : ScriptableObject
{
    public string questTitle;
    public EQuestType questType;
    public bool isComplete;
}
