using System.Collections;
using System.Threading.Tasks;
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
        StartCoroutine(QuitSequenceCoroutine());
    }
    private IEnumerator QuitSequenceCoroutine()
    {
        var quitTask = HandleQuitSequence();
        while (!quitTask.IsCompleted)
        {
            yield return null; // Task가 끝날 때까지 기다림
        }

        Application.Quit();
    }

    private async Task HandleQuitSequence()
    {
        if (CurrentUserManager.Instance.CurrentUserData == null)
        {
            return; // 현재 사용자 데이터가 없으면 바로 종료
        }

        int coin = CurrentUserManager.Instance.CurrentUserData.coin;
        await CurrentUserManager.Instance.UpdateCoin(coin);
        await CurrentUserManager.Instance.UpdateIsOnline(false);
    }
}
