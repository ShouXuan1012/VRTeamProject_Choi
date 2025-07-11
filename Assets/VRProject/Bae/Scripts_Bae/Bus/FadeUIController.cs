using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ȭ�� ��ȯ ȿ��(���̵� ��/�ƿ�)�� ó���ϴ� �̱��� Ŭ����
/// </summary>
public class FadeUIController : MonoBehaviour
{
    public static FadeUIController Instance;

    [SerializeField] private Image fadeImage; // ���̵� ȿ���� ���� UI �̹���

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
            fadeImage.color = new Color(0, 0, 0, alpha); // ���������� ���̵� �ƿ�
            yield return null;
        }
        fadeImage.color = Color.black; // ������ ���������� ����
    }

    public IEnumerator FadeIn(float duration = 1f)
    {
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float alpha = 1 - (t / duration);
            fadeImage.color = new Color(0, 0, 0, alpha); // ���������� �������� ���̵� ��
            yield return null;
        }
        fadeImage.color = Color.clear; // ������ �������� ����
        fadeImage.gameObject.SetActive(false); // ���̵� �̹��� ��Ȱ��ȭ
    }
}
