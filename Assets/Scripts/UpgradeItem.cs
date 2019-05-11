using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeItem : MonoBehaviour {
    [SerializeField] AudioClip pickUpSFX;
    [SerializeField] [Range(0, 1)] float pickUpSFXVolume = 0.4f;
    [SerializeField] int upgradePower = 1;

    public int UpgradePower
    {
        get
        {
            return upgradePower;
        }
    }

    // Use this for initialization

    public void PlayPickUpItemSound()
    {
        AudioSource.PlayClipAtPoint(pickUpSFX, Camera.main.transform.position, pickUpSFXVolume);
    }
}
