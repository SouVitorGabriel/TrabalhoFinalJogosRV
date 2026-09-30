using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _CrackBlock : MonoBehaviour
{
    MovementController player;
    public GameObject crack1;

    public InterfaceManager interfaceManager;

    private int cracks = 1;

    private bool canCrack = true;

    private int localMoves = 0;

    private Vector3 initialpos;

    // Start is called before the first frame update
    void Start()
    {
        initialpos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (interfaceManager.Moviments > (localMoves + 1))
        {
            canCrack = true;
        }
        else
        {
            canCrack = false;
        }
        WhoIsInMe();
        //Debug.Log("Cancrack: " + canCrack + " LocaMoves: " + localMoves + " ManagerMoves: " + interfaceManager.Moviments);
    }

    public void Recomeco()
    {
        transform.localPosition = initialpos;
        localMoves = 0;
        crack1.SetActive(true);
        cracks = 1;
    }

    void WhoIsInMe()
    {
        Ray myRayDown = new Ray(transform.position + new Vector3(0, 0.25f, 0), Vector3.up);
        Debug.DrawRay(myRayDown.origin, myRayDown.direction, Color.black);

        RaycastHit hit2;
        //Debug.DrawRay(ganheiRay.origin, ganheiRay.direction, Color.green);

        if(canCrack)
        {
            if(Physics.Raycast(myRayDown, out hit2, 0.5f))
            {
                if(hit2.collider.tag == "Player")
                {
                    player = hit2.collider.gameObject.GetComponent<MovementController>();
                    localMoves = interfaceManager.Moviments;
                    canCrack = false;
                    //Debug.Log("Player AQUI EM CIMA CARAIO");
                    PleaseCrack();
                }
            }   
        }
    }

    public void PleaseCrack()
    {
        Debug.Log("Entrei aqu ino crak; ");
        if(cracks == 1)
        {
            crack1.SetActive(false);
            cracks = cracks - 1;
        }
        else if(cracks == 0)
        {
            Fall();
            player.Fall();
            
        }
    }

    public void Fall()
    {
        StartCoroutine(CorroutineFall());
    }

    IEnumerator CorroutineFall()
    {
        float timer = 2f;
        do
        {
            timer -= Time.deltaTime;//reduzir o tempo a cada frame
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, transform.localPosition.y -0.75f, timer), transform.localPosition.z);
            yield return new WaitForEndOfFrame();//colocar a coroutine para "dormir"
        }
        while(timer > 0f);
    }

    public int Cracks {get {return cracks;} set{cracks = value;}}
    public bool CanCrack {get {return canCrack;} set{canCrack = value;}}
}
