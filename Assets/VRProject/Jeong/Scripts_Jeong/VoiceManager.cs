using Photon.Voice.Unity;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    public static VoiceManager instance;

    private Recorder recorder;
    private bool isMuted = false;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        recorder = GetComponent<Recorder>();
        if (recorder == null)
        {
            recorder = FindObjectOfType<Recorder>();
        }
    }
    public void ToggleMute()
    {
        if (recorder == null) return;

        isMuted = !isMuted;
        recorder.TransmitEnabled = !isMuted;
    }
    public bool IsMuted()
    {
        return isMuted;
    }

}
