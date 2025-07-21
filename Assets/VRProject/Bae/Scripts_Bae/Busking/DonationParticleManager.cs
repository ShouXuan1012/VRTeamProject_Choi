using UnityEngine;
using Photon.Pun;

public class DonationParticleManager : MonoBehaviourPun
{
    public static DonationParticleManager Instance { get; private set; }

    [SerializeField] private ParticleSystem[] particles; // 씬에서 6개 넣어놓기

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public PhotonView GetView()
    {
        return photonView;
    }

    [PunRPC]
    public void PlayParticlesRPC(int count)
    {
        for (int i = 0; i < particles.Length; i++)
        {
            if (i < count && particles[i] != null)
                particles[i].Play();
        }
    }
}
