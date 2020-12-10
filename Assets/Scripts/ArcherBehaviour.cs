using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArcherBehaviour : MonoBehaviour
{
    [SerializeField]
    private Arrow arrow; 
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
    private bool is_moving = true;
    public NavMeshAgent agent;

    [SerializeField]
    private float _attackSpeed = 0.5f;
    private float _fireTimer = 0;
    private bool _canShoot = false;
    public AnimationClip archershot;

    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        rigi = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer > (1/_attackSpeed) )
            _canShoot = true;

        playerPosition = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
        float distanceToPlayer = Vector3.Distance(playerPosition, transform.position);

        if ( ( (distanceToPlayer < 15f && distanceToPlayer > 6f)|| animator.GetCurrentAnimatorStateInfo(1).IsTag("1") ) && canShoot() )
        {
            agent.enabled = false;
            rigi.velocity = Vector3.zero;
            rigi.isKinematic = true;

            if (!animator.GetCurrentAnimatorStateInfo(1).IsTag("1") && _canShoot)
            {
                animator.SetTrigger("Shoot");
                Shoot();
                _canShoot = false;
                _fireTimer = 0;
            }
            is_moving = false;
        }
        else
        {
            rigi.isKinematic = false;
            agent.enabled = true;
            is_moving = true;
            if (distanceToPlayer > 15f)
                agent.SetDestination(playerPosition);
            if (distanceToPlayer < 6f)
                agent.SetDestination(transform.position +(transform.position - playerPosition).normalized);
            mouvementVector = (transform.forward).normalized;
        }
        UpdateAnimator();
        DoRotation();
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
        animator.SetFloat("velocityForward", 15* Vector3.Dot(rigi.velocity, transform.forward));
        animator.SetFloat("velocityRight", 15* Vector3.Dot(rigi.velocity, transform.right));
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
        Transform cube = gameObject.transform.Find("Cube.003");
        Material[] materials = cube.GetComponent<Renderer>().materials;
        Color color0 = materials[0].color;
        Color color1 = materials[1].color;
        Color color2 = materials[2].color;
        materials[0].color = Color.red;
        materials[1].color = Color.red;
        materials[2].color = Color.red;
        yield return new WaitForSeconds(0.1f);
        materials[0].color = color0;
        materials[1].color = color1;
        materials[2].color = color2;

        if (health <= 0)              //if no hp then dies => despawn
            Destroy(gameObject);
    }

    private void Shoot()
    {
        Vector3 direction = transform.forward;
        GameObject arrow = Factory.GetInstance().GetArrow();
        arrow.transform.position = spawnPoint.position;
        arrow.transform.forward = direction;
    }
    private bool canShoot()
    {
        //test via RayCast in direction of player if obstacle then return true else false
        return true;
    }
}
