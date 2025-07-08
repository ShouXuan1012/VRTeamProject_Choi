using Photon.Pun;
using UnityEngine.EventSystems;

public class EnableLocalEventSystem : MonoBehaviourPun
{
    public EventSystem eventSystem;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            eventSystem.gameObject.SetActive(true);
        }
        else
        {
            eventSystem.gameObject.SetActive(false);
        }
    }
}
