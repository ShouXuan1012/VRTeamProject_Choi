using UnityEngine;

public class VRSeparatedCameraFollower : MonoBehaviour
{
    [SerializeField] private Transform target; // 따라갈 손 위치
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + target.TransformDirection(positionOffset);
        transform.rotation = target.rotation * Quaternion.Euler(rotationOffset);
    }
}
