using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTileBehaviour : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HealthOrb>().Damage(100);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<KnightBehaviour>().LooseHealth(100);
        }
        if (collision.gameObject.CompareTag("Archer"))
        {
            collision.gameObject.GetComponent<ArcherBehaviour>().LooseHealth(100);
        }
        if (collision.gameObject.CompareTag("Shaman"))
        {
            collision.gameObject.GetComponent<ShamanBehavior>().LooseHealth(100);
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<ShamanBehavior>().LooseHealth(100);
        }
    }
}
