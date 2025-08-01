using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

public class DonationButton : MonoBehaviour
{
    [SerializeField] private int donationAmount;
    [SerializeField] private Button button;

    private void Start()
    {
        if (button != null)
            button.onClick.AddListener(Donate);
    }

    private void Donate()
    {
        if (!CoinManager.Instance.UseCoins(donationAmount))
        {
            Debug.Log("[DonationButton] 소지금 부족");
            return;
        }

        Debug.Log($"[DonationButton] {donationAmount}원 후원 성공");

        int countToPlay = (donationAmount == 5000) ? 2 : 6;
        AchievementManager.Instance.AddProgress(EAchievementType.BuskingSupporter, countToPlay);
        QuestEvents.Invoke(EQuestType.PhotoTaken);

        PhotonView particleTargetView = DonationParticleManager.Instance.GetView();
        if (particleTargetView != null)
        {
            particleTargetView.RPC("PlayParticlesRPC", RpcTarget.All, countToPlay);
        }
    }
}
