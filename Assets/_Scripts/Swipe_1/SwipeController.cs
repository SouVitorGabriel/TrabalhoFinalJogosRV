using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    private bool tap, swipeLeft, swipeRight, swipeUp,  swipeDown;
    private Vector2 startTouch, swipeDelta;
    private float lastTap;
    private float sqrDeadzone;
    private bool isDraging = false;
    private void Update()
    {
        tap  = swipeLeft = swipeRight = swipeUp = swipeDown = false;

        #region Unity StandAlone Inputs
            if(Input.GetMouseButtonDown(0))
            {
                tap = true;
                isDraging = true;
                startTouch = Input.mousePosition;
            }
            else if(Input.GetMouseButtonUp(0))
            {
                isDraging = false;
                Reset();
            }
        #endregion

        #region Mobile Inputs
        if(Input.touches.Length > 0)
        {
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDraging = true;
            }
            else if(Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDraging = false;
                Reset();
            }
        }

        #endregion

        //Calculando a distancia
        swipeDelta =  Vector2.zero;
        if(isDraging)
        {
            if(Input.touches.Length > 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            else if(Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }

        //Verficando se ultrapassou a zona morta
        if(swipeDelta.magnitude > 80)
        {
            //Agora verificando em qual direção:
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            
            if(Mathf.Abs(x) > Mathf.Abs(y))
            {
                //já que é para os lados, mas qual?
                if(x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;
            }
            else
            {
                //já que é cima ou para qual, qual deles?
                if(y < 0)
                    swipeDown = true;
                else
                    swipeUp = true;
            }
            Reset();
        }
    }
    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }
    
    #region Get Public Properties
    public Vector2 SwipeDelta {get {return swipeDelta;}}
    public bool SwipeLeft {get {return swipeLeft;}}
    public bool SwipeRight {get {return swipeRight;}}
    public bool SwipeUp {get {return swipeUp;}}
    public bool SwipeDown {get {return swipeDown;}}
    public bool Tap {get {return tap;}}
    #endregion
}
