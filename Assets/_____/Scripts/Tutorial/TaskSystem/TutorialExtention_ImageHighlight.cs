using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialExtention_ImageHighlight : TutorialExtention
{
    public static TutorialExtention_ImageHighlight Instance;

    public RectTransform PlacebleImage;
    public Transform Target;

    private float targetX = 1080;
    private float targetY = 2400;

    Camera _cam;

    internal override void Setup()
    {
        base.Setup();
        _cam = Camera.main;
        Vector2 canvasSize = this.transform.GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.size;
        targetX = canvasSize.x;
        targetY = canvasSize.y;
    }

    internal override void TurnOn()
    {
        base.TurnOn();
        Instance = this;

    }

    internal override void TurnOff()
    {
        base.TurnOff();
    }

    private void Update()
    {
        float curX = Screen.width;
        float curY = Screen.height;
         Vector2 point = _cam.WorldToScreenPoint(Target.position);
        
        PlacebleImage.anchoredPosition = new Vector2(point.x * (targetX/curX), point.y * (targetY/curY));
        //PlacebleImage.anchoredPosition = _cam.WorldToScreenPoint(Target.position);
    }

}
