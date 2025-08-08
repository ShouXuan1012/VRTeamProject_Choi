using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class WheelManager : MonoBehaviour {

    //Creates the wheel
    SpinWheel wheel = new SpinWheel(8);

    [Header("Mode Settings")]
    public Toggle testMode;
    public int currentcoin;

    const int baseBet = 100000;
    public GameObject Background;
    public GameObject result;

    public Text ResultText;
    public Image resultImage;
    public Image winImage;
    public Image loseImage;
    public Text scoreT;
    
    public InputField costInput;
    private int currentCost = baseBet;
    private int getcoin;
   
    void Start () {
        //Keep track of the player money
        if (testMode != null && testMode.isOn)
        {
            // ✅ 테스트 모드
            currentcoin = 300000; // 임의 테스트 금액
        }
        
        if (costInput != null)
        { 
            costInput.onValueChanged.AddListener(OnCostChanged); 
        }
        if (costInput != null)
        {
            if (string.IsNullOrWhiteSpace(costInput.text))
                costInput.SetTextWithoutNotify(baseBet.ToString()); 

            currentCost = Mathf.Max(baseBet, ParseCost(costInput.text));
        }
        else
        {
            currentCost = baseBet;
        }
        //Sets the gameobject
        wheel.setWheel(gameObject);

        //Sets the callback
        wheel.AddCallback((index) => {
            
            switch (index)
            {
                case 1:
                    getcoin = currentCost * 2;
                    Win();
                    break;

                case 2:
                    getcoin = currentCost * 1;  
                    break;

                case 3:
                    getcoin = currentCost * 2;
                    Win();            
                    break;

                case 4:
                    getcoin = currentCost * 0;
                    Lose();
                    break;

                case 5:
                    getcoin = currentCost * 1;
                    break;

                case 6:
                    getcoin = currentCost * 0;
                    Lose(); 
                    break;

                case 7:
                    getcoin = currentCost * 3;
                    Win();
                    break;

                case 8:
                    getcoin = currentCost * 0;
                    Lose();
                    break;
            }


            currentcoin += getcoin;
            //CoinManager.Instance.AddCoins(getcoin); // 코인 매니저에 추가

            scoreT.text = $"+{getcoin}";
        });
	}
    private void OnCostChanged(string raw)
    {
        currentCost = Mathf.Max(baseBet,ParseCost(raw));
        
    }
    
    public void Spin()
    {
       
        if (currentCost < 100000)
        {                
            return;
        }

        else if(currentCost>=100000) //(CoinManager.Instance != null)
        {
            currentcoin -= currentCost;
            //CoinManager.Instance.UseCoins(currentCost); // 코인 매니저에서 차감
            StartCoroutine(wheel.StartNewRun()); // 네 기존 로직
   
        }
        
    }
    public void AddCost(int amount)
    {
        currentCost = Mathf.Max(baseBet, currentCost + amount);
        if (costInput != null)
            costInput.text = currentCost.ToString(); // 버튼으로 바꿀 때만 직접 반영
    }

    private int ParseCost(string raw)
    {
        // 숫자만 추출 (공백/문자 대비)
        string cleaned = Regex.Replace(raw ?? "", @"[^\d]", "");
        if (string.IsNullOrEmpty(cleaned)) return 0;
        int.TryParse(cleaned, out int val);
        return val;
    }

    public void Win()
    {

        resultImage.sprite = winImage.sprite;
        ResultText.text = "Congratulations!";
        Background.SetActive(true);
        result.SetActive(true);
    }

    public void Lose()
    {
        resultImage.sprite = loseImage.sprite;
        ResultText.text = "Nice Try,,,";
        Background.SetActive(true);
        result.SetActive(true);
    }
}
