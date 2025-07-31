using System.Collections.Generic;
using UnityEngine;

public class RankingUIManager : MonoBehaviour
{
    [SerializeField] private string gameId = "basketball";

    [SerializeField] private GameObject rankingSlotPrefab;
    [SerializeField] private Transform rankingSlotParent;

    private List<RankingSlotUI> slotUIs = new List<RankingSlotUI>();
    private GameData gameData;

    void Start()
    {
        gameData = AllGameDataManager.Instance.AllGameDatas[gameId];

        List<RankingEntry> rankingEntries = gameData.ranking;
        rankingEntries.Sort((a, b) => b.score.CompareTo(a.score));

        // 슬롯 생성
        int rank = 1;
        foreach (RankingEntry rankingEntry in rankingEntries)
        {
            GameObject slot = Instantiate(rankingSlotPrefab, rankingSlotParent);
            RankingSlotUI ui = slot.GetComponent<RankingSlotUI>();
            ui.SetRanking(rankingEntry, rank);
            slotUIs.Add(ui);
            rank++;
        }
    }
}
