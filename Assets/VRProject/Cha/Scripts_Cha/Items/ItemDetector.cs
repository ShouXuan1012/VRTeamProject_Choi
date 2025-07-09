using UnityEngine;

public class ItemDetector : MonoBehaviour
{
    public GameObject magnifierUI; // ������ UI ����
    public float maxDistance = 5f;  // ���� �Ÿ� ����
    public LayerMask itemLayerMask;
    public Transform uiParent;
    private InspectableItem currentItem;
    private GameObject currentMagnifier;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
       
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, itemLayerMask))
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
                return;
            }
        }

        if (currentItem != null)
        {
            currentItem = null;

            if(currentMagnifier != null)
            { 
                Destroy(currentMagnifier);
                currentMagnifier = null;
            }
        }
    }
}