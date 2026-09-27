using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class SwipeController : MonoBehaviour
{
    private float doubleToqueDelta = 0.5f;
    private bool tap, doubleToque, swipeLeft, swipeRight, swipeUp,  swipeDown;
    private Vector2 startTouch, swipeDelta;
    private float lastTap;
    private bool isDraging = false;
    private void Update()
    {
        tap = doubleToque = swipeLeft = swipeRight = swipeUp = swipeDown = false;
        swipeDelta = Vector2.zero;

        TouchControl touch = Touchscreen.current != null ? Touchscreen.current.primaryTouch : null;
        Mouse mouse = Mouse.current;
        bool pointerReleased = false;

        if(touch != null && (touch.press.isPressed || touch.press.wasPressedThisFrame || touch.press.wasReleasedThisFrame))
        {
            if(touch.press.wasPressedThisFrame)
            {
                BeginPointer(touch.position.ReadValue());
            }
            pointerReleased = touch.press.wasReleasedThisFrame;
            if(touch.press.isPressed && isDraging)
            {
                swipeDelta = touch.position.ReadValue() - startTouch;
            }
        }
        else if(mouse != null)
        {
            if(mouse.leftButton.wasPressedThisFrame)
            {
                BeginPointer(mouse.position.ReadValue());
            }
            if(mouse.leftButton.isPressed && isDraging)
            {
                swipeDelta = mouse.position.ReadValue() - startTouch;
            }
            pointerReleased = mouse.leftButton.wasReleasedThisFrame;
        }

        if(swipeDelta.magnitude > 80)
        {
            if(Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                swipeLeft = swipeDelta.x < 0;
                swipeRight = swipeDelta.x > 0;
            }
            else
            {
                swipeDown = swipeDelta.y < 0;
                swipeUp = swipeDelta.y > 0;
            }
            Reset();
        }
        else if(pointerReleased)
        {
            Reset();
        }
    }

    private void BeginPointer(Vector2 position)
    {
        tap = true;
        isDraging = true;
        doubleToque = Time.time - lastTap < doubleToqueDelta;
        lastTap = Time.time;
        startTouch = position;
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
    public bool DoubleToque {get {return doubleToque;}}
    #endregion
}
