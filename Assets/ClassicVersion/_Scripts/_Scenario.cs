using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Scenario : MonoBehaviour
{
    [Header("Configs")]

    public _ButtonLogic[] buttons;
    public GameObject groundTrigger;

    public GameObject[] collidersToOpen;

    public GameObject[] groundsToActivate;
    public bool isNormalOpen;
    [Header("Portinhas")]
    public GameObject doorRight;
    public GameObject doorLeft;

    

    
    //float vel = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ActivateGroundHide()
    {
        if(groundsToActivate.Length > 0)
        {
            foreach(GameObject gO in groundsToActivate)
            {
                gO.SetActive(true);
            }
        }
    }

    public void ActivateGround()
    {
        groundTrigger.tag = "Ganhou";
        foreach(GameObject gO in collidersToOpen)
        {
            gO.SetActive(false);
        }
        AbrirPortinhas();
    }

    public void FecharPortinhas()
    {
        if(!isActiveAndEnabled)
        {
            SetDoorsClosedImmediate();
            return;
        }

        StartCoroutine(CorroutineTimerCloseDoors());
    }

    IEnumerator CorroutineTimerCloseDoors()
    {
        float timer = 1f;
        do
        {
            timer -= Time.deltaTime;//reduzir o tempo a cada frame
            doorLeft.transform.localPosition = new Vector3(Mathf.Lerp(1.1f, 2.72f, timer), doorLeft.transform.localPosition.y, doorLeft.transform.localPosition.z);

            doorRight.transform.localPosition = new Vector3(Mathf.Lerp(-1.1f, -2.72f, timer), doorRight.transform.localPosition.y, doorRight.transform.localPosition.z);

            yield return new WaitForEndOfFrame();//colocar a coroutine para "dormir"
        }
        while(timer > 0f);
    }

    public void ResetPortinhas()
    {
        if(buttons != null && buttons.Length > 0)
        {
            foreach(_ButtonLogic bu in buttons)
            {
                if(bu != null)
                {
                    bu.ResetButton();
                }
            }
        }

        if(groundsToActivate != null && groundsToActivate.Length > 0)
        {
            foreach(GameObject gO in groundsToActivate)
            {
                if(gO != null)
                {
                    gO.SetActive(false);
                }
            }
        }

        if(!isActiveAndEnabled)
        {
            if(isNormalOpen)
            {
                SetDoorsOpenImmediate();
            }
            else
            {
                SetDoorsClosedImmediate();
            }
        }
        else if(isNormalOpen)
        {
            StartCoroutine(CorroutineTimerOpenDoors());
        }
        else
        {
            StartCoroutine(CorroutineTimerCloseDoors());
        }

        if(groundTrigger != null)
        {
            groundTrigger.tag = "Ground";
        }

        foreach(GameObject gO in collidersToOpen)
        {
            if(gO != null)
            {
                gO.SetActive(true);
            }
        }
    }

    public void AbrirPortinhas()
    {
        if(!isActiveAndEnabled)
        {
            SetDoorsOpenImmediate();
            return;
        }

        StartCoroutine(CorroutineTimerOpenDoors());
    }

    IEnumerator CorroutineTimerOpenDoors()
    {
        float timer = 1f;
        do
        {
            timer -= Time.deltaTime;//reduzir o tempo a cada frame
            doorLeft.transform.localPosition = new Vector3(Mathf.Lerp(2.72f, 1.1f, timer), doorLeft.transform.localPosition.y, doorLeft.transform.localPosition.z);

            doorRight.transform.localPosition = new Vector3(Mathf.Lerp(-2.72f, -1.1f, timer), doorRight.transform.localPosition.y, doorRight.transform.localPosition.z);

            yield return new WaitForEndOfFrame();//colocar a coroutine para "dormir"
        }
        while(timer > 0f);
    }

    private void SetDoorsClosedImmediate()
    {
        if(doorLeft != null)
        {
            Vector3 leftPos = doorLeft.transform.localPosition;
            leftPos.x = 1.1f;
            doorLeft.transform.localPosition = leftPos;
        }

        if(doorRight != null)
        {
            Vector3 rightPos = doorRight.transform.localPosition;
            rightPos.x = -1.1f;
            doorRight.transform.localPosition = rightPos;
        }
    }

    private void SetDoorsOpenImmediate()
    {
        if(doorLeft != null)
        {
            Vector3 leftPos = doorLeft.transform.localPosition;
            leftPos.x = 2.72f;
            doorLeft.transform.localPosition = leftPos;
        }

        if(doorRight != null)
        {
            Vector3 rightPos = doorRight.transform.localPosition;
            rightPos.x = -2.72f;
            doorRight.transform.localPosition = rightPos;
        }
    }
}
