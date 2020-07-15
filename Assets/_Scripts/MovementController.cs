using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MovementController : MonoBehaviour
{
    [Header("Managers")]

    public GameObject eu;
    public InterfaceManager interfaceManager;
    public ManagerDeScenario managerDeScenario;

    [Header("Booleanas de teste")]
    public GameObject ganhou;
    public GameObject perdeu;

    public CinemachineVirtualCamera cineMachineVCamera;
    [Header("Booleanas de teste")]
    public bool frente;
    public bool esquerda;
    public bool direita;
    public bool atras;

    public bool praBaixo;

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

    public bool gameplay = true;
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

            if(praBaixo)
            {
                nextPos = Vector3.down;
                praBaixo = false;
            }

        if(Vector3.Distance(destination, transform.position) <= 0.00001f)
        {
            ////transform.localEulerAngles = currentDirection;


           transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(currentDirection), 8f * Time.deltaTime);

            if(canMove)
            {
                if(IsValid())
                {
                    
                    destination = transform.position + nextPos;
                    direction = nextPos;
                    canMove = false;
                    interfaceManager.AddOneMove();
                    //MarcaUm10(1);
                }
                else
                {
                    canMove = false;
                }
                ButtonCollid();
            }
            Ganhei();
        }
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

    void ButtonCollid()
    {
        Ray myRaF = new Ray(transform.position + new Vector3(0, 0.25f, 0), Vector3.forward);
        Debug.DrawRay(myRaF.origin, myRaF.direction, Color.black);

        Ray myRayB = new Ray(transform.position + new Vector3(0, 0.25f, 0), Vector3.back);
        Debug.DrawRay(myRayB.origin, myRayB.direction, Color.black);

        Ray myRayR = new Ray(transform.position + new Vector3(0, 0.25f, 0), Vector3.right);
        Debug.DrawRay(myRayR.origin, myRayR.direction, Color.black);

        Ray myRayL = new Ray(transform.position + new Vector3(0, 0.25f, 0), Vector3.left);
        Debug.DrawRay(myRayL.origin, myRayL.direction, Color.black);

        // Ray myRayDown = new Ray(transform.position + new Vector3(0, 0.25f, 0), Vector3.down);
        // Debug.DrawRay(myRayLeft.origin, myRayLeft.direction, Color.black);

        RaycastHit hitFf;
        RaycastHit hitBb;
        RaycastHit[] hitLl;
        hitLl = Physics.RaycastAll(transform.position + new Vector3(0, 0.25f, 0), Vector3.left, rayLength);

        RaycastHit hitRr;

        if(Physics.Raycast(myRaF, out hitFf, rayLength))
        {
            if(hitFf.collider.tag == "Button")
            {
                Debug.Log("Beti no bottão");
            }
        }

        foreach(RaycastHit ra in hitLl)
        {
            if(ra.collider.tag == "Button")
            {
                Debug.Log("Pelo amor de deus deu certo");
                _ButtonLogic b = ra.collider.gameObject.GetComponent<_ButtonLogic>();
                b.DoSomething();
            }
        }

        // if(Physics.Raycast(myRayL, out hitLl, rayLength))
        // {
        //     if(hitLl.collider.tag == "Button")
        //     {
        //         Debug.Log("Beti no bottão");
        //     }
        // }

        if(Physics.Raycast(myRayR, out hitRr, rayLength))
        {
            if(hitRr.collider.tag == "Button")
            {
                Debug.Log("Beti no bottão");
            }
        }
        if(Physics.Raycast(myRayB, out hitBb, rayLength))
        {
            if(hitBb.collider.tag == "Button")
            {
                Debug.Log("Beti no bottão");
            }
        }
    }



    void Ganhei()
    {
        Ray myRayDown = new Ray(transform.position + new Vector3(0, 0.25f, 0), -transform.up);
        Debug.DrawRay(myRayDown.origin, myRayDown.direction, Color.black);

        RaycastHit hit2;
        //Debug.DrawRay(ganheiRay.origin, ganheiRay.direction, Color.green);

        if(Physics.Raycast(myRayDown, out hit2, 0.5f))
        {
            if(hit2.collider.tag == "Ganhou")
            {
                interfaceManager.Ingame = false;
                Debug.Log("GANHEI!");
                cineMachineVCamera.Follow = null;
                for (int i = 0; i <= 5; i++)
                {
                    frente = true;
                }
                managerDeScenario.FecharPortinhas();
                Invoke("FunGanhou", 1f);
            }
        }
    }

    void FunGanhou()
    {
        ganhou.SetActive(true);
        interfaceManager.Ingame = false;
    }

    void FunPerdeu()
    {
        perdeu.SetActive(true);
        interfaceManager.Ingame = false;
    }

    public void Morri()
    {
        Debug.Log("Morri");
        perdeu.SetActive(true);
        interfaceManager.Ingame = false;
    }

    public void SetPositionStart(Vector3 pos)
    {
        // cineMachineVCamera.m_Follow = this.transform;
        // CinemachineTransposer cinemachineTransposer = cineMachineVCamera.GetCinemachineComponent<CinemachineTransposer>();
        // cinemachineTransposer.m_FollowOffset =  new Vector3 (1.523998f, 1.81f, -1.950998f);
        currentDirection = upOrFront;
        nextPos = Vector3.forward;
        transform.position = pos;
        destination = pos;
        SetCameraFollow();
    }

    void SetCameraFollow()
    {
        cineMachineVCamera.Follow = eu.transform;
        CinemachineTransposer cinemachineTransposer = cineMachineVCamera.GetCinemachineComponent<CinemachineTransposer>();
        cinemachineTransposer.m_FollowOffset =  new Vector3 (1.523998f, 1.81f, -1.950998f);
    }


    _CrackBlock cBlock;
    void SeraQueCai()
    {
        Ray myRayDown = new Ray(transform.position + new Vector3(0, 0.25f, 0), -transform.up);
        Debug.DrawRay(myRayDown.origin, myRayDown.direction, Color.black);

        RaycastHit hit2;

        if(Physics.Raycast(myRayDown, out hit2, 0.5f))
        {
            if(hit2.collider.tag == "Crackable")
            {
                Debug.Log("To aqui antes de pegar o bloco;");
                cBlock = hit2.collider.gameObject.GetComponent<_CrackBlock>();
                if(cBlock.Cracks == 0)
                {
                    Debug.Log("Pelo visto caí");
                    Invoke("FunGanhou", 3f);
                }
                Debug.Log("Mandei o block crack");
                //cBlock.PleaseCrack(cBlock.Cracks);
                Debug.Log("Bloqueiei o bloco de fazer crack");
                cBlock.CanCrack = false;
            }
        }
    }

    IEnumerator CorroutinePreFall()
    {
        float timer = 0.4f;
        do
        {
            timer -= Time.deltaTime;
            
            if(timer <= 0.2f)
            {
                StartCoroutine(CorroutineFall());
            }

            yield return new WaitForEndOfFrame();//colocar a coroutine para "dormir"
        }
        while(timer > 0f);
    }

    public void Fall()
    {
        StartCoroutine(CorroutinePreFall());
    }


    void OndeTo()
    {
        Ray myRayFall = new Ray(transform.position + new Vector3(0, 0.25f, 0), -transform.up);
        Debug.DrawRay(myRayFall.origin, myRayFall.direction, Color.yellow);

        RaycastHit hit4;
        //Debug.DrawRay(ganheiRay.origin, ganheiRay.direction, Color.green);

        if(Physics.Raycast(myRayFall, out hit4, 0.5f))
        {
            if(hit4.collider.tag == "Crackable")
            {
                Debug.Log("TO EM CIMA DO BLOCO QUE CAI CARAIO");
            }
        }
    }

    IEnumerator CorroutineFall()
    {
        float timer = 1f;
        do
        {
            timer -= Time.deltaTime;//reduzir o tempo a cada frame
            destination = transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, transform.localPosition.y -0.1f, timer), transform.localPosition.z);
            //praBaixo = true;
            if(timer <= 0.2f)
            {
                FunPerdeu();
            }
            yield return new WaitForEndOfFrame();//colocar a coroutine para "dormir"
        }
        while(timer > 0f);
    }

    void MarcaUm10(int n)
    {
        StartCoroutine(CorroutineMarcaUm10(1));
    }
    IEnumerator CorroutineMarcaUm10(int n)
    {
        float timer = 1f;
        do
        {
            timer -= Time.deltaTime;//reduzir o tempo a cada frame

            if(timer <= 0.2f)
            {
                if(n == 1)
                {
                    SeraQueCai();
                }
            }
            yield return new WaitForEndOfFrame();//colocar a coroutine para "dormir"
        }
        while(timer > 0f);
    }
}
