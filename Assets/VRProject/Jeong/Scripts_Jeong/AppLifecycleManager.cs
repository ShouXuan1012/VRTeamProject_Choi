using UnityEngine;

public class AppLifecycleManager : MonoBehaviour
{
    private void Awake()
    {
        Application.wantsToQuit += WantsToQuitHandler;
        DontDestroyOnLoad(gameObject);
    }

    private bool WantsToQuitHandler()
    {
        HandleQuitSequence();
        return false;  // 종료를 막고 비동기 작업이 완료될 때까지 기다림
    }
    private async void HandleQuitSequence()
    {
        if(CurrentUserManager.Instance.CurrentUserData == null)
        {
            Application.Quit(); // 현재 사용자 데이터가 없으면 바로 종료
            return;
        }

        int coin = CurrentUserManager.Instance.CurrentUserData.coin;
        await CurrentUserManager.Instance.UpdateCoin(coin);

        await CurrentUserManager.Instance.UpdateIsOnline(false);

        Application.Quit(); // 모든 작업이 완료된 후 게임 종료
    }
}
