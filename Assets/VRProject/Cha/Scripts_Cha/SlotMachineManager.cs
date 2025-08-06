using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class SlotMachineManager : MonoBehaviour
{
    [Header("UI References")]
    public ReelSpinner[] reels;       // 3개의 릴
    public Button spinButton;         // 스핀 버튼
    public Button exitButton;         // 종료 버튼

    [Header("Settings")]
    public int baseBet = 50000;       // 고정 베팅 금액
    public int[] symbolRewards = { 100000, 200000, 500000, 10000000 };
    public float[] symbolWeights = { 30f, 15f, 5f,50f };
    public float[] jackpotSymbolChances = { 10f, 3.8f, 0.2f,25f };

    [Header("Debug / Test Mode")]
    public bool testMode = true;          // ✅ 테스트모드 ON/OFF
    public int testCoins = 9999999;       // 테스트용 코인 수량

    private bool isSpinning = false;

    void Start()
    {
        spinButton.onClick.AddListener(() => { if (!isSpinning) StartCoroutine(SpinRoutine()); });
        exitButton.onClick.AddListener(CloseUI);

        foreach (var reel in reels)
        {
            reel.Init();
        }
    }

    IEnumerator SpinRoutine()
    {
        // ✅ 코인 체크
        if (testMode)
        {
            if (testCoins < baseBet)
                yield break;

            testCoins -= baseBet;
        }
        else
        {
            if (!CoinManager.Instance.UseCoins(baseBet))
                yield break;
        }

        isSpinning = true;

        int[] results = new int[reels.Length];

        // 1️⃣ 결과 뽑기
        for (int i = 0; i < reels.Length; i++)
        {
            results[i] = GetWeightedRandomIndex();
        }

        // 2️⃣ 릴 동시에 회전 시작
        for (int i = 0; i < reels.Length; i++)
        {
            StartCoroutine(reels[i].Spin(results[i], 1.5f + (i * 0.5f)));
        }

        // 3️⃣ 릴 순차 멈춤 대기
        yield return new WaitForSeconds(1.5f + (reels.Length * 0.5f));

        // 4️⃣ 결과 판정
        if (results[0] == results[1] && results[1] == results[2])
        {
            int reward = symbolRewards[results[0]];

            if (testMode)
            {
                testCoins += reward;
            }
            else
            {
                CoinManager.Instance.AddCoins(reward);
            }

            // 보상 UI 호출
            Debug.Log($"보상 획득: {reward} 코인");
        }
        else
        {
            // 실패 UI 호출
        }

        isSpinning = false;
    }

    private int GetWeightedRandomIndex()
    {
        float totalWeight = 0;
        foreach (float w in symbolWeights) totalWeight += w;

        float rnd = Random.Range(0, totalWeight);
        float cumulative = 0;

        for (int i = 0; i < symbolWeights.Length; i++)
        {
            cumulative += symbolWeights[i];
            if (rnd <= cumulative)
                return i;
        }
        return symbolWeights.Length - 1;
    }

    void CloseUI()
    {
        gameObject.SetActive(false);
    }
}

