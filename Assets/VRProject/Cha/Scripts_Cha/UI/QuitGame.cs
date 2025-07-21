using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
        Debug.Log("게임 종료 시도"); // 에디터에서는 종료되지 않기 때문에 디버그 로그로 확인
    }
}
