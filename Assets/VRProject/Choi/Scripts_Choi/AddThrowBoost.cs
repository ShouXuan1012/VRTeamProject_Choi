using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class AddThrowBoost : MonoBehaviour
{
    public float boostForce = 1.5f;
    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectExited.AddListener(OnReleased);
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        StartCoroutine(BoostNextFixedUpdate());
    }

    private IEnumerator BoostNextFixedUpdate()
    {
        yield return new WaitForFixedUpdate(); // 던진 뒤 실제 velocity가 적용된 이후
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null && !rb.isKinematic)
        {
            rb.velocity *= boostForce;
        }
    }

    void OnDestroy()
    {
        grab.selectExited.RemoveListener(OnReleased);
    }
}
