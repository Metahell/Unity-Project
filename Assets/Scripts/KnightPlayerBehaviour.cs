using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightPlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private KnightBehaviour KnightBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(1).IsTag("1"))
        {
            animator.SetTrigger("1st Ability");
        }
    }

    public void Ability1()
    {
        Vector3 attackCenter = transform.position + transform.forward;
        Collider[] colliders = Physics.OverlapSphere(attackCenter, 1.5f);
        /**GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = attackCenter;
        sphere.transform.localScale = new Vector3(1.5f,1.5f,1.5f);**/
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                collider.gameObject.GetComponent<KnightBehaviour>().LooseHealth(5);
                collider.gameObject.GetComponent<KnightBehaviour>().is_moving = true;
                collider.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                collider.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * 10;
                collider.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward*10);
                collider.gameObject.transform.position += transform.forward;
            }
        }
    }
}
