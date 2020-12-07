﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthOrb : MonoBehaviour
{
    [SerializeField]
    private int hpmax=100;
    private int currentHp;
    [SerializeField]
    private GameObject healthOrb;
    [SerializeField]
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = hpmax;
        currentHp = hpmax;
        UpdateOrb();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.Space))
        {
            Damage(100);
        }
    }

    public void Damage(int dmg)
    {
        currentHp -= dmg;
        slider.value = currentHp;
    }
    public void UpdateOrb()
    {
        healthOrb.transform.position = new Vector3(healthOrb.transform.position.x,-(slider.maxValue-slider.value)*1.33f+78.37385f,healthOrb.transform.position.z);
    }
}
