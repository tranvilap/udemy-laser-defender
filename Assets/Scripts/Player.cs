using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    //configuration parameters
    [Header("Player Properties")]
    [SerializeField] int playerMaxHealth = 5;
    [SerializeField] int health = 3;
    [SerializeField] int lives = 3;
    [SerializeField] float movingSpeed = 10f;
    [SerializeField] float invincibleTime = 1f;
    [SerializeField] bool isInvincible = false;
    [SerializeField] float blinkTime = 0.1f;
    
    [Header("Upgrades")]
    [SerializeField] [Range(0, 5)] int upgradeLevel = 0;
    [SerializeField] [Range(0, 1)] float firingSpeedFactor = 0.1f;
    [SerializeField] int movingSpeedFactor = 1;
    [SerializeField]int bulletLevel = 1, numSpeedUp= 0, numFiringSpeedup= 0;
    bool isMaxBulletLevel = false;
    bool isMaxSpeed = false;
    bool isMaxFiringSpeed = false;

    [Header("Player's Projectile")]
    [SerializeField] GameObject[] playerProjectilePrefab = null;
    [SerializeField] float bulletSpeed = 1f;
    [SerializeField] float projectileFiringPeriod = 0.75f;

    [Header("Player's SFX")]
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathSFXVolume;
    [SerializeField] AudioClip getDamageSFX;
    [SerializeField] [Range(0, 1)] float getDamageSFXVolume = 0.75f;
    [SerializeField] AudioClip upgradeSFX;
    [SerializeField] [Range(0,1)] float upgradeSFXVolume = 0.75f;

    HealthDisplay healthDisplayer;
    bool isFiring = false;
    float xMin, xMax, yMin, yMax, shipPaddingX, shipPaddingY;
    Coroutine firingCoroutine;
    bool controlable = true;
    public int Health
    {
        get
        {
            return Mathf.Max(health, 0);
        }

        set
        {
            health = value;
        }
    }

    public int Lives
    {
        get
        {
            return lives;
        }
    }

    public int PlayerMaxHealth
    {
        get
        {
            return playerMaxHealth;
        }
    }

    public int UpgradeLevel
    {
        get
        {
            return upgradeLevel;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUpBoundaries();
        healthDisplayer = FindObjectOfType<HealthDisplay>();
        health = playerMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
        Upgrade();
    }

    private void SetUpBoundaries()
    {
        Camera gameCamera = Camera.main;
        shipPaddingX = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        shipPaddingY = GetComponent<SpriteRenderer>().bounds.size.y / 2;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + shipPaddingX;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - shipPaddingX;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + shipPaddingY;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - shipPaddingY;
    }

    private void Move()
    {
        if (controlable)
        {
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * movingSpeed;
                float newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
                float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * movingSpeed;
                float newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
                this.transform.position = new Vector2(newXPos, newYPos);
            }
        }
    }

    private void Shoot()
    {
        if (controlable)
        {
            if (Input.GetButtonDown("Fire1") && !isFiring)
            {
                isFiring = true;
                firingCoroutine = StartCoroutine(FireContinously());
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (firingCoroutine != null)
            {
                StopCoroutine(firingCoroutine);
            }
            isFiring = false;
        }
    }

    IEnumerator FireContinously()
    {
        while (true)
        {
            Vector2 bulletPos = new Vector2(transform.position.x, transform.position.y + shipPaddingY);
            GameObject bullet = Instantiate<GameObject>(playerProjectilePrefab[bulletLevel - 1], bulletPos, Quaternion.identity) as GameObject;
            bullet.GetComponent<Projectile>().StartShootingSound();
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    IEnumerator DamageImmunity()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    IEnumerator BlinkRenderer()
    {
        while (isInvincible)
        {
            gameObject.GetComponent<Renderer>().enabled = !gameObject.GetComponent<Renderer>().enabled;
            yield return new WaitForSeconds(blinkTime);
        }
        gameObject.GetComponent<Renderer>().enabled = true;
    }

    private void Blink()
    {
        StartCoroutine(BlinkRenderer());
    }

    private void ImmuneDamage()
    {
        StartCoroutine(DamageImmunity());
    }

    private IEnumerator ReviveAndMovingTowardScreen()
    {
        bool arrived = false;
        controlable = false;
        health = playerMaxHealth;
        Vector2 respawnPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0));
        respawnPos.y -= 2f;
        transform.position = respawnPos;
        respawnPos.y += 3.15f;
        yield return new WaitForSeconds(0.5f);
        while (!arrived)
        {
            transform.position = Vector2.MoveTowards(transform.position, respawnPos, 7f * Time.deltaTime);
            if (Vector2.Distance(transform.position, respawnPos) == 0)
            {
                arrived = true;
            }
            yield return new WaitForEndOfFrame();
        }
        controlable = true;
    }
    private void Respawn()
    {
        ImmuneDamage();
        Blink();
        StartCoroutine(ReviveAndMovingTowardScreen());
    }
    private void ProcessHit(DamageDealer dmgDealer)
    {
        if (!isInvincible)
        {
            Health -= dmgDealer.Damage;
            dmgDealer.Hit();
            if (Health <= 0)
            {
                Die();
            }
            else
            {
                ImmuneDamage();
                Blink();
                AudioSource.PlayClipAtPoint(getDamageSFX, Camera.main.transform.position, getDamageSFXVolume);
            }
            healthDisplayer.UpdateHealth(health);
        }
        else
        {
            dmgDealer.Hit();
        }
    }

    private void Die()
    {
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
        lives -= 1;
        ResetUpgrade();
        if (lives < 0)
        {
            FindObjectOfType<LevelManager>().LoadGameOver(deathSFX.length + 1f);
            Destroy(gameObject);
        }
        else
        {
            Respawn();
        }
    }

    void PickUpItem(Item item)
    {
        //item.GetPickedUp();
        if (item.gameObject.GetComponent<UpgradeItem>() != null)
        {
            UpgradeItem upgradeItem = item.gameObject.GetComponent<UpgradeItem>();
            upgradeItem.PlayPickUpItemSound();
            int upgradePower = upgradeItem.UpgradePower;
            upgradeLevel += upgradePower;
            if(UpgradeLevel>= 5)
            {
                upgradeLevel = 1;
            }

            Debug.Log("Upgrade Level: " + UpgradeLevel);
        }
        else if (item.gameObject.CompareTag("HealingItem"))
        {
            health++;
        }
    }

    void Upgrade()
    {
        if(Input.GetButtonUp("Upgrade"))
        {
            switch (upgradeLevel)
            {
                case 1:
                    {

                        if(!isMaxSpeed)
                        {
                            AudioSource.PlayClipAtPoint(upgradeSFX, Camera.main.transform.position, upgradeSFXVolume);
                            movingSpeed += movingSpeedFactor;
                            upgradeLevel = 0;
                            numSpeedUp++;
                            if (numSpeedUp >= 3)
                            {
                                isMaxSpeed = true;
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        if(!isMaxFiringSpeed)
                        {
                            AudioSource.PlayClipAtPoint(upgradeSFX, Camera.main.transform.position, upgradeSFXVolume);
                            projectileFiringPeriod -= firingSpeedFactor;
                            upgradeLevel = 0;
                            numFiringSpeedup++;
                            if (numFiringSpeedup >= 3)
                            {
                                isMaxFiringSpeed = true;
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        AudioSource.PlayClipAtPoint(upgradeSFX, Camera.main.transform.position, upgradeSFXVolume);
                        health = playerMaxHealth;
                        healthDisplayer.UpdateHealth(health);
                        upgradeLevel = 0;
                        break;
                    }
                case 4:
                    {
                        if(!isMaxBulletLevel)
                        {
                            AudioSource.PlayClipAtPoint(upgradeSFX, Camera.main.transform.position, upgradeSFXVolume);
                            bulletLevel++;
                            upgradeLevel = 0;
                            if (bulletLevel >= 3)
                            {
                                isMaxBulletLevel = true;
                            }
                        }
                        break;
                    }
            }
        }
    }
    void ResetUpgrade()
    {
        isMaxBulletLevel = false;
        isMaxFiringSpeed = false;
        isMaxSpeed = false;
        movingSpeed -= (movingSpeedFactor * numSpeedUp);
        projectileFiringPeriod += (firingSpeedFactor * numFiringSpeedup);
        bulletLevel = 1;
        numFiringSpeedup = 0;
        numSpeedUp = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            PickUpItem(collision.gameObject.GetComponent<Item>());
        }
        else
        {
            DamageDealer damageDealer = collision.GetComponent<DamageDealer>();
            if (!damageDealer) { return; }
            ProcessHit(damageDealer);
        }
    }
}
