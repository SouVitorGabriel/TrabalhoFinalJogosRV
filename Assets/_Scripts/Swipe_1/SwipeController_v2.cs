using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController_v2 : MonoBehaviour
{
    [Header("Tweaks")]
    [SerializeField] private float deadZone = 100.0f;
    [SerializeField] private float doubleTapDelta = 0.5f;

    [Header("Lógica dos Clicks")]
    private bool tap, doubleTap, swipeLeft, swipeRight, swipeUp,  swipeDown;
    private Vector2 startTouch, swipeDelta;
    private float lastTap;
    private float sqrDeadzone;
    private bool isDraging = false;

    private void Start()
    {
        sqrDeadzone = deadZone * deadZone;
    }
    private void Update()
    {
        tap = doubleTap = swipeLeft = swipeRight = swipeUp = swipeDown = false;

        #region Unity StandAlone Inputs
            UpdateStandalone();
        #endregion

        #region Mobile Inputs
            UpdateMobile();
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
        if(swipeDelta.sqrMagnitude > sqrDeadzone)
        {
            //é um movimento fora da zona morta, agora qual a direção?
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
            startTouch = swipeDelta = Vector2.zero;
        }
    }

    private void UpdateStandalone()
    {
        if(Input.GetMouseButtonDown(0))
        {
            tap = true;
            isDraging = true;
            startTouch = Input.mousePosition;
            //essa booleana abaixo ja´tem um if inserida dentro da lógica dela
            doubleTap = Time.time - lastTap < doubleTapDelta;
            lastTap = Time.time;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            startTouch = swipeDelta = Vector2.zero;
        }

        //reseta a distancia para verificar se ouve double tap
        swipeDelta = startTouch = Vector2.zero;
    }


    private void UpdateMobile()
    {
        if(Input.touches.Length > 0)
        {
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                startTouch = Input.mousePosition;
                doubleTap = Time.time -lastTap < doubleTapDelta;
                Debug.Log(Time.time - lastTap);
                lastTap = Time.time;
            }
            else if(Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                startTouch =  swipeDelta =  Vector2.zero;
            }
        }
        //reseta de novo
        swipeDelta = startTouch = Vector2.zero;
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
    public bool DoubleTap {get {return doubleTap;}} 
    #endregion
}
