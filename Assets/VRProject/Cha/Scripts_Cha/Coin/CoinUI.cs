using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    public Text coinText;

    //private void Start()
    //{
    //    if(CoinManager.Instance != null)
    //    {
    //        CoinManager.Instance.OnCoinChanged += UpdateCoinText;
    //        UpdateCoinText(CoinManager.Instance.CurrentCoins);
    //    }
    //}
    //
    //void OnEnable()
    //{
    //    if (CoinManager.Instance != null)
    //    {
    //        CoinManager.Instance.OnCoinChanged += UpdateCoinText;
    //        UpdateCoinText(CoinManager.Instance.CurrentCoins);
    //    }
    //         
    //}
    //void OnDisable()
    //{
    // if(CoinManager.Instance!= null)
    //    {
    //        CoinManager.Instance.OnCoinChanged -=UpdateCoinText;
    //    }
    //}

    public void UpdateCoinText(int coins)
    {
        if (coinText == null)
        {
            Debug.LogWarning("[CoinUI] coinText가 null입니다.");
            return;
        }
        coinText.text = $"{coins:N0}₩";
    }
}