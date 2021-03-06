﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpot : MonoBehaviour
{
    public GameObject[] enemies;
    int enemyNo;
    public float maxPos = 2.2f;
    public float delayTimer = .9f;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = delayTimer;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Vector3 enemyPos = new Vector3(transform.position.x, Random.Range(-4f, 2f), transform.position.z);
            enemyNo = Random.Range(0, 1);
            Instantiate(enemies[enemyNo], enemyPos, transform.rotation);
            timer = delayTimer;
        }

    }
}
