using UnityEngine;

public class MagnifierUI : MonoBehaviour
{
    private InspectableItem currentTarget;

    public void SetTarget(InspectableItem item)
    {
        currentTarget = item;
        gameObject.SetActive(true); // µ¸º¸±â UI º¸ÀÌ±â
    }

    public void ClearTarget()
    {
        currentTarget = null;
        gameObject.SetActive(false); // ½Ã¾ß ¹þ¾î³ª¸é ¼û±è
    }

    public void OnClick()
    {
        if (currentTarget != null)
        {
            currentTarget.TryPurchase();
            currentTarget.ShowInfo();
        }
    }
}