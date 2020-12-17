using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShamanBehavior : MonoBehaviour
{
    [Header("Link")]
    private Rigidbody rigi;

    [SerializeField]
    private Animator animator;

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
    private Vector3 playerPosition;
    float distanceToPlayer;
    private bool is_moving = true;
    public NavMeshAgent agent;
    public float poisontimer = 0;
    private int poisonTick = 3;
    [SerializeField]
    private float _healtime = 3f;
    private float _HealTimer = 0;
    private bool _canHeal = false;
    

    [SerializeField]
    private int health;

    public bool is_pushed;
    // Start is called before the first frame update
    void Start()
    {
        is_pushed = false;
        rigi = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            StartCoroutine(Death());
        }
        else
        {
            if (poisontimer > 0)
            {
                poisontimer -= Time.deltaTime;
                if (poisonTick - poisontimer >= 0)
                {
                    LooseHealth(2);
                    poisonTick -= 1;
                }
            }
            if (poisontimer <= 0 && poisonTick != 3)
            {
                poisonTick = 3;
            }
            else
            {
                _HealTimer += Time.deltaTime;
                if (_HealTimer > 3)
                    _canHeal = true;

                playerPosition = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
                float distanceToPlayer = Vector3.Distance(playerPosition, transform.position);

                if (((distanceToPlayer >= 8f) || animator.GetCurrentAnimatorStateInfo(1).IsTag("1")))
                {
                    agent.enabled = false;
                    rigi.velocity = Vector3.zero;
                    if (!is_pushed)
                    {
                        rigi.isKinematic = true;
                    }

                    if (!animator.GetCurrentAnimatorStateInfo(1).IsTag("1") && _canHeal)
                    {
                        animator.SetTrigger("1st Ability");
                        Heal();
                        _canHeal = false;
                        _HealTimer = 0;
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
                        
                        if (distanceToPlayer < 8f)
                            agent.SetDestination(transform.position + (transform.position - playerPosition).normalized);
                        
                        mouvementVector = (transform.forward).normalized;
                    }
                }
                if (!ThiefPlayerBehavior.isInvisible)
                {
                    UpdateAnimator();
                    DoRotation();
                }
            }
        }

        //Debug.Log(_visionClear.ToString() +"  "+ distanceToPlayer.ToString());
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
            transform.forward = Vector3.Lerp(transform.forward, direction, .3f);
        
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("velocityForward", 15 * Vector3.Dot(rigi.velocity, transform.forward));
        animator.SetFloat("velocityRight", 15 * Vector3.Dot(rigi.velocity, transform.right));
        animator.SetFloat("velocity", rigi.velocity.magnitude);
    }

    private void DoRotation()
    {
        playerPosition.y = transform.position.y;
        direction = playerPosition - transform.position;
        direction.y = 0;
        direction = direction.normalized;

    }

    public void LooseHealth(int healthLoss)
    {
        health -= healthLoss;
        StartCoroutine("Red");
    }

    IEnumerator Red()
    {
        Transform cube = gameObject.transform.Find("Nightshade");
        Material[] materials = cube.GetComponent<Renderer>().materials;
        Color color0 = materials[0].color;
        materials[0].color = Color.red;
        yield return new WaitForSeconds(0.1f);
        materials[0].color = color0;

    }

    private void Heal()
    {
        GameObject[] Targets = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject target in Targets)
        {
            if (target.CompareTag("Enemy"))
            {
                KnightBehaviour knight = target.GetComponent<KnightBehaviour>();
                if (knight.health<=10)
                target.GetComponent<KnightBehaviour>().LooseHealth(-5);
            }
            else if (target.CompareTag("Archer"))
            {
                ArcherBehaviour archer = target.GetComponent<ArcherBehaviour>();
                if(archer.health<=5)
                target.GetComponent<ArcherBehaviour>().LooseHealth(-5);
            }
            else if (target.CompareTag("Boss"))
            {
               GolemBehavior golem = target.GetComponent<GolemBehavior>();
                if(golem.health<=golem.healthmax)
                target.GetComponent<GolemBehavior>().LooseHealth(-5);
            }
        }

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
