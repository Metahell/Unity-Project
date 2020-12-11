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
    private Transform pos;
    [SerializeField]
    private Wall wall;
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
        GameObject fireball = Factory.GetInstance().GetFireball();
        fireball.transform.position = spawnPoint.position;
        fireball.transform.forward = direction;

    }
    public void Ability2()
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            Vector3 spawn = new Vector3(hit.transform.position.x, 2.3f, hit.transform.position.z);
            Instantiate(wall,spawn,transform.rotation);
        }
    }
    public void Ability3()
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity) && hit.transform.tag == "Enemy")
        {
            Transform temp = gameObject.transform;
            gameObject.transform.position = hit.transform.position;
            hit.transform.position = temp.position;
        }
        else
        {
            //display text (?)
        }
    }
}
