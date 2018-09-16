using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreDisplay : MonoBehaviour {

    private Text scoreText;
    private string highScoreKey = "HighScore";

    // Use this for initialization
    void Start()
    {
        scoreText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = PlayerPrefs.GetInt(highScoreKey, 0).ToString();
    }
}
