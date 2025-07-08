

public class CoinManager
{
    private static CoinManager instance;
    public static CoinManager Instance
    {
        get
        {
            if (instance == null)
                instance = new CoinManager();
            return instance;
        }
    }

    private const int MAX_COINS = 316000;
    private int currentCoins;

    public int CurrentCoins => currentCoins;

    private CoinManager()
    {
        currentCoins = MAX_COINS; // 초기 자본
    }

    /// <summary>
    /// 코인을 추가합니다. 최대 금액을 초과할 수 없습니다.
    /// </summary>
    /// <returns>성공 여부</returns>
    public bool AddCoins(int amount)
    {
        if (amount <= 0) return false;

        if (currentCoins + amount > MAX_COINS)
        {
            currentCoins = MAX_COINS;
            return false; // 일부만 반영되었을 수 있음
        }

        currentCoins += amount;
        return true;
    }

    /// <summary>
    /// 코인을 차감합니다. 보유 금액보다 클 경우 실패합니다.
    /// </summary>
    /// <returns>성공 여부</returns>
    public bool UseCoins(int amount)
    {
        if (amount <= 0 || amount > currentCoins)
            return false;

        currentCoins -= amount;
        return true;
    }
}
