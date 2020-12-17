using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    public int health;
    private int hpMax;
    public bool is_pushed;
    [SerializeField]
    private Renderer GLTFNode_61;
    [SerializeField]
    private Renderer GLTFNode_62;
    [SerializeField]
    private Renderer GLTFNode_63;
    [SerializeField]
    private Renderer GLTFNode_64;
    [SerializeField]
    private Renderer GLTFNode_65;
    [SerializeField]
    private Material GLTFNode_61_color;
    [SerializeField]
    private Material GLTFNode_62_color;
    [SerializeField]
    private Material GLTFNode_63_color;
    [SerializeField]
    private Material GLTFNode_64_color;
    [SerializeField]
    private Material GLTFNode_65_color;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private AudioSource spawn;
    [SerializeField]
    private AudioSource attack;
    [SerializeField]
    private AudioSource hurt;
    // Start is called before the first frame update
    void Start()
    {
        spawn.Play();
        slider.minValue = 0;
        hpMax = health;
        slider.maxValue = hpMax;
        is_pushed = false;
        rigi = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Confined;
        _hitTime += swipe.length;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = health;/*
        if (transform.position.y <= -1)
        {
            transform.position = new Vector3(0f, 17f, 0f);
        }*/
        FixQuaternion = Quaternion.Euler(FixRotation * Time.deltaTime);
        if (CurrentTarget == null)
        {
            FindEnemy();
        }
        else if (CheckHealth(CurrentTarget) <= 0)
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
        GameObject temp = new GameObject("temp");
        CurrentTarget = temp;
        CurrentTarget.transform.position = transform.position + new Vector3(1e32f,1e32f,1e32f);
        GameObject[] Targets = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var target in Targets)
        {
            if (target.activeInHierarchy && Vector3.Distance(target.transform.position,transform.position) <= Vector3.Distance(CurrentTarget.transform.position,transform.position))
            {
                CurrentTarget = target;
            }
        }
        Targets= GameObject.FindGameObjectsWithTag("Archer");
        foreach (var target in Targets)
        {
            if (target.activeInHierarchy && Vector3.Distance(target.transform.position, transform.position) <= Vector3.Distance(CurrentTarget.transform.position, transform.position))
            {
                CurrentTarget = target;
            }
        }
        Targets = GameObject.FindGameObjectsWithTag("Shaman");
        foreach (var target in Targets)
        {
            if (target.activeInHierarchy && Vector3.Distance(target.transform.position, transform.position) <= Vector3.Distance(CurrentTarget.transform.position, transform.position))
            {
                CurrentTarget = target;
            }
        }
        Targets = GameObject.FindGameObjectsWithTag("Boss");
        foreach (var target in Targets)
        {
            if (target.activeInHierarchy && Vector3.Distance(target.transform.position, transform.position) <= Vector3.Distance(CurrentTarget.transform.position, transform.position))
            {
                CurrentTarget = target;
            }
        }
        Destroy(temp);
    }
    private void Hit()
    {
        attack.Play();
        if (Vector3.Distance(CurrentTarget.transform.position , transform.position) < 3.5f)
        {
            if (CurrentTarget.CompareTag("Enemy"))
            {
                CurrentTarget.GetComponent<KnightBehaviour>().LooseHealth(5);
            }
            else if (CurrentTarget.CompareTag("Archer"))
            {
                CurrentTarget.GetComponent<ArcherBehaviour>().LooseHealth(5);
            }
            else if (CurrentTarget.CompareTag("Boss"))
            {
                CurrentTarget.GetComponent<GolemBehavior>().LooseHealth(5);
            }
            else if (CurrentTarget.CompareTag("Shaman"))
            {
                CurrentTarget.GetComponent<ShamanBehavior>().LooseHealth(5);
            }
        }
    }

    public void LooseHealth(int healthLoss)
    {
        health -= healthLoss;
        health = health > hpMax ? hpMax : health;
        if (healthLoss > 0)
        {
            StartCoroutine("Red");
            hurt.Play();

        }
        else StartCoroutine(Green());
    }

    IEnumerator Red()
    {
        GLTFNode_61.material.color = Color.red;
        GLTFNode_62.material.color = Color.red;
        GLTFNode_63.material.color = Color.red;
        GLTFNode_64.material.color = Color.red;
        GLTFNode_65.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GLTFNode_61.material.color = GLTFNode_61_color.color;
        GLTFNode_62.material.color = GLTFNode_62_color.color;
        GLTFNode_63.material.color = GLTFNode_63_color.color;
        GLTFNode_64.material.color = GLTFNode_64_color.color;
        GLTFNode_65.material.color = GLTFNode_65_color.color;

    }

    IEnumerator Green()
    {
        GLTFNode_61.material.color = Color.green;
        GLTFNode_62.material.color = Color.green;
        GLTFNode_63.material.color = Color.green;
        GLTFNode_64.material.color = Color.green;
        GLTFNode_65.material.color = Color.green;
        yield return new WaitForSeconds(0.1f);
        GLTFNode_61.material.color = GLTFNode_61_color.color;
        GLTFNode_62.material.color = GLTFNode_62_color.color;
        GLTFNode_63.material.color = GLTFNode_63_color.color;
        GLTFNode_64.material.color = GLTFNode_64_color.color;
        GLTFNode_65.material.color = GLTFNode_65_color.color;

    }

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

    private int CheckHealth(GameObject ennemy)
    {
        if (ennemy.CompareTag("Enemy"))
        {
            return ennemy.GetComponent<KnightBehaviour>().health;
        }
        else if (ennemy.CompareTag("Archer"))
        {
            return ennemy.GetComponent<ArcherBehaviour>().health;
        }
        else if (ennemy.CompareTag("Shaman"))
        {
            return ennemy.GetComponent<ShamanBehavior>().health;
        }
        else if (ennemy.CompareTag("Boss"))
        {
            return ennemy.GetComponent<GolemBehavior>().health;
        }
        else
        {
            return 0;
        }
    }
}

