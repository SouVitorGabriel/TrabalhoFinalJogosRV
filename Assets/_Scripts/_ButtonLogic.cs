using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _ButtonLogic : MonoBehaviour
{
    [Header("Configs")]
    public GameObject eixo;
    public bool aberto;
    public bool aberto2;
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
