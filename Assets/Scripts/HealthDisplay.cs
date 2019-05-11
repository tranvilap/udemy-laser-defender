using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthDisplay : MonoBehaviour {
    Player player;
    [SerializeField] Image[] hearts; //Your overall Max Health - max health of your game
    //[SerializeField] int maxHealth; //Your current max health - You could increase this number by items, buffs, ....
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<Player>();
        UpdateHealth(player.Health);
	}
	
    public void UpdateHealth(int health)
    {
        int maxHealth = player.PlayerMaxHealth;
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if(i<maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
