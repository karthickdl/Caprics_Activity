using UnityEngine;
using UnityEngine.EventSystems;

public class LeftMoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        PlayerController.Instance.B_moveLeft = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PlayerController.Instance.B_moveLeft = false;
    }
}
