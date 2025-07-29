using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    [SerializeField] private GameObject target;

    public void Toggle()
    {
        if (target != null)
            target.SetActive(!target.activeSelf);
    }
}