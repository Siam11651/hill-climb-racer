using UnityEngine;
using UnityEngine.EventSystems;

public class AccButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Car mCar;

    public void OnPointerDown(PointerEventData eventData)
    {
        mCar.AccDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mCar.AccDown = false;
    }
}
