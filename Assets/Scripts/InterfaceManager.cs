using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    [Header("UI's")]
    public Image img; //uma imagem para fazer fadeout por exemplo
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
        PlayAnimation();
        PreparingLevel1();
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

    //Função do Diego de fade
    public void PlayAnimation()
    {
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        float timer = 0.7f; //tempo da animação
        Color color = img.color;
        do
        {
                timer -= Time.deltaTime;//reduzir o tempo a cada frame
                color.a = Mathf.Lerp(1, 0 , timer/3f);//um lerp para transitar o alpha entre 1 e 0 de acordo com o tempo
                img.color = color;
                yield return new WaitForEndOfFrame();//colocar a coroutine para "dormir"
        }
        while(timer > 0.7f);
        
        levelSelectionMenu.SetActive(false);
        mainMenu.SetActive(false);

        timer = 1f;

        do
        {
                timer -= Time.deltaTime;//reduzir o tempo a cada frame
                color.a = Mathf.Lerp(0, 1 , timer/3f);//um lerp para transitar o alpha entre 1 e 0 de acordo com o tempo
                img.color = color;
                yield return new WaitForEndOfFrame();//colocar a coroutine para "dormir"
        }
        while(timer > 0f);
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
