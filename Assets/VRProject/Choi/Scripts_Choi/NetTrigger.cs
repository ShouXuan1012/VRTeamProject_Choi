// NetTrigger.cs
using UnityEngine;

public class NetTrigger : MonoBehaviour
{
    public ScoreManager scoreManager; // ���� ���� ��ũ��Ʈ ����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            scoreManager.AddScore(1); // ���� 1�� �߰�
        }
    }
}
