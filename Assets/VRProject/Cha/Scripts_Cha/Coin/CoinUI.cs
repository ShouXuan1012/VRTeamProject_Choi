using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    public Text coinText;

    private void Start()
    {
        if(CoinManager.Instance != null)
        {
            CoinManager.Instance.OnCoinChanged += UpdateCoinText;
            UpdateCoinText(CoinManager.Instance.CurrentCoins);
        }
    }

    void OnEnable()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.OnCoinChanged += UpdateCoinText;
            UpdateCoinText(CoinManager.Instance.CurrentCoins);
        }
             
    }
    void OnDisable()
    {
     if(CoinManager.Instance!= null)
        {
            CoinManager.Instance.OnCoinChanged -=UpdateCoinText;
        }
    }

    void UpdateCoinText(int coins)
    {
        coinText.text = $"{coins:N0}₩";
    }
}