using Photon.Voice.Unity;
using UnityEngine;

// 내 보이스 관리
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
    public bool IsMicrophoneAvailable()
    {
        return Microphone.devices != null && Microphone.devices.Length > 0;
    }
    public string GetMicrophoneName()
    {
        if (IsMicrophoneAvailable())
        {
            return Microphone.devices[0];
        }
        else
        {
            return null;
        }
    }
    public bool IsSpeaking()
    {
        if (recorder == null) return false;

        return recorder.IsCurrentlyTransmitting;
    }
}
