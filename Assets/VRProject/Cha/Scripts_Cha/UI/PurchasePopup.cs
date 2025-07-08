
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine;
using static UnityEditor.Progress;
public class PurchasePopup : MonoBehaviour
{
    public static PurchasePopup Instance;

    private Action confirmAction;
    public GameObject popupUI;
    private InspectableItem currentItem;
    private void Awake()
    {
        Instance = this;
    }
    public void Show(Action confirm,InspectableItem item)
    {
       
        confirmAction = confirm;
        currentItem = item;
        popupUI.SetActive(true);
    }

    public void OnClickYes()
    {
        confirmAction?.Invoke();
        confirmAction = null;
        
        if(currentItem != null) 
        {
            currentItem.HideInfo(); 
        }
        popupUI.SetActive(false);
        currentItem = null;

    }

    public void OnClickNo()
    {
        confirmAction = null;
        popupUI.SetActive(false);
        if (currentItem != null)
        {
            currentItem.HideInfo();
        }
        currentItem = null;
    }
}