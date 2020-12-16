using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GolemBehavior : MonoBehaviour
{
    [Header("Link")]
    private Rigidbody rigi;
    [SerializeField] private Animator animator;
    private int state = 1;
    [SerializeField]
    private GameObject BossText;
    private Vector3 mouvementVector = Vector3.zero;
    private Vector3 motionVector = Vector3.zero;
    private Vector3 direction;
    private Vector3 chargedirection;
    private bool charging = false;
    private bool chargehit = false;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private float chargespeed;
    [Header("Movement Parameters")]
    [SerializeField]
    private float maxVelocity;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float decceleration;
    private Vector3 playerPosition;
    public bool is_moving = true;
    public NavMeshAgent agent;
    private float _hitTime = 1;
    private float _hitTimer = 0;
    private bool _canHit = false;
    private float _chargeTime= 20;
    private bool _canCharge = false;
    private float _chargeTimer = 10;
    public AnimationClip GolemSwipe;
    [SerializeField]
    private int healthmax;
    private int health;
    public bool is_pushed;
    // Start is called before the first frame update
    void Start()
    {
        BossText.SetActive(true);
        health = healthmax;
        is_pushed = false;
        rigi = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Confined;
        _hitTime += GolemSwipe.length;
    }
    private void Awake()
    {

        BossText.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            charging = false;
            StartCoroutine(Death());
        }
        else
        {
            if(health >=0&&!charging)
            {
                Physics.IgnoreCollision(this.GetComponent<Collider>(), GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Collider>(), false);
                _hitTimer += Time.deltaTime;
                _chargeTimer += Time.deltaTime;
                if (_hitTimer > _hitTime)
                {
                    _canHit = true;
                }
                if (_chargeTimer > _chargeTime)
                {
                    _canCharge = true;
                }
                playerPosition = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
                if ((Vector3.Distance(playerPosition, transform.position) < 10f)&&_canCharge||animator.GetCurrentAnimatorStateInfo(1).IsTag("1")&&_canCharge)
                {
                    agent.enabled = false;
                    rigi.velocity = Vector3.zero;
                    if (!is_pushed)
                    {
                        rigi.isKinematic = true;
                    }
                    if (!animator.GetCurrentAnimatorStateInfo(1).IsTag("1") && _canCharge)
                    {
                        Debug.Log("Start");
                        animator.SetTrigger("Attack2");
                        _canCharge = false;
                        _chargeTimer = 0;
                    }
                    is_moving = false;
                }
                if (Vector3.Distance(playerPosition, transform.position) < 5f || animator.GetCurrentAnimatorStateInfo(1).IsTag("1"))
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
                    if (!ThiefPlayerBehavior.isInvisible)
                    {
                        rigi.isKinematic = false;
                        agent.enabled = true;
                        is_moving = true;
                        agent.SetDestination(playerPosition);
                        mouvementVector = (transform.forward).normalized;
                    }
                }
            }
            if (!ThiefPlayerBehavior.isInvisible)
            {
                rigi.isKinematic = false;
                UpdateAnimator();
                DoRotation();
            }
        }
    }
    private void CheckLife()
    {
        if (health <= healthmax /2&&state==1)
        {
            chargespeed *= 1.5f;
            _chargeTime /= 2;
            state += 1;
        }
        else if(health <= healthmax / 4 && state == 2)
        {
            chargespeed *= 1.5f;
            _chargeTime /= 2;
            state += 1;
        }
    }
    private void FixedUpdate()
    {
        if (charging)
        {
            rigi.MovePosition(rigi.position + transform.forward *chargespeed/40);
        }
        else
        {
            if (!animator.GetCurrentAnimatorStateInfo(1).IsTag("1") && is_moving)
            {
                motionVector = new Vector3(mouvementVector.x * maxVelocity, rigi.velocity.y, mouvementVector.z * maxVelocity);
                float lerpSmooth = rigi.velocity.magnitude < motionVector.magnitude ? acceleration : decceleration;
                rigi.velocity = Vector3.Lerp(rigi.velocity, motionVector, lerpSmooth / 20);
            }

            if (direction.magnitude > 0)
                transform.forward = Vector3.Lerp(transform.forward, direction, .3f);
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
        if (charging)
        {
            chargedirection.y = transform.position.y;
            direction = chargedirection - transform.position;
            direction.y = 0;
            direction = direction.normalized;
        }
        else
        {
            playerPosition.y = transform.position.y;
            direction = playerPosition - transform.position;
            direction.y = 0;
            direction = direction.normalized;
        }
    }

    private void Hit()
    {
        chargedirection = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.normalized;
        if (Vector3.Distance(playerPosition, transform.position) < 5f)
        {
            //Debug.Log("Touché");
            GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<HealthOrb>().Damage(10);
        }
    }
    private void Charge()
    {
        chargedirection = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
        charging = true;
        chargehit = false;
        Physics.IgnoreCollision(this.GetComponent<Collider>(), GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Collider>(), false);
        transform.LookAt(chargedirection);
    }

    public void LooseHealth(int healthLoss)
    {
        health -= healthLoss;
        UpdateHealthbar();
        StartCoroutine("Red");
    }
    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.collider.CompareTag("Archer"))
        {
            Physics.IgnoreCollision(collision.collider, this.GetComponent<Collider>(),true);
        }
        if (collision.collider.CompareTag("Enemy"))
        {
            Physics.IgnoreCollision(collision.collider, this.GetComponent<Collider>(), true);
        }
        if (collision.collider.CompareTag("Player"))
        {
            GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
            if (!chargehit)
            {
                player.GetComponent<HealthOrb>().Damage(10);
                chargehit = true;
            }
            Physics.IgnoreCollision(this.GetComponent<Collider>(), player.GetComponent<Collider>());
        }
        if (collision.collider.CompareTag("Wall"))
        {
            charging = false;
        }
    }
    IEnumerator Red()
    {


        Transform cube = gameObject.transform.Find("Cube.001");
        Material[] materials = cube.GetComponent<Renderer>().materials;
        Color color0 = materials[0].color;
        materials[0].color = Color.red;
        yield return new WaitForSeconds(0.1f);
        materials[0].color = color0;
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

    private void UpdateHealthbar()
    {
        slider.value = (float)health / healthmax;
    }

}
