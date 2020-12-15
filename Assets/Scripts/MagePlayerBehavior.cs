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
    private float _ability1Time = 2;
    private float _ability1Timer = 2;
    private float _ability2Time = 10;
    private float _ability2Timer = 10;
    private float _ability3Time = 5;
    private float _ability3Timer = 5;
    void Update()
    {
        _ability1Timer += Time.deltaTime;
        _ability2Timer += Time.deltaTime;
        _ability3Timer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(1).IsTag("1") && _ability1Timer >= _ability1Time)
        {
            animator.SetTrigger("1st Ability");
            _ability1Timer = 0;
        }
        if (Input.GetMouseButtonDown(1) && !animator.GetCurrentAnimatorStateInfo(1).IsTag("1") && _ability2Timer >= _ability2Time)
        {
            animator.SetTrigger("2nd Ability");
            _ability2Timer = 0;
        }
        if (Input.GetKey(KeyCode.Space) && !animator.GetCurrentAnimatorStateInfo(1).IsTag("1") && _ability3Timer >= _ability3Time)
        {
            animator.SetTrigger("3rd Ability");
            // le reset du timer est dans la fonction d'abilité au cas où la position de la souris n'est pas bonnes
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
            Vector3 spawn = new Vector3(hit.transform.position.x, 0.017f, hit.transform.position.z);
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
            Debug.Log("" + hit);
            Vector3 temp = hit.transform.position;
            hit.collider.gameObject.transform.position = transform.position;
            gameObject.transform.position = temp;
            _ability3Timer = 0;
        }
        else
        {
            //display text (?)
        }
    }
}
