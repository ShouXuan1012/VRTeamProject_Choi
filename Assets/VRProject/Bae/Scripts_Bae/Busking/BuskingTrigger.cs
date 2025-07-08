using UnityEngine;

public class BuskingTrigger : MonoBehaviour
{
    public AudioSource musicSource;
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (musicSource.isPlaying)
            {
                musicSource.Stop();
            }
        }
    }

}
