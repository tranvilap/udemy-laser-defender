using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    [Header("Enemy's Properties")]
    [SerializeField] int health = 1;
    [SerializeField] int scoreValue = 100;
    [SerializeField] GameObject enemyDeathVFX;
    [SerializeField] bool hasItem = false;
    [SerializeField] float itemFallingSpeed = 3f;

    [Header("Enemy's Projectiles")]
    [SerializeField] bool isAbleToShoot = true;
    [SerializeField] GameObject enemyProjectilePrefab;
    [SerializeField] float bulletSpeed = 3f;
    [SerializeField] float minTimeBetweenShots = 1f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float shotCounter;
    
    [Header("SFX")]
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathSFXVolume;
    [SerializeField] AudioClip getDamageSFX;
    [SerializeField] [Range(0, 1)] float getDamageSFXVolume = 0.75f;

    public bool IsAbleToShoot
    {
        set
        {
            isAbleToShoot = value;
        }
    }

    public bool HasItem
    {
        set
        {
            hasItem = value;
        }
    }


    // Use this for initialization
    void Start()
    {
        if(isAbleToShoot)
        {
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
            StartCoroutine(FireContinously());
        }
    }
    IEnumerator FireContinously()
    {
        Quaternion rot = enemyProjectilePrefab.transform.rotation;
        while (true)
        {
            yield return new WaitForSeconds(shotCounter);
            Vector2 bulletPos = new Vector2(transform.position.x, transform.position.y - gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2);
            GameObject bullet = Instantiate<GameObject>(enemyProjectilePrefab, bulletPos, rot) as GameObject;
            bullet.GetComponent<Projectile>().StartShootingSound();
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletSpeed);
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }
    void Shoot()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        StartCoroutine(FireContinously());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        var damageDealer = collision.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);

    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.Damage;
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
        else
        {
            if (getDamageSFX != null)
            {
                AudioSource.PlayClipAtPoint(getDamageSFX, Camera.main.transform.position, getDamageSFXVolume);
            }
        }
    }

    private void Die()
    {
        GameSession.Instance.AddToScore(scoreValue);
        Destroy(gameObject);
        Instantiate(enemyDeathVFX, transform.position, Quaternion.identity);
        var main = enemyDeathVFX.GetComponent<ParticleSystem>().main;
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
        main.stopAction = ParticleSystemStopAction.Destroy;
        if(hasItem)
        {
            GameObject item = Instantiate(itemPrefab, this.transform.position, Quaternion.identity);
            item.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -itemFallingSpeed);
        }
    }
}
