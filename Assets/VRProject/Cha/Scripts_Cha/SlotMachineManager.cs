using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SlotMachineManager : MonoBehaviour
{
    [Header("UI References")]
    public ReelSpinner[] reels;       // 3개의 릴
    public Button spinButton;         // 스핀 버튼
    public Button exitButton;         // 종료 버튼

    [Header("Settings")]
    public int baseBet = 50000;          // 고정 베팅 금액

    // 심볼별 보상 (index 0~3)
    public int[] symbolRewards = { 100000, 200000, 500000, 10000000 };
    public float[] symbolWeights = { 50f, 30f, 15f, 5f };
    public float[] jackpotSymbolChances = { 25f, 10f, 3.8f, 0.2f };
    

    private int symbolCount = 4;      // 심볼 개수
    private bool isSpinning = false;
    private int playerCoins = 9999999; // 테스트 용 플레이어 코인 수
    void Start()
    {
        spinButton.onClick.AddListener(() => { if (!isSpinning) StartCoroutine(SpinRoutine()); });
        exitButton.onClick.AddListener(CloseUI);

        foreach (var reel in reels)
        {
            reel.Init(symbolCount);
        }

       
        
    }

    IEnumerator SpinRoutine()
    {
        if (playerCoins<baseBet)//!CoinManager.Instance.UseCoins(baseBet))
        {
            //+잔액부족 UI
            yield break;
        }
        //int currentCoins = CoinManager.Instance.CurrentCoins;
        

        isSpinning = true;
        //currentCoins -= baseBet;
        playerCoins -= baseBet; // 테스트 용 플레이어 코인 수 감소

        
        int[] results = new int[reels.Length];
        int[] targetIndexes = new int[reels.Length];

        // 1️⃣ 결과 미리 뽑기

        for (int i = 0; i < reels.Length; i++)
        { results[i] = GetWeightedRandomIndex(); }


        // 2️⃣ 모든 릴 동시에 스핀 시작
        for (int i = 0; i < reels.Length; i++)
        {
            StartCoroutine(reels[i].Spin(results[i], 1.5f + (i * 0.5f)));
        }

        // 3️⃣ 릴 순차 멈춤 (왼 → 오)
        yield return new WaitForSeconds(1.5f + (reels.Length * 0.5f));

        // 4️⃣ 결과 판정
        if (results[0] == results[1] && results[1] == results[2])
        {
            int reward = symbolRewards[results[0]];
            //CoinManager.Instance.AddCoins(reward);
            playerCoins += reward; // 테스트 용 플레이어 코인 수 증가
            //+보상UI
        }
        else
        {
            //실패UI
        }

        
        isSpinning = false;
    }
    private int[] GetSpinResult()
    {
        int[] result = new int[reels.Length];
        bool isJackpot = false;
        int jackpotSymbol = -1;

        float randJackpot = Random.Range(0f, 100f);
        float cumulativeJackpot= 0f;
        // 1️⃣ 잭팟 여부 체크
        for (int i = 0; i < jackpotSymbolChances.Length; i++)
        {
            cumulativeJackpot += jackpotSymbolChances[i];
            if (randJackpot <= cumulativeJackpot)
            {
                isJackpot = true;
                jackpotSymbol = i;
                break;
            }
        }
        if(isJackpot&&jackpotSymbol >= 0)
        {
            // 잭팟 심볼로 결과 설정
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = jackpotSymbol;
            }
            return result;
        }
        float totalWeight = 0f;
        foreach(float w in symbolWeights)
        {
            totalWeight += w;
        }
        for(int i = 0; i < result.Length; i++)
        {
            // 2️⃣ 심볼별 가중치 기반 랜덤 선택
            float randValue = Random.Range(0f, totalWeight);
            float cumulativeWeight = 0f;
            for (int j = 0; j < symbolWeights.Length; j++)
            {
                cumulativeWeight += symbolWeights[j];
                if (randValue <= cumulativeWeight)
                {
                    result[i] = j;
                    break;
                }
            }
        }
        return result;
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