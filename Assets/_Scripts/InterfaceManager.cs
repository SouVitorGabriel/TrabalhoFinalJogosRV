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
    public ManagerDeScenario managerDeScenario;
    public CinemachineVirtualCamera cineMachineVirtual;

    public CrackBlockManager cracksManager;

    public EnemyMovementController enemy;

    [Header("Level Positions")]
    public Vector3 level1StartPosition;
    public Vector3 level2StartPosition;
    public Vector3 level3StartPosition;
    public Vector3 level4StartPosition;
    public Vector3 level1CameraPosition;
    public Vector3 level1CameraRotation;

    float timer = 0f;
    int moviments = 0;

    int currentLevel = 0;
    
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
        if(ingame == false)
        {
            playerGG.gameplay = false;
        }
        else
        {
            playerGG.gameplay = true;
        }
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
        currentLevel = 1;
    }

    public void Playlevel2()
    {
        PlayAnimation();
        PreparingLevel2();
        currentLevel = 2;
    }

    public void PlayLevel3()
    {
        PlayAnimation();
        PreparingLevel3();
        currentLevel = 3;
    }

    public void PlayLevel4()
    {
        PlayAnimation();
        PreparingLevel4();
        currentLevel = 4;
    }

    void PreparingLevel3()
    {
        playerGG.SetPositionStart(level3StartPosition);
        //levelSelectionMenu.SetActive(false);
        mainMenu.SetActive(false);
        ingameInterface.SetActive(true);
        Ingame = true;
        moviments = 0;
    }

    void PreparingLevel4()
    {
        playerGG.SetPositionStart(level4StartPosition);
        //levelSelectionMenu.SetActive(false);
        mainMenu.SetActive(false);
        ingameInterface.SetActive(true);
        Ingame = true;
        moviments = 0;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OffSelectLevel()
    {
        levelSelectionMenu.SetActive(false);
    }

    public void Reiniciar(int i = 0)
    {
        PlayAnimation();
        enemy.SetPositionStart(new Vector3(-100.213f, 0f, 111.15f));
        ingameInterface.SetActive(false);
        finalLevel1Win.SetActive(false);
        finalLevel1Lose.SetActive(false);
        mainMenu.SetActive(true);
        playerGG.SetPositionStart(level1StartPosition);
        Ingame = false;
        moviments = 0;
        managerDeScenario.ResetdePortinhas();
        cracksManager.ResetAll();
        if(i == 1)
        {
            levelSelectionMenu.SetActive(false);
        }
        if(i == 2)
        {
            if(currentLevel == 1)
            {
                Playlevel1();
            }
            else if(currentLevel == 2)
            {
                Playlevel2();
            }
            else if(currentLevel == 3)
            {
                PlayLevel3();
            }
            else if(currentLevel == 4)
            {
                PlayLevel4();
            }
        }
        //cineMachineVirtual.Follow = playerGG.gameObject.transform;
        //SceneManager.LoadScene("Inicio");
    }

    //Funções de organização dos levels
    public void PreparingLevel1()
    {
        playerGG.SetPositionStart(level1StartPosition);
        //levelSelectionMenu.SetActive(false);
        mainMenu.SetActive(false);
        ingameInterface.SetActive(true);
        Ingame = true;
        moviments = 0;
    }

    public void PreparingLevel2()
    {
        playerGG.SetPositionStart(level2StartPosition);
        //levelSelectionMenu.SetActive(false);
        mainMenu.SetActive(false);
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
        StartCoroutine(Transition_p1());
    }

    private IEnumerator Transition_p1()
    {
        float timer = 0.1f; //tempo da animação
        Color color = img.color;
        do
        {
                timer -= Time.deltaTime;

                color.a = Mathf.Lerp(1, 0 , timer);

                img.color = color;
                if(timer <= 0.01)
                {
                    StartCoroutine(Transition_p2());
                }
                yield return new WaitForEndOfFrame();//colocar a coroutine para "dormir"
        }
        while(timer > 0f);
    }

    private IEnumerator Transition_p2()
    {
        float timer = 0.5f; //tempo da animação
        Color color = img.color;
        do
        {
                timer -= Time.deltaTime;

                color = Color.Lerp(color, Color.white , timer);

                img.color = color;
                if(timer <= 0.01)
                {
                    StartCoroutine(Transition_p3());
                }
                yield return new WaitForEndOfFrame();//colocar a coroutine para "dormir"
        }
        while(timer > 0f);
    }

    private IEnumerator Transition_p3()
    {
        float timer = 0.5f; //tempo da animação
        Color color = img.color;
        do
        {
                timer -= Time.deltaTime;

                color = Color.Lerp(color, Color.black , timer);

                img.color = color;
                if(timer <= 0.01)
                {
                    StartCoroutine(Transition_p4());
                }
                yield return new WaitForEndOfFrame();//colocar a coroutine para "dormir"
        }
        while(timer > 0f);
    }

    private IEnumerator Transition_p4()
    {
        float timer = 0.5f; //tempo da animação
        Color color = img.color;
        do
        {
                timer -= Time.deltaTime;
                color.a = Mathf.Lerp(0, 1 , timer);

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
        if(ingame)
        {
            movesText.text = moviments.ToString();
        }
        
    }

    public int Moviments {get{return moviments;}}
}
