using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] AudioClip shootingSFX;
    [SerializeField] [Range(0, 1)] float shootingSFXVolume;
    
    public void StartShootingSound()
    {
        if (shootingSFX != null)
        {
            AudioSource.PlayClipAtPoint(shootingSFX, Camera.main.transform.position,shootingSFXVolume);
        }
    }
}
