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
        if (AllGameDataManager.Instance.IsDataLoaded)
        {
            InitUI();
        }
        else
        {
            AllGameDataManager.Instance.OnDataLoaded += InitUI;
        }

    }

    public void InitUI()
    {
        gameData = AllGameDataManager.Instance.GetGameData(gameId);

        List<RankingEntry> rankingEntries = gameData.ranking;

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

    public void UpdateUI()
    {
        for (int i = 0; i < slotUIs.Count; i++)
        {
            slotUIs[i].SetRanking(AllGameDataManager.Instance.GetGameData(gameId).ranking[i], i + 1);
        }
    }
}
