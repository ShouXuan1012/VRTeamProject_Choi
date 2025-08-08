using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

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
        coinText.text = FormatCoinKoreanAccurate(coins); ;
       
    }


    private string FormatCoinKoreanAccurate(long coins)
    {
        if(coins >= 100_000_000) // 1억 이상
    {
            float value = coins / 100_000_000f;
            return value.ToString(value % 1 == 0 ? "0" : "0.##") + "억";
            // 소수점 최대 2자리
        }
    else if (coins >= 10_000_000) // 1천만 이상
        {
            float value = coins / 10_000_000f;
            return value.ToString(value % 1 == 0 ? "0" : "0.#") + "천만";
            // 소수점 최대 1자리
        }

        else
        {
            return coins.ToString("N0") +"₩";
        }
    }
}