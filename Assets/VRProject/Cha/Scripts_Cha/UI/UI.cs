
using System;
using UnityEngine;

public class UI : MonoBehaviour
{
    public event Action OnClosed;
    public virtual void Close()
    {
        OnClosed?.Invoke();
        Destroy(gameObject);
    }
}

