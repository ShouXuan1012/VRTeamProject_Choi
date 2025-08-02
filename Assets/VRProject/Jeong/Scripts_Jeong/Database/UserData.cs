using Firebase.Firestore;
using System.Collections.Generic;
using UnityEngine;

[FirestoreData]
public class UserData
{
    [FirestoreProperty]
    public string userId { get; set; }

    [FirestoreProperty]
    public string password { get; set; }

    [FirestoreProperty]
    public string nickname { get; set; }

    [FirestoreProperty]
    public string avatar { get; set; }

    [FirestoreProperty]
    public string profileImage { get; set; }

    [FirestoreProperty]
    public int coin { get; set; }

    [FirestoreProperty]
    public List<QuestDataForDB> questProgresses { get; set; }

    [FirestoreProperty]
    public List<AchievementDataForDB> achievementProgresses { get; set; }

    [FirestoreProperty]
    public bool isOnline { get; set; }

    [FirestoreProperty]
    public Timestamp signUpDate { get; set; }
}

[FirestoreData]
public class QuestDataForDB
{
    [FirestoreProperty]
    public string questTitle { get; set; }

    [FirestoreProperty]
    public EQuestType questType { get; set; }

    [FirestoreProperty]
    public bool isComplete { get; set; }
}

[FirestoreData]
public class AchievementDataForDB
{
    [FirestoreProperty]
    public EAchievementType achievementType { get; set; }

    [FirestoreProperty]
    public string titleName { get; set; }

    [FirestoreProperty]
    public string description { get; set; }

    [FirestoreProperty]
    public int goalAmount { get; set; }

    [FirestoreProperty]
    public int currentAmount { get; set; }

    [FirestoreProperty]
    public bool isUnlocked { get; set; }
}