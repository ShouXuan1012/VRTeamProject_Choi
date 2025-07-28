using UnityEngine.UI;
using UnityEngine;

public class ItemPurchaseUI : MonoBehaviour
{
    public int price;
    public Button yesButton;
    public Button noButton;
   
    

    private void Start()
    {
        yesButton.onClick.AddListener(OnClickYes);
        noButton.onClick.AddListener(()=>Destroy(gameObject));
    }

    
    private void OnClickYes()
    {


        if (CoinManager.Instance.UseCoins(price))
        {
            QuestEvents.FoodPurchased();
            //사운드, 이펙트
        }

        else
        {
            UIManager.Instance.OpenUI("Purchase_Fail");
        }
        Destroy(gameObject);
    }
}
