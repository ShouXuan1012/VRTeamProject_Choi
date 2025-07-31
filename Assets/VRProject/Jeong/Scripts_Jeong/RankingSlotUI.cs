using UnityEngine;
using UnityEngine.UI;

public class RankingSlotUI : MonoBehaviour
{
    [SerializeField] private Text nickName;
    [SerializeField] private Text userId;
    [SerializeField] private Text score;
    [SerializeField] private Image iconImage;

    public void SetRanking(RankingEntry rankingEntry)
    {
        nickName.text = rankingEntry.nickname;
        userId.text = rankingEntry.userId;
        score.text = rankingEntry.score.ToString();
        iconImage.color = Color.red;
    }
}
