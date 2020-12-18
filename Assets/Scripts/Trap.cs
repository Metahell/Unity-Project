using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private KnightBehaviour KnightBehaviour;
    [SerializeField]
    private AudioSource TrapSound;
    private bool triggered = false;
    [SerializeField]
    void Start()
    {

    }
    private void Awake()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Archer") || collision.collider.CompareTag("Shaman") || collision.collider.CompareTag("Boss"))
        {
            if (!triggered)
            {
                triggered = true;
                animator.SetTrigger("Triggered");
                StartCoroutine(Disappear(collision.collider.gameObject));
            }
        }
    }

    private void OntriggerEnter()
    {


    }

    private IEnumerator Disappear(GameObject trapped)
    {
        yield return new WaitForSeconds(0.1f);
        TrapSound.Play();
        if (trapped.CompareTag("Enemy"))
        {
            trapped.GetComponent<KnightBehaviour>().trapped = true;
            yield return new WaitForSeconds(1);
            trapped.GetComponent<KnightBehaviour>().trapped = false;
        }
        else if (trapped.CompareTag("Archer"))
        {
            trapped.GetComponent<ArcherBehaviour>().trapped = true;
            yield return new WaitForSeconds(1);
            trapped.GetComponent<ArcherBehaviour>().trapped = false;
        }
        else if (trapped.CompareTag("Shaman"))
        {
            trapped.GetComponent<ShamanBehavior>().trapped = true;
            yield return new WaitForSeconds(1);
            trapped.GetComponent<ShamanBehavior>().trapped = false;
        }
        else if (trapped.CompareTag("Boss"))
        {
            trapped.GetComponent<GolemBehavior>().trapped = true;
            yield return new WaitForSeconds(1);
            trapped.GetComponent<GolemBehavior>().trapped = false;
        }
        yield return new WaitForSeconds(1);
        Factory.GetInstance().RemoveTrap(this);
    }
}
