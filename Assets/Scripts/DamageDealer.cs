using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour {
    [SerializeField] bool isEnemy = false;
    [SerializeField] int damage = 1;

    public int Damage
    {
        get
        {
            return damage;
        }
    }

    public void Hit()
    {
        if(!isEnemy)
            Destroy(gameObject);
    }
}
