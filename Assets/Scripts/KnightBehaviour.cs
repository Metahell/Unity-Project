using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBehaviour : MonoBehaviour
{
    [Header("Link")]
    private Rigidbody rigi;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 mouvementVector = Vector3.zero;
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
    Vector3[] positionArray = new[] { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
    // Start is called before the first frame update
    void Start()
    {
        rigi = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
        if (Vector3.Distance(playerPosition,transform.position) < 2.5f)
        {
            if (!animator.GetCurrentAnimatorStateInfo(1).IsTag("1"))
            { 

                animator.SetTrigger("Attack");
            }
            is_moving = false;
        }
        else
        {
            is_moving = true;
            mouvementVector = (transform.forward).normalized;
            UpdateAnimator();
        }
        DoRotation();
    }

    private void FixedUpdate()
    {
            if (!animator.GetCurrentAnimatorStateInfo(1).IsTag("1"))
            {
            motionVector = new Vector3(mouvementVector.x * maxVelocity, rigi.velocity.y, mouvementVector.z * maxVelocity);
            float lerpSmooth = rigi.velocity.magnitude < motionVector.magnitude ? acceleration : decceleration;
            rigi.velocity = Vector3.Lerp(rigi.velocity, motionVector, lerpSmooth / 20);

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
        playerPosition.y = transform.position.y;
        direction = playerPosition - transform.position;
        direction.y = 0;
        direction = direction.normalized;

    }
}
