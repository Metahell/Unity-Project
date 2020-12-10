using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagePlayerBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private Animator animator;
    private KnightBehaviour KnightBehaviour;
    [SerializeField]
    private float shootAngle;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(1).IsTag("1"))
        {
            animator.SetTrigger("1st Ability");
        }
    }

    public void Ability1()
    {
        Vector3 direction = transform.forward;
        GameObject fireball = Factory.GetInstance().GetFireball();
        fireball.transform.position = spawnPoint.position;
        fireball.transform.forward = direction;

    }
}
