using Photon.Voice.Unity;
using UnityEngine;


public class InspectableItem : MonoBehaviour
{
    public GameObject purchaseUIPrefab;
   

    public void ShowPurchaseUI()
    {
        Instantiate(purchaseUIPrefab);
    }
   
}
