using UnityEngine;
using UnityEngine.UI;

public class RankingSlotUI : MonoBehaviour
{
    [SerializeField] private Text nickName;
    [SerializeField] private Text userId;
    [SerializeField] private Text score;
    [SerializeField] private Image iconImage;

    public void SetRanking(RankingEntry rankingEntry, int rank)
    {
        nickName.text = rankingEntry.nickname;
        userId.text = rankingEntry.userId;
        score.text = rankingEntry.score.ToString();

        if (rank == 1)
        {
            iconImage.color = Color.yellow;
        }
        else if (rank == 2)
        {
            iconImage.color = new Color32(190, 190, 190, 255);
        }
        else if (rank == 3)
        {
            iconImage.color = new Color32(220, 140, 45, 255);
        }
        else
        {
            iconImage.color = new Color32(0, 0, 0, 0);
        }
    }
}
