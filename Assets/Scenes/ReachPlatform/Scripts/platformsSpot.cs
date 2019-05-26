﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformsSpot : MonoBehaviour
{
    public bool platforms_start = false;
    public GameObject[] platforms;
    int paltformNo;
    //public float maxPos = 4.2f;
    public float delayTimer;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = delayTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (platforms_start == true)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Vector3 platformPos = new Vector3(Random.Range(-6f, 6f), transform.position.y, transform.position.z);
                paltformNo = Random.Range(0, 5);
                Instantiate(platforms[paltformNo], platformPos, transform.rotation);
                timer = delayTimer;
            }
        }

    }
}
