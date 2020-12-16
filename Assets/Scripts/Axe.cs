using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    private Rigidbody rigi;
    private Renderer renderer;
    private KnightBehaviour Health;
    private KnightBehaviour KnightBehaviour;
    private Vector3 direction;
    [SerializeField]
    private Vector3 rotateVelocity;
    [SerializeField]
    private float speed;
    private float life_timer;
    // Use this for initialization
    void Awake()
    {
        rigi = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        life_timer = 0;
    }
    public void setDirection(Vector3 transf)
    {
        direction=transf;
    }
    private void FixedUpdate()
    {
        rigi.velocity = direction * speed;
        Quaternion deltaRotation = Quaternion.Euler(rotateVelocity * Time.deltaTime);
        rigi.MoveRotation(rigi.rotation*deltaRotation);
    }

    private void Update()
    {
        life_timer += Time.deltaTime;
        if (life_timer > 1f) // pour l'instant 1 secondes c'est pas trop impactant pour la map de test, faudra check ça avec les maps définitives
        {
            Remove();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")|| other.CompareTag("Archer")|| other.CompareTag("Boss"))
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, 1.5f);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    collider.gameObject.GetComponent<KnightBehaviour>().LooseHealth(5);
                }
                else if (collider.gameObject.CompareTag("Archer"))
                {
                    collider.gameObject.GetComponent<ArcherBehaviour>().LooseHealth(5);
                }
                else if (collider.gameObject.CompareTag("Boss"))
                {
                    collider.gameObject.GetComponent<GolemBehavior>().LooseHealth(5);
                }
                else if (other.gameObject.CompareTag("Shaman"))
                {
                    other.gameObject.GetComponent<ShamanBehavior>().LooseHealth(5);
                }
            }
        }
        if (other.CompareTag("Player"))
        {

        }
        else
        {
            Remove();
        }
    }

    public void Remove()
    {
        life_timer = 0;
        Factory.GetInstance().RemoveAxe(this);
    }
}
