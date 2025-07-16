using UnityEngine;

public class PlayerZoneTracker : MonoBehaviour
{
    public static bool IsInTwoPointZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            IsInTwoPointZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            IsInTwoPointZone = false;
    }
}
