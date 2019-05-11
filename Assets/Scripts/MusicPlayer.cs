using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] bool dontDestroyOnLoad = false;
    private static MusicPlayer instance = null;

    void Awake()
    {
        SetUpSingleTon();
    }
    void SetUpSingleTon()
    {
        if (dontDestroyOnLoad)
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(instance);
            }
            //instance = this;

            //DontDestroyOnLoad(instance);
        }
        else
        {
            if (instance != null)
            {
                Destroy(instance.gameObject);
            }
        }
    }
}
