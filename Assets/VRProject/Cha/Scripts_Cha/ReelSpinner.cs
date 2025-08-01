using UnityEngine;
using System.Collections;

public class ReelSpinner : MonoBehaviour
{
    public RectTransform[] symbols;
    public float spinSpeed = 1000f;
    public float deceleration = 2000f;

    private int symbolCount;
    private float symbolHeight;
    private RectTransform rect;

    public void Init(int count)
    {
        symbolCount = count;
        rect = GetComponent<RectTransform>();

        if (symbols.Length > 0)
            symbolHeight = symbols[0].rect.height;
    }

    public IEnumerator Spin(int targetIndex, float spinTime)
    {
        float elapsed = 0f;
        float currentSpeed = spinSpeed;

        // 총 높이 계산
        float totalHeight = symbolCount * symbolHeight;

        while (elapsed < spinTime)
        {
            Vector2 pos = rect.anchoredPosition;
            pos.y -= currentSpeed * Time.deltaTime;

            // 무한 회전 처리
            if (pos.y <= -totalHeight)
                pos.y += totalHeight;

            rect.anchoredPosition = pos;

            elapsed += Time.deltaTime;

            // 마지막 0.5초 감속
            if (spinTime - elapsed < 0.5f)
                currentSpeed = Mathf.Lerp(currentSpeed, 200f, Time.deltaTime * 5f);

            yield return null;
        }

        // 🔹 목표 심볼 위치로 부드럽게 이동
        float currentPos = Mathf.Repeat(rect.anchoredPosition.y, totalHeight);
        float targetPos = targetIndex * symbolHeight;

        // 현재 위치에서 가장 가까운 targetPos로 이동
        float distance = targetPos - currentPos;
        if (distance > totalHeight / 2) distance -= totalHeight;
        if (distance < -totalHeight / 2) distance += totalHeight;

        float moveTime = 0.2f;
        float startPos = rect.anchoredPosition.y;
        float timer = 0f;

        while (timer < moveTime)
        {
            float y = Mathf.Lerp(startPos, startPos + distance, timer / moveTime);
            rect.anchoredPosition = new Vector2(0, y);
            timer += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = new Vector2(0, startPos + distance);
    }
}