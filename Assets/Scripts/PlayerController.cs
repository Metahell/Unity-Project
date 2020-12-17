using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Link")]
    private Rigidbody rigi;
    [SerializeField] private Animator animator;
    [Header("Movement Parameters")]
    [SerializeField]
    public float maxVelocity;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float decceleration;

    public Transform targetDirection;
    private Vector3 mouvementVector = Vector3.zero;
    private Vector3 motionVector = Vector3.zero;
    private Vector3 direction;
    private HealthOrb HealthOrb;
    private bool isDead;
    private bool is_jumping;

    void Start()
    {
        is_jumping = false;
        isDead = false;
        rigi = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        if (gameObject.GetComponent<KnightPlayerBehaviour>() != null)
        {
            is_jumping = gameObject.GetComponent<KnightPlayerBehaviour>().is_jumping;
        }
        if (!is_jumping)
        {
            mouvementVector = (Vector3.forward * Input.GetAxisRaw("Vertical") + Vector3.right * Input.GetAxisRaw("Horizontal")).normalized;
            DoRotation();
            UpdateAnimator();
        }
    }


    private void FixedUpdate()
    {
        //Calculate velocity according to movementVector
        if (!isDead && !is_jumping)
        {
            motionVector = new Vector3(mouvementVector.x * maxVelocity, rigi.velocity.y, mouvementVector.z * maxVelocity);
            float lerpSmooth = rigi.velocity.magnitude < motionVector.magnitude ? acceleration : decceleration;
            rigi.velocity = Vector3.Lerp(rigi.velocity, motionVector, lerpSmooth / 20);
            if (direction.magnitude > 0)
                transform.forward = Vector3.Lerp(transform.forward, direction, .3f);
        }
    }

    /// <summary>
    /// Calculate player forward according to mouse position. Store it in "direction"
    /// </summary>
    private void DoRotation()
    {
        var camera = Camera.main;

        var mousePosition = Input.mousePosition;
        mousePosition.z = 1;
        Vector3 targetPosition = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(camera.ScreenPointToRay(mousePosition), out hit, 1000, LayerMask.GetMask("Floor")))
        {
            targetPosition = hit.point;
        }
        targetPosition.y = transform.position.y;
        direction = targetPosition - transform.position;
        direction.y = 0;
        direction = direction.normalized;
        if (targetDirection)
        {
            targetPosition.y = targetDirection.position.y;
            targetDirection.position = targetPosition;
        }
        targetDirection.rotation = Quaternion.identity;
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("velocityForward", Vector3.Dot(rigi.velocity, transform.forward));
        animator.SetFloat("velocityRight", Vector3.Dot(rigi.velocity, transform.right));
        animator.SetFloat("velocity", rigi.velocity.magnitude);
        animator.SetBool("isDead", isDead);
    }

    public void Death()
    {
        isDead = true;
        UpdateAnimator();
    }
}
