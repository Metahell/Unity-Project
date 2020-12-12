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
    [SerializeField]
    private GameObject wolf;
    private float radius;
    private void Start()
    {
        radius =wolf.GetComponent<CapsuleCollider>().radius;
    }
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
        Vector3 DruidPos = transform.position;
        Vector3 Spawn = new Vector3(Random.Range(DruidPos.x - 4,DruidPos.x + 4), 0.017f, Random.Range(DruidPos.z - 4, DruidPos.z + 4));
        while (!CheckSpawn(Spawn,radius))
        {
            Spawn = new Vector3(Random.Range(DruidPos.x - 4, DruidPos.x + 4), 0.017f, Random.Range(DruidPos.z - 4, DruidPos.z + 4));
        }
        Instantiate(wolf, Spawn,transform.rotation);
    }
    public void Ability3()
    {
        
        
    }

    private bool CheckSpawn(Vector3 center,float radius)
    {
        return Physics.CheckSphere(center,radius);
    }
}
