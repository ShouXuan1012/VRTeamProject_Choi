using Photon.Voice.Unity;
using UnityEngine;


public class InspectableItem : MonoBehaviour
{
    public GameObject purchaseUIPrefab;

    public MagnifierUI magnifierUI;
    public int price;
    public void ShowPurchaseUI()
    {
        Instantiate(purchaseUIPrefab);
    }
   
}
