using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceManager : MonoBehaviour
{
    void Start()
    {
        SetVsync0_60FPS();
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

    public void SetVsync0_60FPS()
    {    
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate =  60;
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene("Inicio");
    }
}
