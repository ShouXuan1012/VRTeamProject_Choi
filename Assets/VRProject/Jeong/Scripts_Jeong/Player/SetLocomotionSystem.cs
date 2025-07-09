using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class SetLocomotionSystem : MonoBehaviourPun
{
    public LocomotionSystem locomotionSystem;

    void Awake()
    {
        if (photonView.IsMine)
        {
            locomotionSystem.gameObject.SetActive(true);
        }
        else
        {
            locomotionSystem.gameObject.SetActive(false);
        }
    }
}
