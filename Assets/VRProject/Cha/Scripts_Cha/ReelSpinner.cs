using UnityEngine;
using System.Collections;

public class ReelSpinner : MonoBehaviour
{
    public RectTransform content; // 심볼 부모
    public float spinSpeed = 1000f;
    public float stopTime = 2f;
    public float symbolHeight = 150f;

    private int symbolCount;
    private bool spinning = false;
    private int finalIndex = 0;

    public void Init(int count)
    {
        symbolCount = count;
    }

    public IEnumerator Spin(int targetIndex)
    {
        spinning = true;
        float elapsed = 0f;

        while (elapsed < stopTime)
        {
            content.anchoredPosition -= new Vector2(0, spinSpeed * Time.deltaTime);
            if (content.anchoredPosition.y >= symbolHeight * symbolCount)
                content.anchoredPosition = Vector2.zero;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 정확히 targetIndex에 스냅
        float targetPos = targetIndex * symbolHeight;
        content.anchoredPosition = new Vector2(0, targetPos);

        finalIndex = targetIndex;
        spinning = false;
    }

    public int GetResultIndex()
    {
        return finalIndex;
    }
}
