using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public void OnPlayButtonClicked(string key)
    {
        UIManager.Instance.OpenUI(key);
    }
}
