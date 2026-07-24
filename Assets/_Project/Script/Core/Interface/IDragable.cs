
using UnityEngine;

public interface IDragable
{
    public void OnPointerDown();
    public void OnPointerDrag(Vector3 screenPosition);

    public void OnPointerUp(Vector3 screenPosition);
}
