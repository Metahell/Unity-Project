using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemBehavior : MonoBehaviour
{
    [Header("Link")]
    private Rigidbody rigi;
    [SerializeField] private Animator animator;
    private Vector3 mouvementVector = Vector3.zero;
    private Vector3 motionVector = Vector3.zero;
    private Vector3 direction;
    private Vector3 chargedirection;
    private bool charging = false;
    [SerializeField]
    private int chargespeed;
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
    private float _chargeTime= 10;
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
        health = healthmax;
        is_pushed = false;
        rigi = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Confined;
        _hitTime += GolemSwipe.length;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthmax <= 0)
        {
            StartCoroutine(Death());
        }
        else
        {
            if(health >= healthmax/2&&!charging)
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
                    rigi.isKinematic = false;
                    agent.enabled = true;
                    is_moving = true;
                    agent.SetDestination(playerPosition);
                    mouvementVector = (transform.forward).normalized;
                }
                UpdateAnimator();
                DoRotation();
            }
        }
    }

    private void FixedUpdate()
    {
        if (charging)
        {
            transform.LookAt(chargedirection);
            rigi.AddRelativeForce(Vector3.forward * chargespeed, ForceMode.Force);
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
            direction = chargedirection.normalized;
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
        chargedirection = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
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
        Debug.Log("charging"+charging);
    }

    public void LooseHealth(int healthLoss)
    {
        health -= healthLoss;
        StartCoroutine("Red");
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        if (collision.collider.CompareTag("Player"))
        {
            player.GetComponent<HealthOrb>().Damage(20);
            charging = false;
        }
        if (collision.collider.CompareTag("Wall")||rigi.velocity==Vector3.zero)
        {
            Debug.Log("aie");
            charging = false;

            Debug.Log("charging" + charging);
        }
        Physics.IgnoreCollision(this.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
    }
    IEnumerator Red()
    {


        Transform cube = gameObject.transform.Find("Cube.001");
        Material[] materials = cube.GetComponent<Renderer>().materials;
        Color color0 = materials[0].color;
        Color color1 = materials[1].color;
        Color color2 = materials[2].color;
        Color color3 = materials[3].color;
        materials[0].color = Color.red;
        materials[1].color = Color.red;
        materials[2].color = Color.red;
        materials[3].color = Color.red;
        yield return new WaitForSeconds(0.1f);
        materials[0].color = color0;
        materials[1].color = color1;
        materials[2].color = color2;
        materials[3].color = color3;
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

}
