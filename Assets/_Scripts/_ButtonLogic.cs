using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class _ButtonLogic : MonoBehaviour
{
    [Header("Configs")]

    public UnityEvent exec;
    public GameObject eixo;

    public _Scenario scenario;

    public string type;
    public bool aberto;
    public bool aberto2;

    private bool alreadyPressed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(aberto)
        {
            OpenButton();
            aberto = false;
        }
        if(aberto2)
        {
            ResetButton();
            aberto2 = false;
        }
        
    }


    public void DoSomething()
    {
        if(!alreadyPressed)
        {
            exec.Invoke();
            OpenButton();
            alreadyPressed = true;
        }

        // if(type == "JustOpenDoor")
        // {
        //     scenario.ActivateGround();
        // }

        
    }


    public void OpenButton()
    {
       StartCoroutine(CorroutineAberto());
    }

    IEnumerator CorroutineAberto()
    {
        float timer = 1f;
        do
        {
            timer -= Time.deltaTime;//reduzir o tempo a cada frame
            eixo.transform.localRotation = Quaternion.Euler(new Vector3(Mathf.Lerp(0f, 180f, timer), eixo.transform.localRotation.y, eixo.transform.localRotation.z));

            yield return new WaitForEndOfFrame();//colocar a coroutine para "dormir"
        }
        while(timer > 0f);
    }

    public void ResetButton()
    {
       StartCoroutine(CorroutineFechado());
       alreadyPressed = false;
    }

    IEnumerator CorroutineFechado()
    {
        float timer = 1f;
        do
        {
            timer -= Time.deltaTime;//reduzir o tempo a cada frame
            eixo.transform.localRotation = Quaternion.Euler(new Vector3(Mathf.Lerp(180f, 0f, timer), eixo.transform.localRotation.y, eixo.transform.localRotation.z));

            yield return new WaitForEndOfFrame();//colocar a coroutine para "dormir"
        }
        while(timer > 0f);
    }
}
