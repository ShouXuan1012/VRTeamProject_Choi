using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections;

public class WheelManager : MonoBehaviour {

    //Creates the wheel
    SpinWheel wheel = new SpinWheel(8);

   

    const int baseBet = 100000;
    public GameObject Background;
    public GameObject result;
    public Button spinButton;

    public Text ResultText;
    public Image resultImage;
    public Image winImage;
    public Image loseImage;
    public Text scoreT;
    
    public Text costInput;
   
    private int currentCost = baseBet;
    private int getcoin;
    private bool isSpinning = false;
    void Awake () {

        spinButton.onClick.AddListener(() => { if (!isSpinning) StartCoroutine(Spin()); });

        if (costInput != null)
        {
            if (string.IsNullOrWhiteSpace(costInput.text))
            {
                currentCost = baseBet;              
            }
          
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


            
            CoinManager.Instance.AddCoins(getcoin); 

            scoreT.text = $"+{getcoin}";
        });
	}
   
    IEnumerator Spin()
    {

        if (!CoinManager.Instance.UseCoins(currentCost))
        {   
            yield break; // 코인이 부족하면 중단
        }

            isSpinning = true; // 스핀 시작
        spinButton.interactable = false;
        yield return StartCoroutine(wheel.StartNewRun()); // 네 기존 로직


        spinButton.interactable = true;
        isSpinning = false;

    }
    public void AddCost(int amount)
    {
        currentCost = Mathf.Max(baseBet, currentCost + amount);
        if (costInput != null)
            costInput.text = currentCost.ToString() + "원"; // 버튼으로 바꿀 때만 직접 반영
    }


    public void Win()
    {

        resultImage.sprite = winImage.sprite;
        ResultText.text = "Congratulations!";
        Background.SetActive(true);
        result.SetActive(true);
        spinButton.interactable = false; // 스핀 버튼 비활성화
    }

    public void Lose()
    {
        resultImage.sprite = loseImage.sprite;
        ResultText.text = "Nice Try,,,";
        Background.SetActive(true);
        result.SetActive(true);
        spinButton.interactable = false; // 스핀 버튼 비활성화
    }
}
