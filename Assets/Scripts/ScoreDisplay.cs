using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreDisplay : MonoBehaviour {
    Text scoreText;
    GameSession gameSession;

	// Use this for initialization
	void Start () {
        scoreText = GetComponent<Text>();
        gameSession = GameSession.Instance;
    }
	
	// Update is called once per frame
	void Update () {
        scoreText.text = gameSession.Score.ToString("D7");
    }
}
