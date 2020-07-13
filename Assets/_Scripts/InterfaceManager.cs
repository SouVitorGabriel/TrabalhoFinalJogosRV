using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class InterfaceManager : MonoBehaviour
{
    [Header("Tuner")]
    private _GamePerformanceManager gamePerformanceManager = new _GamePerformanceManager();

    [Header("UI's")]
    public Image img; //uma imagem para fazer fadeout por exemplo
    public GameObject mainMenu;
    public GameObject levelSelectionMenu;
    public GameObject ingameInterface;
    public GameObject finalLevel1Win;
    public GameObject finalLevel1Lose;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI movesText;

    [Header("Controllers")]
    public MovementController playerGG;
    public _ScenarioManager scenarioManager;

    public EnemyMovementController enemy;

    public CinemachineVirtualCamera cineMachineVirtual;

    [Header("Level Positions")]
    public Vector3 level1StartPosition;
    public Vector3 level2StartPosition;
    public Vector3 level1CameraPosition;
    public Vector3 level1CameraRotation;

    float timer = 0f;
    int moviments = 0;
    
    bool ingame;

    int actualLevel;

    void Awake() 
    {
        //gamePerformanceManager.Initialize();
    }
    void Start()
    {
        SetVsync0_60FPS();
        gamePerformanceManager.Initialize();
    }
    
    void Update()
    {
        Debug.Log("Meus movimentos são: " + Moviments);
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

    public void Playlevel2()
    {
        PlayAnimation();
        PreparingLevel2();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Reiniciar()
    {
        ingameInterface.SetActive(false);
        finalLevel1Win.SetActive(false);
        finalLevel1Lose.SetActive(false);
        mainMenu.SetActive(true);
        playerGG.SetPositionStart(level1StartPosition);
        Ingame = false;
        moviments = 0;
        scenarioManager.ResetPortinhas();
        //cineMachineVirtual.Follow = playerGG.gameObject.transform;
        //SceneManager.LoadScene("Inicio");
    }

    //Funções de organização dos levels
    public void PreparingLevel1()
    {
        playerGG.SetPositionStart(level1StartPosition);
        ingameInterface.SetActive(true);
        Ingame = true;
        moviments = 0;
    }

    public void PreparingLevel2()
    {
        playerGG.SetPositionStart(level2StartPosition);
        ingameInterface.SetActive(true);
        Ingame = true;
        moviments = 0;
    }

    // private IEnumerator EndLevel()
    // {

    // }


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


    //Timer do gameplay
    public bool Ingame
{
       get {return ingame;}
       set
       {
            if(value == ingame)
                    return; //isso aqui impede que você set o mesmo valor duas vezes desnecessariamente;
            ingame = value;
            if(ingame)
            {
                    timer = 0f;
                    StartCoroutine(CorroutineTimer());
            }
       }
}
    IEnumerator CorroutineTimer()
    {
        do
        {
                timer += Time.deltaTime;

                float milliseconds = (Mathf.Floor(timer * 100) % 100);
                int seconds = (int)(timer % 60);

                timeText.text = string.Format("{0}.{1}", seconds.ToString("00"), milliseconds.ToString("00"));
                yield return new WaitForEndOfFrame();
        }
        while(ingame);
         //fazer coisas depois que a variavel ingame fica false
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

    public void AddOneMove()
    {
        moviments += 1;
        enemy.esquerda = true;
        if(Ingame)
            movesText.text = moviments.ToString();
    }

    public int Moviments {get{return moviments;}}
}
