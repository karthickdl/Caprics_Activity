using UnityEngine;
using UnityEngine.EventSystems;

public class RightMoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        PlayerController.Instance.B_moveRight = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PlayerController.Instance.B_moveRight = false;
    }
}
