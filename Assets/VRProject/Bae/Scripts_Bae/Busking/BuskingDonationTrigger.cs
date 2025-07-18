using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuskingDonationTrigger : MonoBehaviour
{
    [Header("후원 UI Canvas")]
    [SerializeField]private GameObject donationCanvas;

    private string playerTag = "Player"; // 플레이어 태그

    private void Start()
    {
        if (donationCanvas != null)
            donationCanvas.SetActive(false);  //  후원 UI 비활성화
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (donationCanvas != null)
        {
            donationCanvas.SetActive(true);  // 후원 UI 활성화
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        
        if (donationCanvas != null)
        {
            donationCanvas.SetActive(false);  // 후원 UI 비활성화
        }
    }
}
