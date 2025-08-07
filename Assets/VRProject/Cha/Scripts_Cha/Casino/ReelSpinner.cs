using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelSpinner : MonoBehaviour
{
    public RectTransform[] symbolPrefabs;   // 원본 심볼(1~4)
    public float spinSpeed = 1200f;
    public float spacing = 10f;

    private List<RectTransform> spawnedSymbols = new List<RectTransform>();
    private RectTransform rect;
    private float symbolHeight;
    private int symbolCount;

    public void Init()
    {
        rect = GetComponent<RectTransform>();
        spawnedSymbols.Clear();

        if (symbolPrefabs.Length == 0) return;

        symbolHeight = symbolPrefabs[0].sizeDelta.y + spacing;

        // 🔹 첫번째 세트 앞에 "4"(index 3) 추가
        int[] order = { 3, 0, 1, 2 }; // 4 → 1 → 2 → 3

        // 🔹 두 세트 반복 (4123 | 4123)
        for (int loop = 0; loop < 2; loop++)
        {
            foreach (int i in order)
            {
                RectTransform clone = Instantiate(symbolPrefabs[i], rect);
                clone.name = symbolPrefabs[i].name + "_clone_" + loop;
                spawnedSymbols.Add(clone);
            }
        }

        // 🔹 마지막 보정(1번) 추가 → 빈칸 방지
        RectTransform lastClone = Instantiate(symbolPrefabs[3], rect);
        lastClone.name = symbolPrefabs[3].name + "_clone_end";
        spawnedSymbols.Add(lastClone);

        symbolCount = spawnedSymbols.Count;
    }
    


    public IEnumerator Spin(int targetIndex, float spinTime)
    {
        float elapsed = 0f;
        float totalHeight = symbolCount * symbolHeight;

        // 랜덤 스타트 위치
        float posY = Random.Range(0f, totalHeight);
        rect.anchoredPosition = new Vector2(0, posY);

        // 1️⃣ 자유 회전
        while (elapsed < spinTime)
        {
            posY += spinSpeed * Time.deltaTime;
            posY = Mathf.Repeat(posY, totalHeight);
            rect.anchoredPosition = new Vector2(0, posY);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 2️⃣ 결과 스냅 위치 계산 (두 번째 세트 기준)
        int baseIndex = symbolPrefabs.Length;        // 2번째 세트 시작 인덱스
        int finalIndex = baseIndex + targetIndex;    // 결과 심볼 위치
        float targetPos = finalIndex * symbolHeight;

        // 중앙 보정 (두 번째 슬롯을 중앙으로)
        float centerOffset = symbolHeight;

        // 3️⃣ 결과 위치 강제 스냅
        rect.anchoredPosition = new Vector2(0, targetPos - centerOffset);
    }
}
