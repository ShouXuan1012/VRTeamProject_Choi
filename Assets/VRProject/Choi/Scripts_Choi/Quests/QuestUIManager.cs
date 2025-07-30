using System.Collections.Generic;
using UnityEngine;

public class QuestUIManager : MonoBehaviour
{
    [SerializeField] private GameObject questSlotPrefab;
    [SerializeField] private Transform questSlotParent;

    private List<QuestSlotUI> slotUIs = new List<QuestSlotUI>();
    private List<QuestData> questDataList;

    void Start()
    {
        // SO 불러오기
        questDataList = new List<QuestData>(Resources.LoadAll<QuestData>("Quests"));

        // 슬롯 생성
        foreach (QuestData quest in questDataList)
        {
            GameObject slot = Instantiate(questSlotPrefab, questSlotParent);
            QuestSlotUI ui = slot.GetComponent<QuestSlotUI>();
            ui.SetQuest(quest);
            slotUIs.Add(ui);
        }
    }

    // UI 갱신 함수
    public void RefreshUI()
    {
        for (int i = 0; i < slotUIs.Count; i++)
        {
            slotUIs[i].SetQuest(questDataList[i]);
        }
    }
}
