using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemyMovementController : MonoBehaviour
{
    [Header("Managers")]
    public InterfaceManager interfaceManager;
    public ManagerDeScenario scenarioManager;

    public MovementController player;

    [Header("Booleanas de teste")]
    public GameObject ganhou;

    public CinemachineVirtualCamera cineMachineVCamera;
    [Header("Booleanas de teste")]
    public bool frente;
    public bool esquerda;
    public bool direita;
    public bool atras;
    private bool frenteLock;
    private bool atrasLock;
    private bool direitaLock;
    private bool esquerdaLock;

    //vetores de rotação para definir para onde o personagem irá olhar:
    Vector3 upOrFront = Vector3.zero;
    Vector3 right = new Vector3(0, 90, 0);
    Vector3 downOrBack = new Vector3(0, 180, 0);
    Vector3 left = new Vector3(0, 270, 0);

    //vetor de direção atual do personagem durante o tempo de jogo:
    Vector3 currentDirection = Vector3.zero;

    //vetores para serem usados para movimentação em si:
    Vector3 nextPos, destination, direction;

    float moveSpeed = 5f;
    float rayLength = 1f;

    bool canMove = false;

    int actualMoves = 0;
    void Start()
    {
        currentDirection = upOrFront;
        nextPos = Vector3.forward;
        destination = transform.position;
    }

    void Update()
    {
        if(actualMoves != interfaceManager.Moviments)
        {
            int r = Random.Range(1, 4);
            //Debug.Log("Random: " + r);
            if(r == 1)
                frente = true;
            if(r == 2)
                atras = true;
            if(r == 3)
                esquerda = true;
            if(r == 4)
                direita = true;
            actualMoves = interfaceManager.Moviments;
        }
        Move();
    }
    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        if(frente)
        {
            nextPos = Vector3.forward;
            currentDirection = upOrFront;
            canMove = true;
            frente = false;
        }

        if(atras)
        {
            nextPos = Vector3.back;
            currentDirection = downOrBack;
            canMove = true;
            atras = false;
        }

        if(direita)
        {
            nextPos = Vector3.right;
            currentDirection = right;
            canMove = true;
            direita = false;
        }

        if(esquerda)
        {
            nextPos = Vector3.left;
            currentDirection = left;
            canMove = true;
            esquerda = false;
        }
        if(Vector3.Distance(destination, transform.position) <= 0.00001f)
        {
           transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(currentDirection), 8f * Time.deltaTime);

            if(canMove)
            {
                if(IsValid())
                {
                    destination = transform.position + nextPos;
                    direction = nextPos;
                    canMove = false;
                }
            }
        }

        EncosteiNoPlayer();
    }

    bool IsValid()
    {
        Ray myRaFront = new Ray(transform.position + new Vector3(0, 0.25f, 0), Vector3.forward);
        Debug.DrawRay(myRaFront.origin, myRaFront.direction, Color.red);

        Ray myRayBack = new Ray(transform.position + new Vector3(0, 0.25f, 0), Vector3.back);
        Debug.DrawRay(myRayBack.origin, myRayBack.direction, Color.magenta);

        Ray myRayRight = new Ray(transform.position + new Vector3(0, 0.25f, 0), Vector3.right);
        Debug.DrawRay(myRayRight.origin, myRayRight.direction, Color.blue);

        Ray myRayLeft = new Ray(transform.position + new Vector3(0, 0.25f, 0), Vector3.left);
        Debug.DrawRay(myRayLeft.origin, myRayLeft.direction, Color.cyan);

        // Ray myRayDown = new Ray(transform.position + new Vector3(0, 0.25f, 0), Vector3.down);
        // Debug.DrawRay(myRayLeft.origin, myRayLeft.direction, Color.black);

        RaycastHit hitF;
        RaycastHit hitB;
        RaycastHit hitL;
        RaycastHit hitR;

        if(currentDirection == upOrFront)
        {
            if(Physics.Raycast(myRaFront, out hitF, rayLength))
            {
                if(hitF.collider.tag == "Wall")
                {
                    return false;
                }
            }
        }
        else if(currentDirection == left)
        {
            if(Physics.Raycast(myRayLeft, out hitL, rayLength))
            {
                if(hitL.collider.tag == "Wall")
                {
                    direita = true;
                    return false;
                }
            }
        }
        else if(currentDirection == right)
        {
            if(Physics.Raycast(myRayRight, out hitR, rayLength))
            {
                if(hitR.collider.tag == "Wall")
                {
                    return false;
                }
            }
        }
        else if(currentDirection == downOrBack)
        {
            if(Physics.Raycast(myRayBack, out hitB, rayLength))
            {
                if(hitB.collider.tag == "Wall")
                {
                    return false;
                }
            }
        }
        return true;        
    }

    void EncosteiNoPlayer()
    {
        Ray damageRay1 = new Ray(transform.position + new Vector3(0, 0.25f, 0), Vector3.forward);
        Debug.DrawRay(damageRay1.origin, damageRay1.direction, Color.yellow);

        Ray damageRay2 = new Ray(transform.position + new Vector3(0, 0.25f, 0), Vector3.back);
        Debug.DrawRay(damageRay2.origin, damageRay2.direction, Color.yellow);
        
        Ray damageRay3 = new Ray(transform.position + new Vector3(0, 0.25f, 0), Vector3.right);
        Debug.DrawRay(damageRay3.origin, damageRay3.direction, Color.yellow);
        
        Ray damageRay4 = new Ray(transform.position + new Vector3(0, 0.25f, 0), Vector3.left);
        Debug.DrawRay(damageRay4.origin, damageRay4.direction, Color.yellow);

        RaycastHit hitDamage1;
        RaycastHit hitDamage2;
        RaycastHit hitDamage3;
        RaycastHit hitDamage4;

        if(Physics.Raycast(damageRay1, out hitDamage1, 0.3f))
        {
            if(hitDamage1.collider.tag == "Player")
            {
                Debug.Log("Bati no Player");
                player.Morri();
            }
        }

        if(Physics.Raycast(damageRay2, out hitDamage2, 0.3f))
        {
            if(hitDamage2.collider.tag == "Player")
            {
                Debug.Log("Bati no Player");
                player.Morri();
            }
        }

        if(Physics.Raycast(damageRay3, out hitDamage3, 0.3f))
        {
            if(hitDamage3.collider.tag == "Player")
            {
                Debug.Log("Bati no Player");
                player.Morri();
            }
        }

        if(Physics.Raycast(damageRay4, out hitDamage4, 0.3f))
        {
            if(hitDamage4.collider.tag == "Player")
            {
                Debug.Log("Bati no Player");
                player.Morri();
            }
        }
    }


    public void SetPositionStart(Vector3 pos)
    {
        cineMachineVCamera.Follow = gameObject.transform;
        currentDirection = upOrFront;
        nextPos = Vector3.forward;
        transform.position = pos;
        destination = pos;
    }
}
