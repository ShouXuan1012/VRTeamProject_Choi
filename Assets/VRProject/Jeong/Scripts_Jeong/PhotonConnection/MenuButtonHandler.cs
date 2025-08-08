using UnityEngine;
using UnityEngine.UI;

public class MenuButtonHandler : MonoBehaviour
{
    [SerializeField] private ConnectionManager connectionManager;

    [SerializeField] private Button joinRoomButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        joinRoomButton.onClick.AddListener(OnJoinRoomClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
    }

    private void OnJoinRoomClicked()
    {
        connectionManager.TryJoinRoom();
    }
    private void OnQuitClicked()
    {
        HandleQuitSequence();
        //Application.Quit();
    }
    private async void HandleQuitSequence()
    {
        if (CurrentUserManager.Instance.CurrentUserData == null)
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
