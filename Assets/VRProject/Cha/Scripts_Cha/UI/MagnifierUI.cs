using Unity.VisualScripting;
using UnityEngine;

public class MagnifierUI : MonoBehaviour
{
    private InspectableItem currentTarget;

    public Transform uiParent;  

    public void SetTarget(InspectableItem item ,Transform Parent)
    {
        currentTarget = item;
        uiParent = Parent;
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
        Instantiate(currentTarget.purchaseUIPrefab,uiParent);
    }


}