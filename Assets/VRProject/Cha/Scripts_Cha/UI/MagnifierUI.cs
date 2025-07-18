using Unity.VisualScripting;
using UnityEngine;

public class MagnifierUI : MonoBehaviour
{
    private InspectableItem currentTarget;

    public Transform uiParent;  

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
        if (currentTarget == null||currentTarget.purchaseUIPrefab==null)
        {
            return;
        }

        UIManager.Instance.SpawnDynamicUI(currentTarget.purchaseUIPrefab, uiParent, (go) =>
        { var purchaseUI=go.GetComponent<ItemPurchaseUI>();
            if (purchaseUI != null)
            {
                purchaseUI.price = currentTarget.price;


            }
        });
    }


}