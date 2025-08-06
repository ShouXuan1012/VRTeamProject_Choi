using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelSpinner : MonoBehaviour
{
    [Header("Prefabs")]
    public RectTransform[] symbolPrefabs;

    [Header("Settings")]
    public float spinSpeed = 1000f;
    public float deceleration = 2000f;
    public float spacing = 10f;   // Layout Group의 Spacing과 동일하게 설정

    private List<RectTransform> symbols = new();
    private RectTransform rect;
    private float slotHeight;
    private int symbolCount;

    public void Init()
    {
        rect = GetComponent<RectTransform>();
        symbols.Clear();

        // ✅ 심볼 높이 계산
        if (symbolPrefabs.Length > 0)
            slotHeight = symbolPrefabs[0].sizeDelta.y + spacing;

        symbolCount = symbolPrefabs.Length;

        // 🔹 [4] → [1,2,3,4] → [1,2,3,4] → [1,2,3,4] → [1]
        // 맨 앞에 마지막 심볼(4)
        RectTransform firstClone = Instantiate(symbolPrefabs[symbolCount - 1], rect);
        symbols.Add(firstClone);

        // 3세트 반복
        for (int loop = 0; loop < 3; loop++)
        {
            for (int i = 0; i < symbolCount; i++)
            {
                RectTransform clone = Instantiate(symbolPrefabs[i], rect);
                symbols.Add(clone);
            }
        }

        // 맨 끝에 첫번째 심볼(1)
        RectTransform lastClone = Instantiate(symbolPrefabs[0], rect);
        symbols.Add(lastClone);
    }

    public IEnumerator Spin(int targetIndex, float spinTime)
    {
        float elapsed = 0f;
        float currentSpeed = spinSpeed;
        float totalHeight = symbols.Count * slotHeight;

        // 🔹 랜덤 회전
        while (elapsed < spinTime)
        {
            Vector2 pos = rect.anchoredPosition;
            pos.y -= currentSpeed * Time.deltaTime;

            pos.y = Mathf.Repeat(pos.y, totalHeight);
            rect.anchoredPosition = pos;

            elapsed += Time.deltaTime;

            if (spinTime - elapsed < 0.5f)
                currentSpeed = Mathf.Lerp(currentSpeed, 200f, Time.deltaTime * 5f);

            yield return null;
        }

        // 🔹 두 번째 세트 시작 위치
        float secondSetStart = slotHeight; // [4] 하나 제외 후 시작
        float targetPos = secondSetStart + (targetIndex * slotHeight);

       
        targetPos = Mathf.Repeat(targetPos, totalHeight);

        // 🔹 최종 정렬 보정
        float currentPos = Mathf.Repeat(rect.anchoredPosition.y, totalHeight);
        float distance = targetPos - currentPos;
        if (distance > totalHeight / 2) distance -= totalHeight;
        if (distance < -totalHeight / 2) distance += totalHeight;

        float moveTime = 0.25f;
        float startPos = rect.anchoredPosition.y;
        float timer = 0f;

        while (timer < moveTime)
        {
            float y = Mathf.Lerp(startPos, startPos + distance, timer / moveTime);
            rect.anchoredPosition = new Vector2(0, Mathf.Repeat(y, totalHeight));
            timer += Time.deltaTime;
            yield return null;
        }
        
        // ✅ 최종 스냅
        rect.anchoredPosition = new Vector2(0, targetPos);
    }
}
