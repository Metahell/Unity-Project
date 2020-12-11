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
    // Use this for initialization
    void Awake()
    {
        rigi = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
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
            }
        }
        else
        {
            Remove();
        }
    }

    public void Remove()
    {
        Factory.GetInstance().RemoveAxe(this);
    }
}
