using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    [SerializeField]
    public ParticleSystem part;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Enemy" || other.tag == "Archer" || other.tag == "Boss")
        {
            if (other.tag == "Enemy")
            {
                other.GetComponent<KnightBehaviour>().poisontimer = 3;
            }
            if (other.tag == "Archer")
            {
                other.GetComponent<ArcherBehaviour>().poisontimer = 3;
            }
            if (other.tag == "Shaman")
            {
                other.GetComponent<ShamanBehavior>().poisontimer = 3;
            }
            if (other.tag == "Boss")
            {
                other.GetComponent<GolemBehavior>().poisontimer = 3;
            }
        }
    }
}
