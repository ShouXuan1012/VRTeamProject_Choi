using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumUITrigger : MonoBehaviour
{
    [Header("UI 오브젝트")]
    public GameObject MuseumUI; // UI 오브젝트

    private void Start()
    {
        // 시작 시 UI 비활성화
        if (MuseumUI != null)
        {
            MuseumUI.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 트리거에 들어오면 UI 활성화
            MuseumUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 트리거를 벗어나면 UI 비활성화
            MuseumUI.SetActive(false);
        }
    }
}
