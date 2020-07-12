using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _ScenarioManager : MonoBehaviour
{
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

    public void FecharPortinhas()
    {
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
}
