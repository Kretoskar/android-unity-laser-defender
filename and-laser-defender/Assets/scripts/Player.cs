﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    //config params
    [SerializeField]
    float moveSpeed = 25f;

    [SerializeField]
    float padding = 0.5f;

    [SerializeField]
    float yMaxPadding = 4f;

    [SerializeField]
    GameObject laserPrefab;

    [SerializeField]
    float laserSpeed = 10f;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    private void Start()
    {
        SetUpMoveBoundaries();
    }

    void Update () {
        Move();
        Fire();
	}

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0))    //both for mouse and touching screen
        {
            GameObject laser = Instantiate(
                laserPrefab, 
                transform.position, 
                Quaternion.identity) as GameObject;

            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
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

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - yMaxPadding;
    }

}
