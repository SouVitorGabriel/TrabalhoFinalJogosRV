using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceManager : MonoBehaviour
{
    [Header("UI's")]

    public GameObject mainMenu;
    public GameObject levelSelectionMenu;
    public GameObject ingameInterface;
    public GameObject finalLevel1;

    [Header("Objects")]
    public MovementController playerGG;

    [Header("Level Positions")]
    public Vector3 level1StartPosition;


    void Start()
    {
        SetVsync0_60FPS();
    }
    



    //Funções dos botões do Menu Principal
    public void _TurnOnLevelSelection()
    {
        levelSelectionMenu.SetActive(true);
    }

    public void Playlevel1()
    {
        PreparingLevel1();
        levelSelectionMenu.SetActive(false);
        mainMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Reiniciar()
    {
        ingameInterface.SetActive(false);
        finalLevel1.SetActive(false);
        mainMenu.SetActive(true);
        playerGG.SetPositionStart(level1StartPosition);
        //SceneManager.LoadScene("Inicio");
    }

    //Funções de organização dos levels
    public void PreparingLevel1()
    {
        playerGG.SetPositionStart(level1StartPosition);
        ingameInterface.SetActive(true);
    }


    //Funções de VSync
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
}
