using UnityEngine;
using Photon.Pun;

public class RemoteTransformSync : MonoBehaviourPun, IPunObservable
{
    [Header("전송 여부")]
    [SerializeField] private bool syncPosition = true;
    [SerializeField] private bool syncRotation = true;

    [Header("부드럽게 보간")]
    [SerializeField] private float lerpSpeed = 10f;

    private Vector3 networkPosition;
    private Quaternion networkRotation;

    void Update()
    {
        if (photonView.IsMine) return;

        if (syncPosition)
        {
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * lerpSpeed);
        }

        if (syncRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * lerpSpeed);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (syncPosition) stream.SendNext(transform.position);
            if (syncRotation) stream.SendNext(transform.rotation);
        }
        else
        {
            if (syncPosition) networkPosition = (Vector3)stream.ReceiveNext();
            if (syncRotation) networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
