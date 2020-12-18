using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class ArcherBehaviour : MonoBehaviour
{
    [SerializeField]
    private Arrow arrow;
    [Header("Link")]
    private Rigidbody rigi;
    public bool isdead = false;
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
    private float _attackSpeed = 0.5f;
    private float _fireTimer = 0;
    private bool _canShoot = false;
    private bool _visionClear = true;
    private int hpMax;
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    public int health;

    public bool is_pushed;
    [SerializeField]
    Material mat0;
    [SerializeField]
    Material mat1;
    [SerializeField]
    Material mat2;
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
            isdead = true;
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
            _fireTimer += Time.deltaTime;
            if (_fireTimer > (1 / _attackSpeed))
                _canShoot = true;

            playerPosition = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
            float distanceToPlayer = Vector3.Distance(playerPosition, transform.position);

            if (((distanceToPlayer < 20f && distanceToPlayer > 6f) || animator.GetCurrentAnimatorStateInfo(1).IsTag("1")) && _visionClear)
            {
                agent.enabled = false;
                rigi.velocity = Vector3.zero;
                if (!is_pushed)
                {
                    rigi.isKinematic = true;
                }

                if (!animator.GetCurrentAnimatorStateInfo(1).IsTag("1") && _canShoot)
                {
                    animator.SetTrigger("1st Ability");
                    Shoot();
                    _canShoot = false;
                    _fireTimer = 0;
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
                    if (!_visionClear)
                    {
                        agent.SetDestination(playerPosition);
                    }
                    else
                    {
                        if (distanceToPlayer > 20f)
                            agent.SetDestination(playerPosition);
                        if (distanceToPlayer < 6f)
                        {
                            NavMeshHit hit;
                            Vector3 fuite = (transform.position - playerPosition).normalized;
                            if (NavMesh.SamplePosition(playerPosition + fuite * 7, out hit, 6.0f, NavMesh.AllAreas)) // fuite intelligente
                            {
                                if (Vector3.Distance(hit.position, playerPosition) > 6.0f)
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

        _visionClear = canShoot();
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
        Transform cube = gameObject.transform.Find("Cube.003");
        Material[] materials = cube.GetComponent<Renderer>().materials;
        Debug.Log(materials[0].ToString() + materials[1].ToString() + materials[2].ToString());
        materials[0].color = Color.red;
        materials[1].color = Color.red;
        materials[2].color = Color.red;
        yield return new WaitForSeconds(0.1f);
        materials[0].color = mat0.color;
        materials[1].color = mat1.color;
        materials[2].color = mat2.color;

    }

    IEnumerator Green()
    {
        Transform cube = gameObject.transform.Find("Cube.003");
        Material[] materials = cube.GetComponent<Renderer>().materials;
        materials[0].color = Color.green;
        materials[1].color = Color.green;
        materials[2].color = Color.green;
        yield return new WaitForSeconds(0.1f);
        materials[0].color = mat0.color;
        materials[1].color = mat1.color;
        materials[2].color = mat2.color;

    }

    private void Shoot()
    {
        Vector3 direction = transform.forward;
        GameObject arrow = Factory.GetInstance().GetArrow();
        arrow.transform.position = spawnPoint.position;
        arrow.transform.forward = direction;
        arrow.GetComponent<Arrow>().IsEnemyArrow();
    }
    private bool canShoot()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + new Vector3(0, 2, 0);
        Vector3 dest = playerPosition + new Vector3(0, 2, 0);
        return !Physics.Raycast(origin, dest - origin, out hit, 40f, LayerMask.GetMask("Obstacle"));
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
