using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour {

    private int score = 0;
    private string highScoreKey = "HighScore";

    private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void AddToScore(int additionalScore)
    {
        int highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt(highScoreKey, score);
        }
        score += additionalScore;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
