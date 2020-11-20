﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private Transform player;

    private Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - player.position;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + offset, .2f);
	}
}