using System.Collections.Generic;
using UnityEngine;

public class RankingUIManager : MonoBehaviour
{
    [SerializeField] private string gameId = "basketball";

    [SerializeField] private GameObject rankingSlotPrefab;
    [SerializeField] private Transform rankingSlotParent;

    private List<RankingSlotUI> slotUIs = new List<RankingSlotUI>();
    private GameData rankingDataList;

    void Start()
    {
        rankingDataList = AllGameDataManager.Instance.AllGameDatas[gameId];

        // 슬롯 생성
        foreach (RankingEntry rankingEntry in rankingDataList.ranking)
        {
            GameObject slot = Instantiate(rankingSlotPrefab, rankingSlotParent);
            RankingSlotUI ui = slot.GetComponent<RankingSlotUI>();
            ui.SetRanking(rankingEntry);
            slotUIs.Add(ui);
        }
    }

    // UI 갱신 함수
    public void RefreshUI()
    {
        for (int i = 0; i < slotUIs.Count; i++)
        {
            slotUIs[i].SetRanking(rankingDataList.ranking[i]);
        }
    }
}
