using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    public MovementController moveController;
    public SwipeController swipeControls;
    public Transform player;
    private Vector3 desiredPosition;

    private void Update()
    {
        if(swipeControls.SwipeLeft)
        {
            //desiredPosition += Vector3.left;
            moveController.esquerda = true;
        }
        if(swipeControls.SwipeRight)
        {
            // desiredPosition += Vector3.right;
            moveController.direita = true;
        }
        if(swipeControls.SwipeUp)
        {
            //desiredPosition += Vector3.forward;
            moveController.frente = true;
        }
        if(swipeControls.SwipeDown)
        {
        //    desiredPosition += Vector3.back;
            moveController.atras = true;
        }
        
       //player.transform.position = Vector3.MoveTowards(player.transform.position, desiredPosition, 3f * Time.deltaTime);
    }
}
