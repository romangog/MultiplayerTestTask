using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleTouchGraphic : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public  Action PointerDownEvent;
    public  Action PointerUpEvent;

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerDownEvent?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerUpEvent?.Invoke();
    }
}
