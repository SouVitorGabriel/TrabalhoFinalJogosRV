using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _CrackBlock : MonoBehaviour
{
    public GameObject crack1;

    public InterfaceManager interfaceManager;

    private int cracks = 0;

    public bool test;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Crack()
    {
        if(cracks == 2)
        {
            crack1.SetActive(false);
            Cracks = Cracks - 1;
        }
        
    }

    public void Fall()
    {
        StartCoroutine(CorroutineFall());
    }

    IEnumerator CorroutineFall()
    {
        float timer = 1f;
        do
        {
            timer -= Time.deltaTime;//reduzir o tempo a cada frame
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, transform.localPosition.y -0.25f, timer), transform.localPosition.z);

            yield return new WaitForEndOfFrame();//colocar a coroutine para "dormir"
        }
        while(timer > 0f);
    }

    public int Cracks {get {return cracks;} set{cracks = value;}}
}
