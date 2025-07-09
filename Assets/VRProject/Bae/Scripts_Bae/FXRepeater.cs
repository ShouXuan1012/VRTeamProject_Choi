using System.Collections;
using UnityEngine;

public class FXRepeater : MonoBehaviour
{
    [Header("FX 프리팹")]
    public GameObject fxPrefab;

    [Header("설정")]
    public float interval = 15f;
    public float destroyDelay = 5f;

    [Header("이펙트 생성 위치")]
    public Transform spawnPoint;

    void Start()
    {
        StartCoroutine(SpawnLoopCo());    
    }

    IEnumerator SpawnLoopCo()
    {
        while (true)
        {
            // 이펙트 생성
            GameObject fx = Instantiate(fxPrefab, spawnPoint.position, spawnPoint.rotation);

            // 이펙트 파괴
            Destroy(fx, destroyDelay);

            // 다음 반복 까지 대기
            yield return new WaitForSeconds(interval);
        }
    }


}
