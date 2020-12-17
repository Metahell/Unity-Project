using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private float shootAngle;

    [SerializeField]
    private int bulletNumber;

    [SerializeField]
    private Animator animator;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    /// <summary>
    /// Shoot bullets in a cône
    /// </summary>
    private void Shoot()
    {
        animator.SetTrigger("Shoot");
        Vector3 direction = Quaternion.Euler(0, -shootAngle / 2, 0) * spawnPoint.forward;
        float iteration = shootAngle / bulletNumber;
        for (int i = 0; i < bulletNumber; i++)
        {
            GameObject bullet = BulletFactory.GetInstance().GetBullet();
            bullet.transform.position = spawnPoint.position;
            bullet.transform.up = direction;
            direction = Quaternion.Euler(0, iteration, 0) * direction;
        }
    }
}
