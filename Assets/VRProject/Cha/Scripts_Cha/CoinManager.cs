

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
        currentCoins = MAX_COINS; // �ʱ� �ں�
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
            return false; // �Ϻθ� �ݿ��Ǿ��� �� ����
        }

        currentCoins += amount;
        return true;
    }

    /// <summary>
    /// ���� ����. ���� �ݾ׺��� Ŭ ��� ����.
    /// </summary>
    /// <returns>���� ����</returns>
    public bool UseCoins(int amount)
    {
        if (amount <= 0 || amount > currentCoins)
            return false;

        currentCoins -= amount;
        return true;
    }
}
