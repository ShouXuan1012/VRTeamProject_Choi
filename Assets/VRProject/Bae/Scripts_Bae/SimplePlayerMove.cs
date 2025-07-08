using UnityEngine;

public class SimplePlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;  // �̵� �ӵ�

    private void Update()
    {
        // �Է°� �ޱ� (WASD �Ǵ� ����Ű)
        float horizontal = Input.GetAxis("Horizontal"); // A/D �Ǵ� ��/��
        float vertical = Input.GetAxis("Vertical");     // W/S �Ǵ� ��/��

        // �̵� ���� ��� (X,Z�����θ� �̵�)
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // ������ ���� ��� �̵�
        if (moveDirection.magnitude >= 0.1f)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}