using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class ShamanBehavior : MonoBehaviour
{
    [Header("Link")]
    private Rigidbody rigi;

    [SerializeField]
    private Animator animator;

    private Vector3 mouvementVector = Vector3.zero;
    private Vector3 motionVector = Vector3.zero;
    private Vector3 direction;
    public bool trapped = false;
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
    public int health;

    public bool is_pushed;
    private int hpMax;
    [SerializeField]
    Material mat0;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Image back_slider;
    [SerializeField]
    private Sprite normal_back;
    [SerializeField]
    private Sprite buff_back;
    // Start is called before the first frame update
    void Start()
    {
        slider.minValue = 0;
        hpMax = health;
        slider.maxValue = hpMax;
        is_pushed = false;
        rigi = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = health;
        if (health > slider.maxValue)
        {
            back_slider.sprite = buff_back;
        }
        else
        {
            back_slider.sprite = normal_back;
        }
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
                    if (!ThiefPlayerBehavior.isInvisible&&!trapped)
                    {
                        rigi.isKinematic = false;
                        agent.enabled = true;
                        is_moving = true;

                        if (distanceToPlayer < 6f)
                        {
                            NavMeshHit hit;
                            Vector3 fuite = (transform.position - playerPosition).normalized;
                            if (NavMesh.SamplePosition(playerPosition + fuite * 7, out hit, 6.0f, NavMesh.AllAreas)) // fuite intelligente
                            {
                                if (Vector3.Distance(hit.position, playerPosition) >= Vector3.Distance(transform.position, playerPosition))
                                {
                                    agent.SetDestination(hit.position);
                                }
                                else
                                {
                                    agent.SetDestination(transform.position + fuite);
                                }
                            }
                            else
                            {
                                agent.SetDestination(transform.position + fuite);
                            }
                        }

                        mouvementVector = (transform.forward).normalized;
                    }
                }
                if (!ThiefPlayerBehavior.isInvisible&&!trapped)
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
        health = health > hpMax + 5 ? hpMax + 5 : health;
        if (healthLoss > 0) StartCoroutine("Red"); else StartCoroutine(Green());
    }

    IEnumerator Red()
    {
        Transform cube = gameObject.transform.Find("Nightshade");
        Material[] materials = cube.GetComponent<Renderer>().materials;
        Color color0 = materials[0].color;
        materials[0].color = Color.red;
        yield return new WaitForSeconds(0.1f);
        materials[0].color = mat0.color;

    }

    IEnumerator Green()
    {
        Transform cube = gameObject.transform.Find("Nightshade");
        Material[] materials = cube.GetComponent<Renderer>().materials;
        materials[0].color = Color.green;
        yield return new WaitForSeconds(0.1f);
        materials[0].color = mat0.color;

    }

    private void Heal()
    {
        GameObject[] Targetsk = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject target in Targetsk)
        {
            KnightBehaviour knight = target.GetComponent<KnightBehaviour>();
            if (knight.health <= 10 && !knight.isdead)
            {
                target.GetComponent<KnightBehaviour>().LooseHealth(-5);
                return;
            }
        }
        GameObject[] Targetsa = GameObject.FindGameObjectsWithTag("Archer");
        foreach (GameObject target in Targetsa)
        {
            ArcherBehaviour archer = target.GetComponent<ArcherBehaviour>();
            if (archer.health <= 5 && !archer.isdead)
            {
                target.GetComponent<ArcherBehaviour>().LooseHealth(-5);
                return;
            }
        }
        GameObject[] Target = GameObject.FindGameObjectsWithTag("Boss");
        foreach (GameObject target in Target)
        {
            GolemBehavior golem = target.GetComponent<GolemBehavior>();
            if (golem.health <= golem.healthmax && !golem.isdead)
            {
                target.GetComponent<GolemBehavior>().LooseHealth(-5);
                return;
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
