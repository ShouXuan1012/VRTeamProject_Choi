using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/AchievementData")]
public class AchievementData : ScriptableObject
{
    public EAchievementType achievementType;

    [Header("칭호 UI")]
    public string titleName;       // 예: "잭팟"
    [TextArea]
    public string description;     // 예: "카지노에서 한번에 1000만원 벌기"

    [Header("진행 조건")]
    public int goalAmount;         // 예: 1000000
    [HideInInspector]
    public int currentAmount;      // 세이브할 경우
    [HideInInspector]
    public bool isUnlocked;
}
