using UnityEngine;

public class MagnifierUI : MonoBehaviour
{
    private InspectableItem currentTarget;

    public void SetTarget(InspectableItem item)
    {
        currentTarget = item;
        gameObject.SetActive(true); // ������ UI ���̱�
    }

    public void ClearTarget()
    {
        currentTarget = null;
        gameObject.SetActive(false); // �þ� ����� ����
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