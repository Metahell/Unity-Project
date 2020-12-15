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
    }

    public void Ability1()
    {
        Vector3 direction = transform.forward;
        GameObject arrow = Factory.GetInstance().GetArrow();
        arrow.transform.position = spawnPoint.position;
        arrow.transform.forward = direction;
    }
}
