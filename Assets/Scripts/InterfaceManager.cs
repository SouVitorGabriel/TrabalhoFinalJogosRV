using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    void Start()
    {
        SetVsync2();
    }

    void Update()
    {
        
    }

    public void Sair()
    {
        Application.Quit();
    }

    public void SetVsync2()
    {
        QualitySettings.vSyncCount = 2;
        Application.targetFrameRate =  -1;
    }
}
