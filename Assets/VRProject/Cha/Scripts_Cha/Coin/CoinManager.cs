

using System;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    
    public static CoinManager Instance { get; private set; }
    

    private const int MAX_COINS = 316000;
    private int currentCoins;

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
        currentCoins = MAX_COINS;
        DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// ���� �߰�. �ִ� �ݾ� �ʰ� X.
    /// </summary>
    /// <returns>���� ����</returns>
    public bool AddCoins(int amount)
    {
        if (amount <= 0) return false;

        if (currentCoins + amount > MAX_COINS)
        {
            currentCoins = MAX_COINS;
            OnCoinChanged?.Invoke(currentCoins);
            return false; // �Ϻθ� �ݿ��Ǿ��� �� ����
        }

        currentCoins += amount;
        OnCoinChanged?.Invoke(currentCoins);
        return true;
    }

    /// <summary>
    /// ���� ����. ���� �ݾ׺��� Ŭ ��� ����.
    /// </summary>
    /// <returns>���� ����</returns>
    public bool UseCoins(int amount)
    {
        if (amount <= 0 || amount > currentCoins)
        { 
            return false; 
        }

        if (currentCoins >= amount)
        { 
            currentCoins -= amount;
            OnCoinChanged?.Invoke(CurrentCoins);
            
        }
        return true;
    }
}
