using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour
{
    [Header("Link")]
    private Rigidbody rigi;
    [SerializeField] private Animator animator;
    private Vector3 mouvementVector = Vector3.zero;
    private Vector3 motionVector = Vector3.zero;
    private Vector3 direction;
    [Header("Movement Parameters")]
    [SerializeField]
    private float maxVelocity;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float decceleration;
    private Vector3 EnemyPosition;
    public bool is_moving = true;
    public NavMeshAgent agent;
    private float _hitTime = 1;
    private float _hitTimer = 0;
    private bool _canHit = false;
    private Quaternion FixQuaternion;
    private Vector3 FixRotation = new Vector3(0,-180,0);
    public AnimationClip swipe;
    private GameObject CurrentTarget;
    [SerializeField]
    private int health;
    public bool is_pushed;
    // Start is called before the first frame update
    void Start()
    {
        is_pushed = false;
        rigi = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Confined;
        _hitTime += swipe.length;
    }

    // Update is called once per frame
    void Update()
    {
        FixQuaternion = Quaternion.Euler(FixRotation * Time.deltaTime);
        if (CurrentTarget == null)
        {
            FindEnemy();
        }
        if (health <= 0)
        {
            StartCoroutine(Death());
        }
        else
        {
            _hitTimer += Time.deltaTime;
            if (_hitTimer > _hitTime)
            {
                _canHit = true;
            }
            if (CurrentTarget==null)
            {
                FindEnemy();
            }
            EnemyPosition = CurrentTarget.transform.position;
            if (Vector3.Distance(EnemyPosition, transform.position) < 2.5f || animator.GetCurrentAnimatorStateInfo(1).IsTag("1"))
            {
                agent.enabled = false;
                rigi.velocity = Vector3.zero;
                if (!is_pushed)
                {
                    rigi.isKinematic = true;
                }
                if (!animator.GetCurrentAnimatorStateInfo(1).IsTag("1") && _canHit)
                {
                    animator.SetTrigger("Attack");
                    _canHit = false;
                    _hitTimer = 0;
                }
                is_moving = false;
            }
            else
            {
                rigi.isKinematic = false;
                agent.enabled = true;
                is_moving = true;
                agent.SetDestination(EnemyPosition);
                rigi.MoveRotation(rigi.rotation * FixQuaternion);
                mouvementVector = (-transform.forward).normalized;
            }
            UpdateAnimator();
            DoRotation();
        }
    }

    private void FixedUpdate()
    {
        if (!animator.GetCurrentAnimatorStateInfo(1).IsTag("1") && is_moving)
        {
            motionVector = new Vector3(mouvementVector.x * maxVelocity, rigi.velocity.y, mouvementVector.z * maxVelocity);
            float lerpSmooth = rigi.velocity.magnitude < motionVector.magnitude ? acceleration : decceleration;
            rigi.velocity = Vector3.Lerp(rigi.velocity, motionVector, lerpSmooth / 20);

        }
        if (direction.magnitude > 0)
        {
            transform.forward = Vector3.Lerp(transform.forward, direction, .3f);
            FixQuaternion = Quaternion.Euler(FixRotation * Time.deltaTime);
            rigi.MoveRotation(rigi.rotation * FixQuaternion);
        }
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("velocityForward", Vector3.Dot(rigi.velocity, transform.forward));
        animator.SetFloat("velocityRight", Vector3.Dot(rigi.velocity, transform.right));
        animator.SetFloat("velocity", rigi.velocity.magnitude);
    }

    private void DoRotation()
    {
        EnemyPosition.y = transform.position.y;
        direction = EnemyPosition - transform.position;
        direction.y = 0;
        direction = -direction.normalized;
    }
    private void FindEnemy()
    {
        GameObject[] Targets = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < Targets.Length; i++)
        {
            if (Targets[i].activeInHierarchy)
            {
                CurrentTarget = Targets[i];
                return;
            }
        }
        Targets= GameObject.FindGameObjectsWithTag("Archer");
        for (int i = 0; i < Targets.Length; i++)
        {
            if (Targets[i].activeInHierarchy)
            {
                CurrentTarget = Targets[i];
                return;
            }
        }
    }
    private void Hit()
    {
        EnemyPosition = GameObject.FindGameObjectsWithTag("Enemy")[0].transform.position;
        if (Vector3.Distance(EnemyPosition, transform.position) < 2.5f)
        {
            //Debug.Log("Touché");
            GameObject.FindGameObjectsWithTag("Enemy")[0].GetComponent<HealthOrb>().Damage(5);
        }
    }

    public void LooseHealth(int healthLoss)
    {
        health -= healthLoss;
        StartCoroutine("Red");
    }

    /*IEnumerator Red()
    {
        
        WIP

        Transform cube = gameObject.transform.Find();
        Material[] materials = cube.GetComponent<Renderer>().materials;
       
        yield return new WaitForSeconds(0.1f);
        
}*/

IEnumerator Death()
    {
        animator.SetBool("IsDead", true);
        agent.enabled = false;
        rigi.isKinematic = true;
        gameObject.GetComponent<Collider>().enabled = false;
        UpdateAnimator();
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

}
