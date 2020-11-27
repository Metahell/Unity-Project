using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnightBehaviour : MonoBehaviour
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
    private Vector3 playerPosition;
    private bool is_moving = true;
    public NavMeshAgent agent;
    private float _hitTime = 1;
    private float _hitTimer = 0;
    private bool _canHit = false;
    public AnimationClip knightSlash2;
    private NavMeshModifier navMeshModifier;
    // Start is called before the first frame update
    void Start()
    {
        rigi = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Confined;
        _hitTime += knightSlash2.length;
    }

    // Update is called once per frame
    void Update()
    {

        _hitTimer += Time.deltaTime;
        if (_hitTimer > _hitTime)
        {
            _canHit = true;
        }
        playerPosition = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
        if (Vector3.Distance(playerPosition,transform.position) < 2.5f || animator.GetCurrentAnimatorStateInfo(1).IsTag("1"))
        {
            agent.enabled = false;
            rigi.velocity = Vector3.zero;
            rigi.isKinematic = true;
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
        direction = playerPosition - transform.position;
        direction.y = 0;
        direction = direction.normalized;

    }

    private void Hit()
    {
        playerPosition = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
        if (Vector3.Distance(playerPosition, transform.position) < 2.5f)
        {
            Debug.Log("Touché");
        }
    }

}
