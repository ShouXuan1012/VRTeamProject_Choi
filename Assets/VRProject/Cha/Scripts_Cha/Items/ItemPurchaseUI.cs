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
            //����, ����Ʈ
        }

        else
        {
            //��ȭ ���� UI ����
        }
        Destroy(gameObject);
    }
}
