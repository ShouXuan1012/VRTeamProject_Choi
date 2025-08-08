using UnityEngine.UI;
using UnityEngine;

public class ItemPurchaseUI : MonoBehaviour
{
    public int price;
    public Button yesButton;
    public Button noButton;
    public AudioClip AudioClip;
     
    

    private void Start()
    {
        yesButton.onClick.AddListener(OnClickYes);
        noButton.onClick.AddListener(()=>Destroy(gameObject));
    }

    
    private void OnClickYes()
    {


        if (CoinManager.Instance.UseCoins(price))
        {
            QuestEvents.Invoke(EQuestType.FoodPurchased);
            AchievementManager.Instance.AddProgress(EAchievementType.MarketSpender, price);
            AudioClip.LoadAudioData();
        }

        Destroy(gameObject);
    }
}
