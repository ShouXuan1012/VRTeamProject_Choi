using UnityEngine;
using static UnityEditor.Progress;

public class ItemDetector : MonoBehaviour
{
    public float maxDistance = 5f;             // 감지 거리
    public float holdDuration = 0.3f;          // 유지 시간
    public float sphereRadius = 0.1f;          // 감지 반지름
    public LayerMask itemLayerMask;            // 감지 대상 레이어

    private InspectableItem currentItem;
    private float lostTime = 0f;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);

        if (Physics.SphereCast(ray, sphereRadius, out RaycastHit hit, maxDistance, itemLayerMask))
        {
            var item = hit.collider.GetComponent<InspectableItem>();
            if (item != null)
            {
                if (item != currentItem)
                {
                    // 이전 아이템 UI 비활성화
                    if (currentItem != null)
                    {
                        currentItem.magnifierUI.ClearTarget();
                        currentItem.magnifierUI.gameObject.SetActive(false);
                    }

                    currentItem = item;

                    currentItem.magnifierUI.gameObject.SetActive(true);
                    currentItem.magnifierUI.SetTarget(currentItem);
                }

                lostTime = 0f;
                return;
            }
        }

        // 감지되지 않음
        if (currentItem != null)
        {
            lostTime += Time.deltaTime;
            if (lostTime > holdDuration)
            {
                currentItem.magnifierUI.ClearTarget();
                currentItem.magnifierUI.gameObject.SetActive(false);
                currentItem = null;
            }
        }
    }
}