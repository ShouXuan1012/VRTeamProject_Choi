// NetTrigger.cs
using UnityEngine;

public class NetTrigger : MonoBehaviour
{
    public ScoreManager scoreManager; // 점수 관리 스크립트 참조

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            scoreManager.AddScore(1); // 점수 1점 추가
        }
    }
}
