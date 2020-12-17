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
            if (other.CompareTag("Wolf"))
            {
                GameObject.FindGameObjectsWithTag("Wolf")[0].GetComponent<Wolf>().LooseHealth(5);
            }
            if (other.CompareTag("Enemy") || other.CompareTag("Archer") || other.CompareTag("Boss") || other.CompareTag("Shaman"))
                return;
        }
        else
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<KnightBehaviour>().LooseHealth(10);
            }
            else if (other.CompareTag("Archer"))
            {
                other.GetComponent<ArcherBehaviour>().LooseHealth(10);
            }
            else if (other.gameObject.CompareTag("Boss"))
            {
                other.gameObject.GetComponent<GolemBehavior>().LooseHealth(10);
            }
            else if (other.gameObject.CompareTag("Shaman"))
            {
                other.gameObject.GetComponent<ShamanBehavior>().LooseHealth(10);
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