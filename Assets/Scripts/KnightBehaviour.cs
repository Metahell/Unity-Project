using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class KnightBehaviour : MonoBehaviour
{
    [Header("Link")]
    private Rigidbody rigi;
    [SerializeField] private Animator animator;
    public float poisontimer=0;
    private float poisonTick=3;
    private Vector3 mouvementVector = Vector3.zero;
    private Vector3 motionVector = Vector3.zero;
    private Vector3 direction;
    public bool isdead=false;
    [Header("Movement Parameters")]
    [SerializeField]
    private float maxVelocity;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float decceleration;
    private Vector3 playerPosition;
    private Vector3 wolfPosition;
    public bool is_moving = true;
    public NavMeshAgent agent;
    private float _hitTime = 1;
    private float _hitTimer = 1;
    private bool _canHit = false;
    public AnimationClip knightSlash2;
    [SerializeField]
    public int health;
    private int hpMax;
    public bool is_pushed;
    [SerializeField]
    Material mat0;
    [SerializeField]
    Material mat1;
    [SerializeField]
    Material mat2;
    [SerializeField]
    Material mat3;
    [SerializeField]
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider.minValue = 0;
        hpMax = health;
        slider.maxValue = hpMax;
        is_pushed = false;
        rigi = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Confined;
        _hitTime += knightSlash2.length;
        _hitTimer = _hitTime;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = health;
        if (is_pushed)
        {
            is_moving = false;
        }
        if (health <= 0)
        {
            isdead = true;
            StartCoroutine(Death());
        }
        else
        {
        if (poisontimer>0)
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
            _hitTimer += Time.deltaTime;
            if (_hitTimer > _hitTime)
            {
                _canHit = true;
            }
            playerPosition = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
            if (GameObject.FindGameObjectsWithTag("Wolf").Length > 0)
            {
                wolfPosition = GameObject.FindGameObjectsWithTag("Wolf")[0].transform.position;
            }
            else
            {
                wolfPosition = Vector3.positiveInfinity;
            }
                if (Vector3.Distance(playerPosition, transform.position) < 2.5f || animator.GetCurrentAnimatorStateInfo(1).IsTag("1") || Vector3.Distance(wolfPosition, transform.position) < 2.5f)
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
                    agent.SetDestination(Vector3.Distance(playerPosition,transform.position) < Vector3.Distance(wolfPosition,transform.position) ? playerPosition : wolfPosition);
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
        animator.SetFloat("velocityForward", Vector3.Dot(rigi.velocity, transform.forward));
        animator.SetFloat("velocityRight", Vector3.Dot(rigi.velocity, transform.right));
        animator.SetFloat("velocity", rigi.velocity.magnitude);
    }

    private void DoRotation()
    {
        playerPosition.y = transform.position.y;
        direction = Vector3.Distance(playerPosition, transform.position) < Vector3.Distance(wolfPosition, transform.position) ? playerPosition -transform.position : wolfPosition - transform.position;
        direction.y = 0;
        direction = direction.normalized;

    }

    private void Hit()
    {
        playerPosition = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
        if (Vector3.Distance(playerPosition, transform.position) < 2.5f)
        {
            //Debug.Log("Touché");
            GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<HealthOrb>().Damage(5);
        }
        if (GameObject.FindGameObjectsWithTag("Wolf").Length > 0)
        {
            wolfPosition = GameObject.FindGameObjectsWithTag("Wolf")[0].transform.position;
            if (Vector3.Distance(wolfPosition,transform.position) < 2.5f)
            {
                Debug.Log("vie luop avant :" + GameObject.FindGameObjectsWithTag("Wolf")[0].GetComponent<Wolf>().health);
                GameObject.FindGameObjectsWithTag("Wolf")[0].GetComponent<Wolf>().LooseHealth(5);
                Debug.Log("loup touché  " + GameObject.FindGameObjectsWithTag("Wolf")[0].GetComponent<Wolf>().health);
            }
        }
    }

    public void LooseHealth(int healthLoss)
    {
        health -= healthLoss;
        health = health > hpMax + 5 ? hpMax + 5 : health;
        if (healthLoss > 0) StartCoroutine("Red"); else StartCoroutine(Green());
    }

    IEnumerator Red()
    {


        Transform cube = gameObject.transform.Find("Cube.002");
        Material[] materials = cube.GetComponent<Renderer>().materials;
        materials[0].color = Color.red;
        materials[1].color = Color.red;
        materials[2].color = Color.red;
        materials[3].color = Color.red;
        yield return new WaitForSeconds(0.1f);
        materials[0].color = mat0.color;
        materials[1].color = mat1.color;
        materials[2].color = mat2.color;
        materials[3].color = mat3.color;
    }

    IEnumerator Green()
    {


        Transform cube = gameObject.transform.Find("Cube.002");
        Material[] materials = cube.GetComponent<Renderer>().materials;
        materials[0].color = Color.green;
        materials[1].color = Color.green;
        materials[2].color = Color.green;
        materials[3].color = Color.green;
        yield return new WaitForSeconds(0.1f);
        materials[0].color = mat0.color;
        materials[1].color = mat1.color;
        materials[2].color = mat2.color;
        materials[3].color = mat3.color;
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
