using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    public MovementController moveController;
    public SwipeController swipeControls;
    public Transform player;
    private Vector3 desiredPosition;

    public EnemyMovementController enemy;

    private void Update()
    {
        if(swipeControls.SwipeLeft)
        {
            //desiredPosition += Vector3.left;
            moveController.esquerda = true;
            enemy.esquerda = true;
        }
        if(swipeControls.SwipeRight)
        {
            // desiredPosition += Vector3.right;
            moveController.direita = true;
            enemy.direita = true;
        }
        if(swipeControls.SwipeUp)
        {
            //desiredPosition += Vector3.forward;
            moveController.frente = true;
            enemy.atras = true;
        }
        if(swipeControls.SwipeDown)
        {
        //    desiredPosition += Vector3.back;
            moveController.atras = true;
            enemy.frente = true;
        }
        if(swipeControls.DoubleToque)
        {
            Debug.Log("DOIS TOQUES AQUI");
        }
        if(swipeControls.Tap)
        {
            
        }
       //player.transform.position = Vector3.MoveTowards(player.transform.position, desiredPosition, 3f * Time.deltaTime);
    }
}
