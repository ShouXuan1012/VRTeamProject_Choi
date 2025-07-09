using UnityEngine;

public class ItemDetector : MonoBehaviour
{
    public GameObject magnifierUI; // ������ UI ����
    public float maxDistance = 5f;  // ���� �Ÿ� ����
    public float holdDuration = 0.3f;  // ����� �����Ǵ� �ð�
    public float sphereRadius = 0.1f;  // ���� ������
    public LayerMask itemLayerMask;
    public Transform uiParent;

    private InspectableItem currentItem;
    private GameObject currentMagnifier;
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
                    if (currentMagnifier != null)
                    { Destroy(currentMagnifier); }

                    currentMagnifier = Instantiate(magnifierUI, uiParent);
                    var magnifier = currentMagnifier.GetComponent<MagnifierUI>();
                    magnifier.SetTarget(item,uiParent);
                    
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


                if (currentMagnifier != null)
                {
                    Destroy(currentMagnifier);
                    currentMagnifier = null;
                }
            }
        }
    }
}