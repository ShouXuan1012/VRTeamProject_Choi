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
    public Transform[] spawnPoints;

    void Start()
    {
        StartCoroutine(SpawnLoopCo());    
    }

    IEnumerator SpawnLoopCo()
    {
        while (true)
        {
            foreach(Transform point in spawnPoints)
            {
                GameObject fx = Instantiate(fxPrefab, point.position, point.rotation);
                Destroy(fx, destroyDelay);
            }

            // ���� �ݺ� ���� ���
            yield return new WaitForSeconds(interval);
        }
    }


}
