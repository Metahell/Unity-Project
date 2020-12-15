using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private KnightBehaviour KnightBehaviour;
    private bool triggered = false;
    [SerializeField]
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Enemy")|| collision.collider.CompareTag("Archer"))
        {
            if (!triggered)
            {
                triggered = true;
                animator.SetTrigger("Triggered");
                if (collision.collider.CompareTag("Enemy"))
                {
                    collision.collider.GetComponent<KnightBehaviour>().LooseHealth(10);
                }
                else if (collision.collider.CompareTag("Archer"))
                {
                    collision.collider.GetComponent<ArcherBehaviour>().LooseHealth(10);
                }
                Disappear();
            }
        }
    }
    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(1);
        Factory.GetInstance().RemoveTrap(this);
    }
}
