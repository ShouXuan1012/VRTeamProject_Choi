

using System;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    
    public static CoinManager Instance { get; private set; }
    

    private const int MAX_COINS = 500000;
    private int currentCoins;
    private CoinUI currentCoinUI;

    public int CurrentCoins => currentCoins;
    public event Action<int> OnCoinChanged;
  
    private void Awake()
    {
        if(Instance!=null&&Instance!=this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        currentCoins = CurrentUserManager.Instance.CurrentUserData?.coin ?? 0; // DB에서 가져온 데이터로 초기화
        OnCoinChanged?.Invoke(currentCoins);
    }

    /// <summary>
    /// 코인 추가. 최대 금액 초과 X.
    /// </summary>
    /// <returns>성공 여부</returns>
    public bool AddCoins(int amount)
    {
        if (amount <= 0) return false;

        if (currentCoins + amount > MAX_COINS)
        {
            currentCoins = MAX_COINS;
            OnCoinChanged?.Invoke(currentCoins);
            return false; // 일부만 반영되었을 수 있음
        }

        currentCoins += amount;
        CurrentUserManager.Instance.SetCoin(currentCoins); // CurrentUserData에도 반영

        OnCoinChanged?.Invoke(currentCoins);
        return true;
    }

    /// <summary>
    /// 코인 차감. 보유 금액보다 클 경우 실패.
    /// </summary>
    /// <returns>성공 여부</returns>
    public bool UseCoins(int amount)
    {
        if (amount <= 0 || amount > currentCoins)
        { 
            return false; 
        }

        if (currentCoins >= amount)
        { 
            currentCoins -= amount;
            CurrentUserManager.Instance.SetCoin(currentCoins); // CurrentUserData에도 반영

            OnCoinChanged?.Invoke(currentCoins);
            
        }
        return true;
    }

    private void OnEnable()
    {
        PlayerSpawner.OnPlayerSpawned += SetupUIFromPlayer;
    }

    private void OnDisable()
    {
        PlayerSpawner.OnPlayerSpawned -= SetupUIFromPlayer;
    }

    public void SetupUIFromPlayer(GameObject player)
    {
        var coinUI = player.transform.Find("Camera Offset/Main Camera/UICamera/MainUI/CoinCounterUI/Text")?.GetComponent<CoinUI>();
        if (coinUI == null)
        {
            Debug.LogWarning("[CoinManager] CoinUI를 찾을 수 없습니다.");
            return;
        }

        // 이전 리스너 제거
        if (currentCoinUI != null)
            OnCoinChanged -= currentCoinUI.UpdateCoinText;

        currentCoinUI = coinUI;

        // 새 리스너 등록
        OnCoinChanged += coinUI.UpdateCoinText;
        coinUI.UpdateCoinText(currentCoins);
    }
}
