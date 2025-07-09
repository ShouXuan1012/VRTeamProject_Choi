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
        CoinManager.Instance.UseCoins(price);

        if (CoinManager.Instance.UseCoins(price))
        {
            //사운드, 이펙트
        }

        else
        {
            //재화 부족 UI 생성
        }
        Destroy(gameObject);
    }
}
