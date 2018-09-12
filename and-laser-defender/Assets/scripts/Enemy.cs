using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [Header("Stats")]
    [SerializeField]
    private float health = 100;
    [SerializeField]
    private int scoreValue = 150;

    [Header("Shooting")]
    [SerializeField]
    private float shotCounter;  //serialized for debuging porpouses
    [SerializeField]
    private float minTimebetweenShots = 0.2f;
    [SerializeField]
    private float maxTimeBetweenShots = 2f;
    [SerializeField]
    private float enemyLaserSpeed = 10f;
    [SerializeField]
    private GameObject laserPrefab;

    [Header("Sound effects and animation")]
    [SerializeField]
    private GameObject deathVFX;
    [SerializeField]
    private AudioClip deathSFX;
    [SerializeField]
    private AudioClip laserSFX;

    private float durationOfExplosion = 1f;
    private float deathSoundVolume = 0.7f;
    private float laserSoundVolume = 0.3f;

    private void Start()
    {
        ResetShotCounter();
    }

    private void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0f)
        {
            Fire();
            ResetShotCounter();
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(
                laserPrefab, 
                transform.position, 
                Quaternion.identity) as GameObject;

            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -enemyLaserSpeed);
        AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, laserSoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();
        if(!damageDealer)
        {
            return;
        }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.OnHit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSoundVolume);
    }

    private void ResetShotCounter()
    {
        shotCounter = UnityEngine.Random.Range(minTimebetweenShots, maxTimeBetweenShots);
    }
}
