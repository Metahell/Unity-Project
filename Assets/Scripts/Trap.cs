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
        if(collision.collider.CompareTag("Enemy")|| collision.collider.CompareTag("Archer"))
        {
            if (!triggered)
            {
                triggered = true;
                animator.SetTrigger("Triggered");
                StartCoroutine(Disappear(collision.collider.gameObject));
            }
        }
    }
    private IEnumerator Disappear(GameObject trapped)
    {
        yield return new WaitForSeconds(0.4f);
        TrapSound.Play();
        if (trapped.CompareTag("Enemy"))
        {
            trapped.GetComponent<KnightBehaviour>().LooseHealth(10);
        }
        else if (trapped.CompareTag("Archer"))
        {
            trapped.GetComponent<ArcherBehaviour>().LooseHealth(10);
        }
        yield return new WaitForSeconds(1);
        Factory.GetInstance().RemoveTrap(this);
    }
}
