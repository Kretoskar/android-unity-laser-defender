using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField]
    private float health = 100;

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
            Destroy(gameObject);
        }
    }

    private void ResetShotCounter()
    {
        shotCounter = UnityEngine.Random.Range(minTimebetweenShots, maxTimeBetweenShots);
    }
}
