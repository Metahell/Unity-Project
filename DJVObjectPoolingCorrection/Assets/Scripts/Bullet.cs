﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {

    private Rigidbody rigi;
    private Renderer renderer;

    [SerializeField]
    private float speed;
	// Use this for initialization
	void Awake ()
    {
        rigi = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        renderer.material.color = Random.ColorHSV();
	}

    private void FixedUpdate()
    {
        rigi.velocity = transform.up * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Remove();
    }

    public void Remove()
    {
        BulletFactory.GetInstance().RemoveBullet(this);
    }

    private void OnEnable()
    {
        renderer.material.color = Random.ColorHSV();
    }

    private void OnDisable()
    {
        
    }
}
