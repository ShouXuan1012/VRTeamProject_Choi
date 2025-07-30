using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CurrentUserManager : MonoBehaviour
{
    public static CurrentUserManager Instance { get; private set; }

    public UserData CurrentUserData { get; private set; }
    public Dictionary<string, TopScoreData> TopScoreDict { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetCurrentUserData(UserData userData)
    {
        CurrentUserData = userData;
    }
    public void SetNickname(string nickname)
    {
        if (CurrentUserData != null)
        {
            CurrentUserData.nickname = nickname;
        }
        else
        {
            Debug.LogError("현재 사용자 데이터가 없습니다.");
        }
    }
    public void SetAvatar(string avatar)
    {
        if (CurrentUserData != null)
        {
            CurrentUserData.avatar = avatar;
        }
        else
        {
            Debug.LogError("현재 사용자 데이터가 없습니다.");
        }
    }
    public void SetProfileImage(string profileImage)
    {
        if (CurrentUserData != null)
        {
            CurrentUserData.profileImage = profileImage;
        }
        else
        {
            Debug.LogError("현재 사용자 데이터가 없습니다.");
        }
    }
    public void SetCoin(int coin)
    {
        if (CurrentUserData != null)
        {
            CurrentUserData.coin = coin;
        }
        else
        {
            Debug.LogError("현재 사용자 데이터가 없습니다.");
        }
    }
    public void SetIsOnline(bool isOnline)
    {
        if (CurrentUserData != null)
        {
            CurrentUserData.isOnline = isOnline;
        }
        else
        {
            Debug.LogError("현재 사용자 데이터가 없습니다.");
        }
    }

    public void AddOrUpdateTopScore(TopScoreData topScoreData)
    {
        if (TopScoreDict.ContainsKey(topScoreData.gameId))
        {
            TopScoreDict[topScoreData.gameId] = topScoreData;
        }
        else
        {
            TopScoreDict.Add(topScoreData.gameId, topScoreData);
        }
    }

    public async Task<bool> UpdateNickname(string newNickname)
    {
        return await UserDataManager.Instance.UpdateNickname(CurrentUserData.userId, newNickname);
    }
    public async Task<bool> UpdateAvatar(string newAvatar)
    {
        return await UserDataManager.Instance.UpdateAvatar(CurrentUserData.userId, newAvatar);
    }
    public async Task<bool> UpdateProfileImage(string newProfileImage)
    {
        return await UserDataManager.Instance.UpdateProfileImage(CurrentUserData.userId, newProfileImage);
    }
    public async Task<bool> UpdateCoin(int newCoin)
    {
        return await UserDataManager.Instance.UpdateCoin(CurrentUserData.userId, newCoin);
    }
    public async Task<bool> UpdateIsOnline(bool isOnline)
    {
        return await UserDataManager.Instance.UpdateIsOnline(CurrentUserData.userId, isOnline);
    }

    public async Task<bool> AddOrUpdateTopScoreData(TopScoreData topScoreData)
    {
        bool isDataExists = await TopScoreDataManager.Instance.CheckGameIdExists(CurrentUserData.userId, topScoreData.gameId);
        if (isDataExists)
        {
            return await TopScoreDataManager.Instance.UpdateScore(CurrentUserData.userId, topScoreData.gameId, topScoreData.score);
        }
        else
        {
            return await TopScoreDataManager.Instance.SaveTopScoreData(CurrentUserData.userId, topScoreData.gameId, topScoreData);
        }
    }
}
