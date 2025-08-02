using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;

public class UserDataManager
{
    private static UserDataManager _instance;
    public static UserDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UserDataManager();
            }
            return _instance;
        }
    }

    private FirestoreDBManager dbManager;

    private UserDataManager()
    {
        dbManager = FirestoreDBManager.Instance;
    }

    // 컬렉션 경로
    private const string collectionPath = "users";

    // 필드 경로
    private const string passwordPath = "password";
    private const string nicknamePath = "nickname";
    private const string avatarPath = "avatar";
    private const string profileImagePath = "profileImage";
    private const string coinPath = "coin";
    private const string questProgressesPath = "questProgresses";
    private const string achievementProgressesPath = "achievementProgresses";
    private const string isOnlinePath = "isOnline";
    private const string signUpDatePath = "signUpDate";

    public async Task<bool> SaveUserData(UserData userData)
    {
        return await dbManager.TrySetDocumentAsync(collectionPath, userData.userId, userData);
    }

    public async Task<bool> UpdatePassword(string userId, string newPassword)
    {
        return await dbManager.TryUpdateFieldAsync(collectionPath, userId, passwordPath, newPassword);
    }

    public async Task<bool> UpdateNickname(string userId, string newNickname)
    {
        return await dbManager.TryUpdateFieldAsync(collectionPath, userId, nicknamePath, newNickname);
    }

    public async Task<bool> UpdateAvatar(string userId, string newAvatar)
    {
        return await dbManager.TryUpdateFieldAsync(collectionPath, userId, avatarPath, newAvatar);
    }

    public async Task<bool> UpdateProfileImage(string userId, string newProfileImage)
    {
        return await dbManager.TryUpdateFieldAsync(collectionPath, userId, profileImagePath, newProfileImage);
    }

    public async Task<bool> UpdateCoin(string userId, int newCoin)
    {
        return await dbManager.TryUpdateFieldAsync(collectionPath, userId, coinPath, newCoin);
    }

    public async Task<bool> UpdateQuestProgresses(string userId, List<QuestDataForDB> questProgresses)
    {
        return await dbManager.TryUpdateFieldAsync(collectionPath, userId, questProgressesPath, questProgresses);
    }

    public async Task<bool> UpdateAchievementProgresses(string userId, List<AchievementDataForDB> achievementProgresses)
    {
        return await dbManager.TryUpdateFieldAsync(collectionPath, userId, achievementProgressesPath, achievementProgresses);
    }

    public async Task<bool> UpdateIsOnline(string userId, bool isOnline)
    {
        return await dbManager.TryUpdateFieldAsync(collectionPath, userId, isOnlinePath, isOnline);
    }

    public async Task<bool> UpdateSignUpDate(string userId, Timestamp signUpDate)
    {
        return await dbManager.TryUpdateFieldAsync(collectionPath, userId, signUpDatePath, signUpDate);
    }

    public async Task<UserData> GetUserData(string userId)
    {
        return await dbManager.GetDocumentAsync<UserData>(collectionPath, userId);
    }

    public async Task<List<UserData>> GetAllUsers()
    {
        return await dbManager.GetCollectionAsync<UserData>(collectionPath);
    }

    public async Task<bool> CheckIdExists(string userId)
    {
        return await dbManager.DocumentExistsAsync(collectionPath, userId);
    }
}
