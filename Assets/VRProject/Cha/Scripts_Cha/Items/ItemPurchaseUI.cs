using UnityEngine.UI;
using UnityEngine;

public class ItemPurchaseUI : MonoBehaviour
{
    public int price;
    public Button yesButton;
    public Button noButton;
    public GameObject purchasefailUI;
    private Transform uiParent;

    private void Start()
    {
        yesButton.onClick.AddListener(OnClickYes);
        noButton.onClick.AddListener(()=>Destroy(gameObject));
    }

    public void Init(Transform parent)
    {
        this.uiParent = parent;
    }
    private void OnClickYes()
    {


        if (CoinManager.Instance.UseCoins(price))
        {
            //사운드, 이펙트
        }

        else
        {
            GameObject failUI=Instantiate(purchasefailUI,uiParent);
        }
        Destroy(gameObject);
    }
}
