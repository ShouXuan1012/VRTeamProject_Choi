using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class EnableLocalXRController : MonoBehaviourPun
{
    public XRBaseController gazeInteractor;
    public XRBaseController leftController;
    public XRBaseController rightController;

    void Awake()
    {
        if (photonView.IsMine)
        {
            gazeInteractor.enabled = true;
            leftController.enabled = true;
            rightController.enabled = true;
        }
        else
        {
            gazeInteractor.enabled = false;
            leftController.enabled = false;
            rightController.enabled = false;
        }
    }
}
