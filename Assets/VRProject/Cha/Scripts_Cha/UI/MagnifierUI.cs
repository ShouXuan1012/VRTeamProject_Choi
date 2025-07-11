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
        if (currentTarget == null)
        {
            return;
        }
        if(currentTarget.purchaseUIPrefab==null)
        {
            return;
        }
        GameObject ui=Instantiate(currentTarget.purchaseUIPrefab,uiParent);
        Destroy(ui, 99f);
        ui.GetComponent<ItemPurchaseUI>().Init(uiParent);
    }


}