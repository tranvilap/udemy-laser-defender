using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeDisplay : MonoBehaviour {
    Player player;
    [SerializeField] Image[] upgrades;
    [SerializeField] Sprite unselectSprite;
    [SerializeField] Sprite selectSprite;
	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        if(player!=null)
        {
            int upgradeLevel = player.GetComponent<Player>().UpgradeLevel - 1;
            ChangeUpgradeSprites(upgradeLevel);
        }
	}
    void ChangeUpgradeSprites(int index)
    {
        for(int i = 0; i < upgrades.Length; i++)
        {
            if(i == index)
            {
                upgrades[i].sprite = selectSprite;
            }
            else
            {
                upgrades[i].sprite = unselectSprite;
            }
        }
    }
}
