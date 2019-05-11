using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LivesDisplay : MonoBehaviour {
    Text livesText;
    Player player;
    // Use this for initialization
    void Start()
    {
        livesText = GetComponent<Text>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            livesText.text = player.Lives.ToString();
        }
        else
        {
            livesText.text = "0";
        }
    }
}
