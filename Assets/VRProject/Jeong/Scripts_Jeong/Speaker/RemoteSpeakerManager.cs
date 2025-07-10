using Photon.Voice.Unity;
using UnityEngine;

public class RemoteSpeakerManager : MonoBehaviour
{
    private Speaker speaker;
    private AudioSource audioSource;
    private bool isMuted = false;

    private void Start()
    {
        speaker = GetComponent<Speaker>();
        audioSource = GetComponent<AudioSource>();
    }
    public void ToggleMute()
    {
        if (audioSource == null) return;

        isMuted = !isMuted;
        audioSource.mute = isMuted;
    }
    public bool IsSpeaking()
    {
        if (speaker == null) return false;

        return speaker.IsPlaying;
    }
}
