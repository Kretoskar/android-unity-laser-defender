using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    [Header("Player")]
    [SerializeField]
    private float moveSpeed = 25f;

    [SerializeField]
    private float padding = 0.5f;

    [SerializeField]
    private float yMaxPadding = 4f;

    [SerializeField]
    private int health = 500;

    [Header("Projectiles")]

    [SerializeField]
    private GameObject laserPrefab;

    [SerializeField]
    private float laserSpeed = 10f;

    [SerializeField]
    private AudioClip deathSFX;

    [SerializeField]
    private GameObject deathVFX;

    [SerializeField]
    private AudioClip laserSound;

    [SerializeField]
    private float projectileFiringPeriod = 0.3f;

    private float durationOfExplosion = 1f;

    private float deathSoundVolume = 0.7f;

    private float laserSoundVolume = 0.3f;

    private Coroutine firingCoroutine;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    private void Start()
    {
        SetUpMoveBoundaries();
    }

    void Update () {
        Move();
        Fire();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
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
        FindObjectOfType<LevelManager>().LoadGameOver();
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSoundVolume);
    }

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0))    //both for mouse and touching screen
        {
            firingCoroutine = StartCoroutine(FireContinously());
        }
        if(Input.GetMouseButtonUp(0))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinously()
    {
        while (true)
        {
            GameObject laser = Instantiate(
                     laserPrefab,
                     transform.position,
                     Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position, laserSoundVolume);

            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        float deltaX = Input.acceleration.x * Time.deltaTime * moveSpeed;
        float newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);

        float deltaY = Input.acceleration.y * Time.deltaTime * moveSpeed;
        float newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    public int GetHealth()
    {
        return health;
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - yMaxPadding;
    }

}
