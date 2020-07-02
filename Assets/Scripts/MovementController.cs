using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Booleanas de teste")]
    public GameObject ganhou;
    [Header("Booleanas de teste")]
    public bool frente;
    public bool esquerda;
    public bool direita;
    public bool atras;

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
    void Start()
    {
        currentDirection = upOrFront;
        nextPos = Vector3.forward;
        destination = transform.position;
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || frente)
        {
            nextPos = Vector3.forward;
            currentDirection = upOrFront;
            canMove = true;
            frente = false;
        }

        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || atras)
        {
            nextPos = Vector3.back;
            currentDirection = downOrBack;
            canMove = true;
            atras = false;
        }

        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || direita)
        {
            nextPos = Vector3.right;
            currentDirection = right;
            canMove = true;
            direita = false;
        }

        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || esquerda)
        {
            nextPos = Vector3.left;
            currentDirection = left;
            canMove = true;
            esquerda = false;
        }

        if(Vector3.Distance(destination, transform.position) <= 0.00001f)
        {
            //transform.localEulerAngles = currentDirection;
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
            Ganhei();
        }
    }

    bool IsValid()
    {
        Ray myRay = new Ray(transform.position + new Vector3(0, 0.25f, 0), transform.forward);
        RaycastHit hit;

        Debug.DrawRay(myRay.origin, myRay.direction, Color.red);

        if(Physics.Raycast(myRay, out hit, rayLength))
        {
            if(hit.collider.tag == "Wall")
            {
                return false;
            }
            if(hit.collider.tag == "Ganhou")
            {
                ganhou.SetActive(true);
                Debug.Log("GANHEI!");
            }
        }
        return true;
    }

    void Ganhei()
    {
        Ray ganheiRay = new Ray(transform.position + new Vector3(0, 0.25f, 0), transform.position - new Vector3(0,1, 0));
        RaycastHit hit2;
        Debug.DrawRay(ganheiRay.origin, ganheiRay.direction, Color.green);

        if(Physics.Raycast(ganheiRay, out hit2, 0.5f))
        {
            if(hit2.collider.tag == "Ganhou")
            {
                ganhou.SetActive(true);
                Debug.Log("GANHEI!");
            }
        }
    }
}
