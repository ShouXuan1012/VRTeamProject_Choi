using System.Collections;
using UnityEngine;

public class FXRepeater : MonoBehaviour
{
    [Header("FX ������")]
    public GameObject fxPrefab;

    [Header("����")]
    public float interval = 15f;
    public float destroyDelay = 5f;

    [Header("����Ʈ ���� ��ġ")]
    public Transform spawnPoint;

    void Start()
    {
        StartCoroutine(SpawnLoopCo());    
    }

    IEnumerator SpawnLoopCo()
    {
        while (true)
        {
            // ����Ʈ ����
            GameObject fx = Instantiate(fxPrefab, spawnPoint.position, spawnPoint.rotation);

            // ����Ʈ �ı�
            Destroy(fx, destroyDelay);

            // ���� �ݺ� ���� ���
            yield return new WaitForSeconds(interval);
        }
    }


}
