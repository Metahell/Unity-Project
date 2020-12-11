﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private float lifetime = 5f;
    private float start;
    // Start is called before the first frame update
    void Start()
    {
        start = Time.time;
        Debug.Log("start");
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        Debug.Log("" + lifetime);
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}