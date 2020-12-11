using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour
{
    private Rigidbody rigi;
    private Renderer renderer;
    private KnightBehaviour Health;
    private bool enemyArrow = false;

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
        if (enemyArrow)
        {
            if (other.CompareTag("Player"))
            {
                GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<HealthOrb>().Damage(5);
            }
            if (other.CompareTag("Enemy") || other.CompareTag("Archer"))
                return;
        }
        else
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<KnightBehaviour>().LooseHealth(5);
            }
            else if (other.CompareTag("Archer"))
            {
                other.GetComponent<ArcherBehaviour>().LooseHealth(5);
            }
        }

        Remove();
    }

    public void Remove()
    {
        Factory.GetInstance().RemoveArrow(this);
        enemyArrow = false;
    }

    public void IsEnemyArrow()
    {
        enemyArrow = true;
    }
}