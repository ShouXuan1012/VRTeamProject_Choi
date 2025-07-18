
using UnityEngine;

public class UI : MonoBehaviour
{
    private Transform uiParent;

    public void Open(Transform uiparent)
    {
        this.uiParent = uiparent;
        Instantiate(gameObject,uiparent);
    }
    public void Close()
    {
        Destroy(gameObject);
    }
}
