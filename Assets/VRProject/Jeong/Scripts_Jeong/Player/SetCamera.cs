using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class SetCamera : MonoBehaviourPun
{
    public Camera playerCamera;

    void Awake()
    {
        var listener = playerCamera.GetComponent<AudioListener>();
        var poseDriver = playerCamera.GetComponent<UnityEngine.InputSystem.XR.TrackedPoseDriver>(); // ← 추가

        if (photonView.IsMine)
        {
            if (playerCamera != null) playerCamera.enabled = true;
            if (listener != null) listener.enabled = true;
            if (poseDriver != null) poseDriver.enabled = true; // ← 내 것만 작동
        }
        else
        {
            if (playerCamera != null) playerCamera.enabled = false;
            if (listener != null) listener.enabled = false;
            if (poseDriver != null) poseDriver.enabled = false; // ← 상대방은 꺼줘야 함
        }
    }
}
