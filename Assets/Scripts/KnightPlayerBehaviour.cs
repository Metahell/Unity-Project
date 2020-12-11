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
        if (Input.GetMouseButtonDown(1) && !animator.GetCurrentAnimatorStateInfo(1).IsTag("1"))
        {
            animator.SetTrigger("2nd Ability");
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
                StartCoroutine(PushKnight(collider.gameObject));
            }
            else if (collider.gameObject.CompareTag("Archer"))
            {
                collider.gameObject.GetComponent<ArcherBehaviour>().LooseHealth(5);
                StartCoroutine(PushArcher(collider.gameObject));
            }
        }
    }

    public void Ability2()
    {
        StartCoroutine(Jump());
    }

    IEnumerator Jump()
    {
        Vector3 position = transform.position;
        Vector3 goal = transform.position + 2 * transform.forward;
        for (float i = 0; i < 1; i+=0.01f)
        {
            transform.position = Vector3.Lerp(position, goal, i);
            yield return new WaitForSeconds(0.005f);
        }
    }

    IEnumerator PushKnight(GameObject knight)
    {
        knight.GetComponent<KnightBehaviour>().is_pushed = true;
        knight.GetComponent<Rigidbody>().isKinematic = false;
        knight.GetComponent<Rigidbody>().AddForce(transform.forward*3000);
        yield return new WaitForSeconds(0.5f);
        if (knight != null)
        {
            knight.GetComponent<KnightBehaviour>().is_pushed = false;
        }
    }

    IEnumerator PushArcher(GameObject archer)
    {
        archer.GetComponent<ArcherBehaviour>().is_pushed = true;
        archer.GetComponent<Rigidbody>().isKinematic = false;
        archer.GetComponent<Rigidbody>().AddForce(transform.forward * 3000);
        yield return new WaitForSeconds(0.5f);
        if (archer != null)
        {
            archer.GetComponent<ArcherBehaviour>().is_pushed = false;
        }
    }
}
