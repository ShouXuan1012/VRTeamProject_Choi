using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class SlotMachineManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject slotMachineUI;
    public ReelSpinner[] reels;
    public Button spinButton;
    

    [Header("Settings")]
    public int baseBet = 50000;
    public int[] symbolRewards = { 100000, 200000, 1000000, 10000000 };
    public float[] symbolWeights = { 25f, 25f, 25f, 25f };        // 일반 심볼 확률
    public float[] jackpotSymbolChances = { 77.6f, 20f, 2.3f, 0.1f }; // 잭팟 심볼 확률
    public float jackpotRate = 25f;   // 잭팟이 나올 전체 확률(%)

  

    private bool isSpinning = false;

    [Header("RewardUI")]
    public GameObject Background;
    public GameObject rewardUI;
    public Image[] rewardImage;
    public Sprite[] rewardSprites;
    public Text rewardTitleText;
    public Text rewardText;

    void Awake()
    {
        spinButton.onClick.AddListener(() => { if (!isSpinning) StartCoroutine(SpinRoutine()); });
        
    }
    void OnEnable()
    {
        foreach (var reel in reels)
            reel.Init();
    }
    IEnumerator SpinRoutine()
    {
       
        if (!CoinManager.Instance.UseCoins(baseBet))
        {
            yield break; // 코인이 부족하면 중단

        }
        isSpinning = true;
        spinButton.interactable = false;
        // 1️ 결과 뽑기
        int[] results = GetSpinResults();


        // 2️ 릴 회전 (동시에 돌리고 순차 멈춤)
        for (int i = 0; i < reels.Length; i++)
            StartCoroutine(reels[i].Spin(results[i], 1.5f + (i * 0.5f)));

        yield return new WaitForSeconds(1.5f + reels.Length * 0.5f);

        // 3️ 결과 체크
        if (results[0] == results[1] && results[1] == results[2])
        {
            int reward = symbolRewards[results[0]];
            rewardText.text = $"+{reward:N0}";
            //testCoins += reward;
            CoinManager.Instance.AddCoins(reward);
            Background.SetActive(true);
            rewardUI.SetActive(true);
            spinButton.interactable = false;
        }

        isSpinning = false;
        spinButton.interactable = true;
    }

    /// <summary>
    /// 3릴 결과값 생성 (잭팟 확률 포함)
    /// </summary>
    private int[] GetSpinResults()
    {
        int[] results = new int[reels.Length];

        // 🎰 잭팟 모드
        float jackpotRoll = Random.Range(0f, 100f);
        if (jackpotRoll <= jackpotRate)
        {
            int jackpotSymbol = GetWeightedRandomIndex(jackpotSymbolChances);
            for (int i = 0; i < reels.Length; i++)
                results[i] = jackpotSymbol;
            // 잭팟 심볼에 따른 rewardUI처리
            if (jackpotSymbol == 0)
            {
                Reward(0); // 체리

            }
            else if (jackpotSymbol == 1)
            {
                Reward(1); // 포도
            }
            else if (jackpotSymbol == 2)
            {
                Reward(2); // 키위
            }
            else if (jackpotSymbol == 3)
            {
                Reward(3); // 잭팟
            }
        }
        else
        {
            // 3릴 모두 다른 값 강제
            results[0] = GetWeightedRandomIndex(symbolWeights);

            // 2번째 릴 (1번과 같으면 다시 뽑음)
            do
            {
                results[1] = GetWeightedRandomIndex(symbolWeights);
            } while (results[1] == results[0]);

            // 3번째 릴 (1, 2번과 같으면 다시 뽑음)
            do
            {
                results[2] = GetWeightedRandomIndex(symbolWeights);
            } while (results[2] == results[0] || results[2] == results[1]);
        }

        return results;
    }


    /// <summary>
    /// 가중치 랜덤 추출
    /// </summary>
    private int GetWeightedRandomIndex(float[] weights)
    {
        float totalWeight = 0;
        foreach (float w in weights) totalWeight += w;

        float rnd = Random.Range(0, totalWeight);
        float cumulative = 0;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];
            if (rnd <= cumulative)
                return i;
        }
        return weights.Length - 1;
    }

    private void Reward(int index)
    {

        // 보상 이미지 설정
        for (int i = 0; i < rewardImage.Length; i++)
        {
            rewardImage[i].sprite = rewardSprites[index];
        }

        // 보상 텍스트 설정
        switch (index)
        {
            case 0:
                rewardTitleText.text = "Cherry!";
                break;
            case 1:
                rewardTitleText.text = "Grapes!";
                break;
            case 2:
                rewardTitleText.text = "Kiwi";
                break;
            case 3:
                rewardTitleText.text = "JACKPOT!";
                break;
        }
        spinButton.interactable = false;
    }
    
}
