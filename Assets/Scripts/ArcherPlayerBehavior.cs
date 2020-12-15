using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherPlayerBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private Animator animator;
    private KnightBehaviour KnightBehaviour;
    [SerializeField]
    private float shootAngle;
    private float _ability1Time = 1;
    private float _ability1Timer = 1;
    private float _ability2Time = 3;
    private float _ability2Timer = 3;
    private float _ability3Time = 10;
    private float _ability3Timer = 10;
    private float Boost=0;
    private bool boosted=false;
    void Update()
    {

        _ability1Timer += Time.deltaTime;
        _ability2Timer += Time.deltaTime;
        _ability3Timer += Time.deltaTime;
        if (Boost >= 0)
        {
            Boost -= Time.deltaTime;
        }
        if (Boost <= 0&&boosted)
        {
            _ability1Time *= 2;
            this.GetComponent<PlayerController>().maxVelocity /= 2;
            boosted = false;
        }
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
            _ability3Timer = 0;
        }
    }

    public void Ability1()
    {
        Vector3 direction = transform.forward;
        GameObject arrow = Factory.GetInstance().GetArrow();
        arrow.transform.position = spawnPoint.position;
        arrow.transform.forward = direction;
    }
    public void Ability2()
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            Vector3 spawn = new Vector3(hit.transform.position.x, 0.017f, hit.transform.position.z);
            GameObject trap = Factory.GetInstance().GetTrap();
            trap.transform.position = spawn;
        }
    }
    public void Ability3()
    {
        _ability1Time /= 2;
        Boost = 3;
        this.GetComponent<PlayerController>().maxVelocity *= 2;
        boosted=true;
    }
}
