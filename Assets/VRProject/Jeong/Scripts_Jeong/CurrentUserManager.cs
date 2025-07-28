using System.Threading.Tasks;
using UnityEngine;

public class CurrentUserManager : MonoBehaviour
{
    public static CurrentUserManager Instance { get; private set; }

    public UserData CurrentUserData { get; private set; }

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

    public async Task<bool> UpdateNickname(string newNickname)
    {
        return await UserDataManager.Instance.UpdateNickname(CurrentUserData.userId, newNickname);
    }
    public async Task<bool> UpdateAvatar(string newAvatar)
    {
        return await UserDataManager.Instance.UpdateAvatar(CurrentUserData.userId, newAvatar);
    }
    public async Task<bool> UpdateCoin(int newCoin)
    {
        return await UserDataManager.Instance.UpdateCoin(CurrentUserData.userId, newCoin);
    }
}
