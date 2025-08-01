using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RankingUIManager : MonoBehaviour
{
    [SerializeField] private string gameId = "basketball";

    [SerializeField] private GameObject rankingSlotPrefab;
    [SerializeField] private Transform rankingSlotParent;

    [SerializeField] Button refreshButton;
    [SerializeField] Text lastUpdateText;

    [SerializeField] private int refreshInterval = 10000; // 10초

    private List<RankingSlotUI> slotUIs = new List<RankingSlotUI>();

    private RectTransform buttonRectComponent;
    private bool isRefreshing = false;
    private DateTime lastUpdateTime;

    private void Awake()
    {
        buttonRectComponent = refreshButton.GetComponent<RectTransform>();
    }

    void Start()
    {
        refreshButton.onClick.AddListener(UpdateUI);

        UpdateUI();
    }

    private void Update()
    {
        if (isRefreshing)
        {
            buttonRectComponent.Rotate(0f, 0f, 200f * Time.deltaTime);
        }
    }

    private async void UpdateUI()
    {
        if (isRefreshing) return;

        isRefreshing = true;
        refreshButton.interactable = false;

        Task rankingTask = UpdateRankingSlots();
        Task delayTask = Task.Delay(500); // 최소 0.5초는 대기하도록 설정(너무 빨리 업데이트되면 헷갈릴 여지가 있으므로)

        await Task.WhenAll(rankingTask, delayTask);

        isRefreshing = false;
        lastUpdateTime = DateTime.Now;
        lastUpdateText.text = $"마지막 업데이트 {lastUpdateTime:HH:mm:ss}";

        await Task.Delay(refreshInterval);
        refreshButton.interactable = true;
    }

    private async Task UpdateRankingSlots()
    {
        GameData gameData = await AllGameDataManager.Instance.LoadGameData(gameId);
        List<RankingEntry> rankingEntries = gameData.ranking;

        for (int i = 0; i < slotUIs.Count; i++)
        {
            slotUIs[i].SetRanking(rankingEntries[i], i + 1);
        }
        for (int i = slotUIs.Count; i < rankingEntries.Count; i++)
        {
            GameObject slot = Instantiate(rankingSlotPrefab, rankingSlotParent);
            RankingSlotUI ui = slot.GetComponent<RankingSlotUI>();

            ui.SetRanking(rankingEntries[i], i + 1);
            slotUIs.Add(ui);
        }
    }
}
