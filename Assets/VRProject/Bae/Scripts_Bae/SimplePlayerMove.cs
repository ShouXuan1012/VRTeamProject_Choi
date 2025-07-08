using UnityEngine;

public class SimplePlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;  // 이동 속도

    private void Update()
    {
        // 입력값 받기 (WASD 또는 방향키)
        float horizontal = Input.GetAxis("Horizontal"); // A/D 또는 ←/→
        float vertical = Input.GetAxis("Vertical");     // W/S 또는 ↑/↓

        // 이동 방향 계산 (X,Z축으로만 이동)
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // 방향이 있을 경우 이동
        if (moveDirection.magnitude >= 0.1f)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}