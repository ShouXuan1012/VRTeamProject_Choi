using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform targetLocation;
    [SerializeField] private float stayTime = 1f;

    private bool hasTriggered = false;
    private float timer = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player") && IsLocalPlayer(other))
        {
            timer += Time.deltaTime;
            if (timer >= stayTime)
            {
                hasTriggered = true;
                TeleportPlayer(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && IsLocalPlayer(other))
        {
            timer = 0f;
            hasTriggered = false;
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        Debug.Log("[Portal] 플레이어 위치 이동");
        player.transform.position = targetLocation.position;
        player.transform.rotation = targetLocation.rotation;
    }

    private bool IsLocalPlayer(Collider other)
    {
        PhotonView view = other.GetComponent<PhotonView>();
        return view != null && view.IsMine;
    }
}
