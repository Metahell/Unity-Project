using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBehaviour : MonoBehaviour
{
    [Header("Link")]
    private Rigidbody rigi;
    [SerializeField] private Animator animator;
    public Transform targetDirection;
    [SerializeField]  private Vector3 mouvementVector = Vector3.zero;
    private Vector3 motionVector = Vector3.zero;
    private Vector3 direction;
    [Header("Movement Parameters")]
    [SerializeField]
    private float maxVelocity;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float decceleration;
    Vector3[] positionArray = new[] { Vector3.left, Vector3.right,Vector3.forward,Vector3.back };
    [SerializeField] private bool is_moving;
    private float lerpSmooth;
    // Start is called before the first frame update
    void Start()
    {
        rigi = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Confined;
        is_moving = false;   
    }

    // Update is called once per frame
    void Update()
    {
        DoRotation();
        StartCoroutine("pause");
    }

    IEnumerator pause()
    {
        if (!is_moving)
        {
            is_moving = true;

            mouvementVector = Vector3.zero;
            motionVector = new Vector3(mouvementVector.x * maxVelocity, rigi.velocity.y, mouvementVector.z * maxVelocity);
            lerpSmooth = rigi.velocity.magnitude < motionVector.magnitude ? acceleration : decceleration;
            rigi.velocity = Vector3.Lerp(rigi.velocity, motionVector, lerpSmooth / 20);
            if (direction.magnitude > 0)
                transform.forward = Vector3.Lerp(transform.forward, direction, .3f);
            UpdateAnimator();
            yield return new WaitForSeconds(0.1f);
        mouvementVector = positionArray[Random.Range(0, 4)];
        StartCoroutine("move");

        }
        
        yield return null;
    }

    IEnumerator move()
    {
        if (is_moving)
        {
            for (float i = 0; i < 4; i += Time.deltaTime)
            {
                motionVector = new Vector3(mouvementVector.x * maxVelocity, rigi.velocity.y, mouvementVector.z * maxVelocity);
                lerpSmooth = rigi.velocity.magnitude < motionVector.magnitude ? acceleration : decceleration;
                rigi.velocity = Vector3.Lerp(rigi.velocity, motionVector, lerpSmooth / 20);
                if (direction.magnitude > 0)
                    transform.forward = Vector3.Lerp(transform.forward, direction, .3f);
                UpdateAnimator();
                yield return null;
            }

            mouvementVector = Vector3.zero;
            motionVector = new Vector3(mouvementVector.x * maxVelocity, rigi.velocity.y, mouvementVector.z * maxVelocity);
            lerpSmooth = rigi.velocity.magnitude < motionVector.magnitude ? acceleration : decceleration;
            rigi.velocity = Vector3.Lerp(rigi.velocity, motionVector, lerpSmooth / 20);
            if (direction.magnitude > 0)
                transform.forward = Vector3.Lerp(transform.forward, direction, .3f);
            UpdateAnimator();
            is_moving = false;
            
        }
        mouvementVector = Vector3.zero;
        motionVector = new Vector3(mouvementVector.x * maxVelocity, rigi.velocity.y, mouvementVector.z * maxVelocity);
        lerpSmooth = rigi.velocity.magnitude < motionVector.magnitude ? acceleration : decceleration;
        rigi.velocity = Vector3.Lerp(rigi.velocity, motionVector, lerpSmooth / 20);
        if (direction.magnitude > 0)
            transform.forward = Vector3.Lerp(transform.forward, direction, .3f);
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
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
        Vector3 playerPosition = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
        playerPosition.y = transform.position.y;
        direction = playerPosition - transform.position;
        direction.y = 0;
        direction = direction.normalized;

    }
}
