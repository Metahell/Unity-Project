﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Rigidbody rigi;
    private Renderer renderer;
    private KnightBehaviour Health;
    private KnightBehaviour KnightBehaviour;

    [SerializeField]
    private float speed;
    // Use this for initialization
    void Awake()
    {
        rigi = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        rigi.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, 1.5f);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    collider.gameObject.GetComponent<KnightBehaviour>().LooseHealth(5);
                    StartCoroutine(PushKnight(collider.gameObject));
                }
                else if (collider.gameObject.CompareTag("Archer"))
                {
                    collider.gameObject.GetComponent<ArcherBehaviour>().LooseHealth(5);
                }
            }
        }
          Remove();
    }

    public void Remove()
    {
        Factory.GetInstance().RemoveFireball(this);
    }

    IEnumerator PushKnight(GameObject knight)
    {
        knight.GetComponent<KnightBehaviour>().is_pushed = true;
        knight.GetComponent<Rigidbody>().isKinematic = false;
        knight.GetComponent<Rigidbody>().AddForce(transform.forward * 3000);
        yield return new WaitForSeconds(0.5f);
        if (knight != null)
        {
            knight.GetComponent<KnightBehaviour>().is_pushed = false;
        }
    }
}
