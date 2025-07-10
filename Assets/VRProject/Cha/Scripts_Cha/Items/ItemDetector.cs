using UnityEngine;

public class ItemDetector : MonoBehaviour
{
    public MagnifierUI magnifierUI; // 돋보기 UI 참조
    public float maxDistance = 5f;  // 감지 거리 조절
    public float holdDuration = 0.3f;  // 벗어나도 유지되는 시간
    public float sphereRadius = 0.1f;  // 감지 반지름
    public LayerMask itemLayerMask;
   

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
                    currentItem = item;
                    magnifierUI.gameObject.SetActive(true);
                    magnifierUI.SetTarget(item);
                    
                }
                lostTime = 0f;
                return;
            }
        }

        if (currentItem != null)
        {
            lostTime += Time.deltaTime;
            if (lostTime > holdDuration)
            {
                currentItem = null;
                magnifierUI.ClearTarget();
                magnifierUI.gameObject.SetActive(false);
            }
        }
    }
}