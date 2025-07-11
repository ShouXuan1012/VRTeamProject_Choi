using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 화면 전환 효과(페이드 인/아웃)를 처리하는 싱글턴 클래스
/// </summary>
public class FadeUIController : MonoBehaviour
{
    public static FadeUIController Instance;

    [SerializeField] private Image fadeImage; // 페이드 효과를 위한 UI 이미지

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public IEnumerator FadeOut(float duration = 1f)
    {
        fadeImage.gameObject.SetActive(true);

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float alpha = t / duration;
            fadeImage.color = new Color(0, 0, 0, alpha); // 검은색으로 페이드 아웃
            yield return null;
        }
        fadeImage.color = Color.black; // 완전히 검은색으로 설정
    }

    public IEnumerator FadeIn(float duration = 1f)
    {
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float alpha = 1 - (t / duration);
            fadeImage.color = new Color(0, 0, 0, alpha); // 검은색에서 투명으로 페이드 인
            yield return null;
        }
        fadeImage.color = Color.clear; // 완전히 투명으로 설정
        fadeImage.gameObject.SetActive(false); // 페이드 이미지 비활성화
    }
}
