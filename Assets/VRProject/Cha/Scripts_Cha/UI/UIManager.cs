using System;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviourPun
{
    public static UIManager Instance { get; private set; }
    [System.Serializable]
    public class UIEntry
    {
        public string key;
        public UI prefab;
    }

    [SerializeField] private List<UIEntry> uiPrefabs;
    private Transform uiParent;

    private Dictionary<string, UI> spawnedUI = new();
    private void Awake()
    {
        if (Instance != null&&Instance!=null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        PlayerSpawner.OnPlayerSpawned += OnPlayerSpawned;
    }

    void OnDisable()
    {
        PlayerSpawner.OnPlayerSpawned -= OnPlayerSpawned;
    }

    private void OnPlayerSpawned(GameObject player)
    {        
        // 플레이어 하위에서 MainUI 찾기
        Transform found = player.transform.Find("UI/MainUI");
        if (found != null)
        {
            uiParent = found;
            Debug.Log("[UIManager] MainUI 연결성공");
        }
        else
        {
            Debug.LogWarning("[UIManager] 플레이어 하위에서 MainUI를 찾을 수 없습니다.");
        }
    }

    public void OpenUI(string key)
    {
        if (spawnedUI.ContainsKey(key)) return;

        UIEntry entry = uiPrefabs.Find(e => e.key == key);
        if (entry==null||entry.prefab==null)
        {
            return;
        }
        UI instance = Instantiate(entry.prefab,uiParent);
        spawnedUI[key] = instance;
        instance.OnClosed += () => { spawnedUI.Remove(key); };
    }

    public void CloseUI(string key)
    {
        if(!spawnedUI.ContainsKey(key)) return;

        spawnedUI[key].Close();
    }
    public void SpawnDynamicUI(GameObject prefab, Transform parent, Action<GameObject>onInitialized=null)
    {
        GameObject instance = Instantiate(prefab, parent);
        onInitialized?.Invoke(instance);
    }

}
