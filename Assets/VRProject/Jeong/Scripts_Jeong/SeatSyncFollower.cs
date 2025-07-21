using Photon.Pun;
using UnityEngine;

public class SeatSyncFollower : MonoBehaviourPun
{
    private Transform busTransform;
    private Transform[] seatPositions;

    private int seatIndex = -1;
    private bool isSeated = false;

    public void Initialize(Transform bus, Transform[] seats)
    {
        if (bus == null || seats == null) return;

        busTransform = bus;
        seatPositions = seats;
    }

    public void AssignSeat(int index)
    {
        isSeated = true;
        seatIndex = index;
        enabled = true;
    }

    public void ClearSeat()
    {
        isSeated = false;
        seatIndex = -1;
        enabled = false;
    }

    private void Update()
    {
        if (!isSeated || photonView.IsMine || seatPositions == null || busTransform == null)
            return;

        Transform seat = seatPositions[seatIndex];
        transform.position = seat.position;
        transform.rotation = seat.rotation;
    }
}