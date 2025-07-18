using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {  get; private set; }
    [System.Serializable]
    public class UIEntry
    {
        public string key;
        public UI prefab;
    }

    [SerializeField] private List<UIEntry> uiPrefabs;
    [SerializeField] private Transform uiParent;

    private Dictionary<string, UI> spawnedUI = new();
    private void Awake()
    {
        if (Instance != null&&Instance!=null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
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
