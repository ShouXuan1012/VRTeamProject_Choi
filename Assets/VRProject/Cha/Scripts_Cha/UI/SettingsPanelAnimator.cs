using UnityEngine;
using DG.Tweening;

public class SettingsMenuAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform mainButton;
    [SerializeField] private RectTransform[] subButtons;
    [SerializeField] private Vector2[] offsets; // 각 버튼이 퍼질 위치
    [SerializeField] private float duration = 0.3f;
    

    private bool isOpen = false;

    void Start()
    {
       
        // 초기에는 숨김
        for (int i = 0; i < subButtons.Length; i++)
        {
            
            subButtons[i].localScale = Vector3.zero*0.02f;
            subButtons[i].anchoredPosition = mainButton.anchoredPosition;
        }
    }

    public void ToggleMenu()
    {
        if (isOpen)
        {
            for (int i = 0; i < subButtons.Length; i++)
            {
                int index = i;
                var cg= subButtons[index].GetComponent<CanvasGroup>(); 
                if(cg !=null)
                {
                    cg.interactable = false;
                    cg.blocksRaycasts = false;
                }
                subButtons[i].DOAnchorPos(mainButton.anchoredPosition, duration).SetEase(Ease.InBack).OnComplete(() =>
                {
                    subButtons[index].gameObject.SetActive(false);
                });
            }
        }
        else
        {
            for (int i = 0; i < subButtons.Length; i++)
            {
                subButtons[i].gameObject.SetActive(true);
                subButtons[i].anchoredPosition = mainButton.anchoredPosition;
                subButtons[i].localScale = Vector3.one * 0.02f;  
                var cg = subButtons[i].GetComponent<CanvasGroup>();
                if(cg != null)
                {
                    cg.interactable = true;
                    cg.blocksRaycasts = true;
                }
                subButtons[i].DOAnchorPos(mainButton.anchoredPosition + offsets[i], duration).SetEase(Ease.OutBack);
            }
        }

        isOpen = !isOpen;
    }
}