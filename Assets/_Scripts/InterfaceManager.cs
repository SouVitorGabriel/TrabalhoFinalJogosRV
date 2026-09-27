using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.Cinemachine;

public class InterfaceManager : MonoBehaviour
{
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
    public CinemachineVirtualCameraBase cineMachineVirtual;

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
    bool isTransitioning;

    int actualLevel;

    void Awake() 
    {
        //gamePerformanceManager.Initialize();
    }
    void Start()
    {
        SetVsync0_60FPS();
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
        StartLevel(1);
    }

    public void Playlevel2()
    {
        StartLevel(2);
    }

    public void PlayLevel3()
    {
        StartLevel(3);
    }

    public void PlayLevel4()
    {
        StartLevel(4);
    }

    void StartLevel(int level)
    {
        if(isTransitioning)
            return;

        currentLevel = level;
        StartCoroutine(Transition(() => PrepareLevel(level)));
    }

    void PrepareLevel(int level)
    {
        switch(level)
        {
            case 1: PreparingLevel1(); break;
            case 2: PreparingLevel2(); break;
            case 3: PreparingLevel3(); break;
            case 4: PreparingLevel4(); break;
        }
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
        if(isTransitioning)
            return;

        StartCoroutine(Transition(() => ResetGame(i)));
    }

    void ResetGame(int i)
    {
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
            PrepareLevel(currentLevel);
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
        if(!isTransitioning)
            StartCoroutine(Transition(null));
    }

    private IEnumerator Transition(Action onCovered)
    {
        Color color = img.color;
        color.a = 0f;
        img.color = color;
        img.raycastTarget = true;
        isTransitioning = true;

        yield return FadeImage(1f, 0.15f);
        onCovered?.Invoke();
        yield return null;
        yield return new WaitForSecondsRealtime(0.2f);
        yield return FadeImage(0f, 0.15f);

        img.raycastTarget = false;
        isTransitioning = false;
    }

    private IEnumerator FadeImage(float targetAlpha, float duration)
    {
        float startAlpha = img.color.a;
        float elapsed = 0f;

        while(elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            Color color = img.color;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, Mathf.Clamp01(elapsed / duration));
            img.color = color;
            yield return null;
        }

        Color finalColor = img.color;
        finalColor.a = targetAlpha;
        img.color = finalColor;
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
