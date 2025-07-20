using Unity.VisualScripting;
using UnityEngine;

public class MagnifierUI : MonoBehaviour
{
    private InspectableItem currentTarget;
    Transform uiParent;

    void OnEnable()
    {
        PlayerSpawner.OnPlayerSpawned += OnPlayerSpawned;
    }

    void OnDisable()
    {
        PlayerSpawner.OnPlayerSpawned -= OnPlayerSpawned;
    }

    private void OnPlayerSpawned(GameObject player)
    {
        // 플레이어 하위에서 MainUI 찾기
        Transform found = player.transform.Find("Camera Offset/Main Camera/UICamera/MainUI");
        if (found != null)
            uiParent = found;
        gameObject.SetActive(false);
    }

    public void SetTarget(InspectableItem item)
    {
        currentTarget = item;

    }

    public void ClearTarget()
    {
        currentTarget = null;
    }
    public void OnClick()
    {
        if (currentTarget == null || currentTarget.purchaseUIPrefab == null)
        {
            return;
        }

        UIManager.Instance.SpawnDynamicUI(currentTarget.purchaseUIPrefab, uiParent, (go) =>
        {
            var purchaseUI = go.GetComponent<ItemPurchaseUI>();
            if (purchaseUI != null)
            {
                purchaseUI.price = currentTarget.price;


            }
        });
    }


}