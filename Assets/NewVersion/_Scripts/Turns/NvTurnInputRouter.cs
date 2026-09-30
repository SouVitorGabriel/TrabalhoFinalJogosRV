using NewVersion.Fatia1.Core;
using NewVersion.Fatia1.LegacyBridge;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewVersion.Fatia1.Turns
{
    public class NvTurnInputRouter : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private NvGameFlowController flowController;
        [SerializeField] private NvLegacyTurnBridge legacyTurnBridge;

        [Header("Input Sources")]
        [SerializeField] private bool readKeyboardInput;
        [SerializeField] private bool readSwipeInput = true;

        [Header("Turn Timing")]
        [SerializeField] private float minTurnInterval = 0.22f;

        [Header("Swipe Settings")]
        [SerializeField] private float minSwipeDistance = 80f;

        [Header("Debug")]
        [SerializeField] private bool verboseLogs;

        private bool pointerPressed;
        private Vector2 pointerStart;
        private float nextTurnAllowedAt;

        private void Update()
        {
            if (!CanReadInput())
            {
                return;
            }

            NvTurnDirection direction = ReadDirectionThisFrame();
            if (direction == NvTurnDirection.None)
            {
                return;
            }

            TryIssueTurn(direction, "runtime-input");
        }

        public void QueueUp()
        {
            TryIssueTurn(NvTurnDirection.Up, "ui");
        }

        public void QueueDown()
        {
            TryIssueTurn(NvTurnDirection.Down, "ui");
        }

        public void QueueLeft()
        {
            TryIssueTurn(NvTurnDirection.Left, "ui");
        }

        public void QueueRight()
        {
            TryIssueTurn(NvTurnDirection.Right, "ui");
        }

        private bool CanReadInput()
        {
            if (legacyTurnBridge == null)
            {
                return false;
            }

            if (flowController == null)
            {
                return true;
            }

            return flowController.CurrentState == NvGameState.Playing;
        }

        private NvTurnDirection ReadDirectionThisFrame()
        {
            if (readKeyboardInput)
            {
                NvTurnDirection keyboardDirection = ReadKeyboardDirection();
                if (keyboardDirection != NvTurnDirection.None)
                {
                    return keyboardDirection;
                }
            }

            if (readSwipeInput)
            {
                return ReadSwipeDirection();
            }

            return NvTurnDirection.None;
        }

        private static NvTurnDirection ReadKeyboardDirection()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return NvTurnDirection.None;
            }

            if (keyboard.wKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame)
            {
                return NvTurnDirection.Up;
            }

            if (keyboard.sKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame)
            {
                return NvTurnDirection.Down;
            }

            if (keyboard.aKey.wasPressedThisFrame || keyboard.leftArrowKey.wasPressedThisFrame)
            {
                return NvTurnDirection.Left;
            }

            if (keyboard.dKey.wasPressedThisFrame || keyboard.rightArrowKey.wasPressedThisFrame)
            {
                return NvTurnDirection.Right;
            }

            return NvTurnDirection.None;
        }

        private NvTurnDirection ReadSwipeDirection()
        {
            Touchscreen touch = Touchscreen.current;
            if (touch != null && (touch.primaryTouch.press.isPressed || touch.primaryTouch.press.wasPressedThisFrame || touch.primaryTouch.press.wasReleasedThisFrame))
            {
                return ReadSwipeFromPointer(
                    touch.primaryTouch.position.ReadValue(),
                    touch.primaryTouch.press.wasPressedThisFrame,
                    touch.primaryTouch.press.isPressed,
                    touch.primaryTouch.press.wasReleasedThisFrame);
            }

            Mouse mouse = Mouse.current;
            if (mouse != null)
            {
                return ReadSwipeFromPointer(
                    mouse.position.ReadValue(),
                    mouse.leftButton.wasPressedThisFrame,
                    mouse.leftButton.isPressed,
                    mouse.leftButton.wasReleasedThisFrame);
            }

            return NvTurnDirection.None;
        }

        private NvTurnDirection ReadSwipeFromPointer(Vector2 currentPosition, bool pressedThisFrame, bool pressed, bool releasedThisFrame)
        {
            if (pressedThisFrame)
            {
                pointerPressed = true;
                pointerStart = currentPosition;
            }

            if (!pointerPressed)
            {
                return NvTurnDirection.None;
            }

            Vector2 delta = currentPosition - pointerStart;
            if (delta.magnitude >= minSwipeDistance)
            {
                pointerPressed = false;
                return ResolveDirection(delta);
            }

            if (releasedThisFrame || !pressed)
            {
                pointerPressed = false;
            }

            return NvTurnDirection.None;
        }

        private bool TryIssueTurn(NvTurnDirection direction, string source)
        {
            if (direction == NvTurnDirection.None)
            {
                return false;
            }

            if (Time.time < nextTurnAllowedAt)
            {
                return false;
            }

            if (!legacyTurnBridge.TryIssueTurn(direction))
            {
                return false;
            }

            nextTurnAllowedAt = Time.time + Mathf.Max(0f, minTurnInterval);

            if (verboseLogs)
            {
                Debug.Log($"[NvTurnInputRouter] Turn accepted from {source}: {direction}", this);
            }

            return true;
        }

        private static NvTurnDirection ResolveDirection(Vector2 delta)
        {
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                return delta.x >= 0f ? NvTurnDirection.Right : NvTurnDirection.Left;
            }

            return delta.y >= 0f ? NvTurnDirection.Up : NvTurnDirection.Down;
        }
    }
}
