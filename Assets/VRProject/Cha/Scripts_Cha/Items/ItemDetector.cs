using UnityEngine;

public class ItemDetector : MonoBehaviour
{
    public MagnifierUI magnifierUI; // ������ UI ����
    public float maxDistance = 5f;  // ���� �Ÿ� ����
    public LayerMask itemLayerMask;

    private InspectableItem currentItem;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, itemLayerMask))
        {
            Debug.Log("Ray�� ���� ������Ʈ: " + hit.collider.name);
            var item = hit.collider.GetComponent<InspectableItem>();
            if (item != null)
            {
                if (item != currentItem)
                {
                    currentItem = item;
                    magnifierUI.SetTarget(item);
                }
                return;
            }
        }

        if (currentItem != null)
        {
            currentItem = null;
            magnifierUI.ClearTarget();
        }
    }
}