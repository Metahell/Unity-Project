using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTileBehaviour : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("It burns !!");
        }
    }
}
