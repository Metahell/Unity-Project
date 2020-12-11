using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DruidPlayerBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private Animator animator;
    private KnightBehaviour KnightBehaviour;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(1).IsTag("1"))
        {
            animator.SetTrigger("1st Ability");
        }
        if (Input.GetMouseButtonDown(1) && !animator.GetCurrentAnimatorStateInfo(1).IsTag("1"))
        {
            animator.SetTrigger("2nd Ability");
        }
        if (Input.GetKey(KeyCode.Space) && !animator.GetCurrentAnimatorStateInfo(1).IsTag("1"))
        {
            animator.SetTrigger("3rd Ability");
        }
    }

    public void Ability1()
    {
        Vector3 direction = transform.forward;
        GameObject axe = Factory.GetInstance().GetAxe();
        axe.transform.position = spawnPoint.position;
        axe.GetComponent<Axe>().setDirection(transform.forward);
        axe.transform.right = direction;

    }
    public void Ability2()
    {
       
    }
    public void Ability3()
    {
        
        
    }
}
