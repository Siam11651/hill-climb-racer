using UnityEngine;
using UnityEngine.EventSystems;

public class RevButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Car mCar;

    public void OnPointerDown(PointerEventData eventData)
    {
        mCar.RevDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mCar.RevDown = false;
    }
}
