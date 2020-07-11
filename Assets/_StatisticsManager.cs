using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _StatisticsManager : MonoBehaviour
{

    private int playerMovements;
    public int PlayerMovements {get {return playerMovements;} set {playerMovements = value;}}
    private float levelTime;
    public float LevelTime {get {return levelTime;} set {levelTime = value;}}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void SavePlayerPlay(int level, int moves, float time)
    {
        PlayerPrefs.SetInt("Level" + level + "moves", moves);
        PlayerPrefs.SetFloat("Level" + level + "time", time);
    }
}
