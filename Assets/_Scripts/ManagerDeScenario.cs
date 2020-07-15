using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerDeScenario : MonoBehaviour
{
    public _Scenario[] scenarios;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FecharPortinhas()
    {
        foreach(_Scenario s in scenarios)
        {
            s.FecharPortinhas();
        }
    }

    public void ResetdePortinhas()
    {  
        foreach(_Scenario s in scenarios)
        {
            s.ResetPortinhas();
        }
    }
}
