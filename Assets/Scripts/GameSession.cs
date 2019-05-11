using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession
{
    private static GameSession instance = null;
    int score = 0;

    public GameSession()
    {
        score = 0;
    }
    void Start()
    {
        int width = 500;
        int height = 900;
        bool isFullscreen = false;
        Screen.SetResolution(width, height, isFullscreen);
    }
    public static GameSession Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameSession();
            }
            return instance;
        }
    }

    public int Score
    {
        get
        {
            return score;
        }
    }
    public void AddToScore(int scoreValue)
    {
       score += scoreValue;
    }
    public void ResetGame()
    {
        instance = new GameSession();
    }
}
