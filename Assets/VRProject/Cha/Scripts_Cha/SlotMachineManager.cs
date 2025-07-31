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
    public int[] symbolRewards = { 50000, 100000, 200000, 1000000 };

    private int symbolCount = 4;      // 심볼 개수
    private bool isSpinning = false;

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
        if (!CoinManager.Instance.UseCoins(baseBet))
        {
            //+잔액부족 UI
            yield break;
        }
        int currentCoins = CoinManager.Instance.CurrentCoins;
        

        isSpinning = true;
        currentCoins -= baseBet;
        
      
        int[] results = new int[reels.Length];
        int[] targetIndexes = new int[reels.Length];

        // 1️⃣ 결과 미리 뽑기
        for (int i = 0; i < reels.Length; i++)
        {
            targetIndexes[i] = Random.Range(0, symbolCount);
        }

        // 2️⃣ 모든 릴 동시에 스핀 시작
        for (int i = 0; i < reels.Length; i++)
        {
            StartCoroutine(reels[i].Spin(targetIndexes[i]));
        }

        // 3️⃣ 릴 순차 멈춤 (왼 → 오)
        for (int i = 0; i < reels.Length; i++)
        {
            yield return new WaitForSeconds(reels[i].stopTime + 0.3f);
            results[i] = targetIndexes[i];
        }

        // 4️⃣ 결과 판정
        if (results[0] == results[1] && results[1] == results[2])
        {
            int reward = symbolRewards[results[0]];
            CoinManager.Instance.AddCoins(reward);
           //보상UI
        }
        else
        {
            //실패UI
        }

        
        isSpinning = false;
    }

    
    void CloseUI()
    {
        gameObject.SetActive(false);
    }
}