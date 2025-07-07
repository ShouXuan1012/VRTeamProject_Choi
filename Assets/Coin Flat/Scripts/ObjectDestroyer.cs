using UnityEngine;

namespace ObjectDestroy // �������� "MyNamespace" �� ���� ������������ ����
{
    public class ObjectDestroyer : MonoBehaviour
    {
        public float destroyDelay = 3f; // �������� ����� ������������ �������

        void Start()
        {
            Destroy(gameObject, destroyDelay); // ���������� ������ ����� �������� �����
        }
    }
}