﻿using Scenes.RaceToTheCashRegister.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class CheckLight : MonoBehaviour
{
    public bool _isOpen { get; set; } = false;
    public bool _isClosed { get; set; } = false;
    private float timer1, timer2 = 2;
    private int x ;
    private Animator light;
    public static CheckLight instance;
    public RaceGameController race;

    private void Awake()
    {
         instance = this;
    }
  
    // Start is called before the first frame update
    void Start()
    {
        setTimer1();
        light = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (timer1 > 0)
        {
            timer1 -= 5 * Time.deltaTime;
            x = Random.Range(3, 6);
        }

        if (timer1 <= 0 )
        { 
            if (x % 2 == 1)
            {
                light.SetBool("IsOpen", true);
                stopLight();
            }

            else
            {
                light.SetBool("IsClosed", true);
                stopLight();            
            }
        }
        _isOpen = light.GetBool("IsOpen");
        _isClosed = light.GetBool("IsClosed");
    }

    private void setTimer1()
    {
        timer1 = Random.Range(1, 3);
    }

    private void stopLight()
    {
        if (timer2 > 0)
            timer2 -= 3 * Time.deltaTime;
        else
        {
            setTimer1();
            light.SetBool("IsOpen", false);
            light.SetBool("IsClosed", false);
            timer2 = 1.8f;
        }
    }
}