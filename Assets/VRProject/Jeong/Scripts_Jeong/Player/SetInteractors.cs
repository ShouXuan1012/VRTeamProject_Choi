using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetInteractors : MonoBehaviourPun
{
    [Header("왼손 인터랙터")]
    [SerializeField] private XRBaseInteractor leftPokeInteractor;
    [SerializeField] private XRBaseInteractor leftRayInteractor;
    [SerializeField] private XRBaseInteractor leftDirectInteractor;
    [SerializeField] private XRBaseInteractor leftTeleportInteractor;

    [Header("오른손 인터랙터")]
    [SerializeField] private XRBaseInteractor rightPokeInteractor;
    [SerializeField] private XRBaseInteractor rightRayInteractor;
    [SerializeField] private XRBaseInteractor rightDirectInteractor;
    [SerializeField] private XRBaseInteractor rightTeleportInteractor;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            EnableInteractor(leftPokeInteractor, true);
            EnableInteractor(leftRayInteractor, true);
            EnableInteractor(leftDirectInteractor, true);
            EnableInteractor(leftTeleportInteractor, true);

            EnableInteractor(rightPokeInteractor, true);
            EnableInteractor(rightRayInteractor, true);
            EnableInteractor(rightDirectInteractor, true);
            EnableInteractor(rightTeleportInteractor, true);
        }
        else
        {
            EnableInteractor(leftPokeInteractor, false);
            EnableInteractor(leftRayInteractor, false);
            EnableInteractor(leftDirectInteractor, false);
            EnableInteractor(leftTeleportInteractor, false);

            EnableInteractor(rightPokeInteractor, false);
            EnableInteractor(rightRayInteractor, false);
            EnableInteractor(rightDirectInteractor, false);
            EnableInteractor(rightTeleportInteractor, false);
        }
    }

    private void EnableInteractor(XRBaseInteractor interactor, bool isEnabled)
    {
        if (interactor != null)
            interactor.enabled = isEnabled;
    }
}
