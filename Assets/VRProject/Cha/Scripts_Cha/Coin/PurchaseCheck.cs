using System;
using UnityEngine;

public class PurchaseCheck : MonoBehaviour
{
    private Action confirmAction;

    public void SetOnConfirm(Action action)
    {
        confirmAction = action;
    }

    public void Confirm()
    {
        confirmAction?.Invoke();
    }
}
