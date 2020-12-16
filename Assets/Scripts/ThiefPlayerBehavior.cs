using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefPlayerBehavior : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private KnightBehaviour KnightBehaviour;
    [SerializeField]
    private ParticleSystem Smoke;
    [SerializeField]
    private static bool isInvisible = false;
    private float invisibleCountdown = 0;
    [SerializeField]
    private GameObject Mat;
    [SerializeField]
    private Material Default;
    [SerializeField]
    private Material Invisible;
    [SerializeField]
    private AudioSource slash;
    private float _ability1Time = 1;
    private float _ability1Timer = 1;
    private float _ability2Time = 4;
    private float _ability2Timer = 5;
    private float _ability3Time = 15;
    private float _ability3Timer = 5;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        _ability1Timer += Time.deltaTime;
        _ability2Timer += Time.deltaTime;
        _ability3Timer += Time.deltaTime;
        if (invisibleCountdown >= 0)
        {
            Debug.Log(isInvisible);
            Debug.Log(Mat.GetComponent<SkinnedMeshRenderer>().materials[0]);
            invisibleCountdown -= Time.deltaTime;
        }
        if(invisibleCountdown <= 0&&isInvisible)
        {
            isInvisible = false;
            Material[] mats = Mat.GetComponent<SkinnedMeshRenderer>().materials;
            mats[0] = Default;
            Mat.GetComponent<SkinnedMeshRenderer>().materials = mats;
        }
        if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(1).IsTag("1") && _ability1Timer >= _ability1Time)
        {
            animator.SetTrigger("1st Ability");
            _ability1Timer = 0;
        }
        if (Input.GetMouseButtonDown(1) && !animator.GetCurrentAnimatorStateInfo(1).IsTag("1") && _ability2Timer >= _ability2Time)
        {
            animator.SetTrigger("2nd Ability");
            // le reset du timer est dans la fonction d'abilité au cas où la position de la souris n'est pas bonne
        }
        if (Input.GetKey(KeyCode.Space) && !animator.GetCurrentAnimatorStateInfo(1).IsTag("1") && _ability3Timer >= _ability3Time)
        {
            animator.SetTrigger("3rd Ability");
            _ability3Timer = 0;
        }
    }
    public void Ability1()
    {
        Vector3 attackCenter = transform.position + transform.forward;
        Collider[] colliders = Physics.OverlapSphere(attackCenter, 2f);
        /**GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = attackCenter;
        sphere.transform.localScale = new Vector3(1.5f,1.5f,1.5f);**/
        slash.Play();
        int dmg = 5;
        if (isInvisible)
        {
            dmg *= 2;
            invisibleCountdown = 0;
        }
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                Debug.Log(Mathf.Abs(Vector3.Angle(transform.forward, collider.gameObject.transform.forward) - 180) >= 90);
                if (Mathf.Abs(Vector3.Angle(transform.forward, collider.gameObject.transform.forward) - 180) >= 90)
                {
                    collider.gameObject.GetComponent<KnightBehaviour>().LooseHealth(10);
                }
                else
                {
                    collider.gameObject.GetComponent<KnightBehaviour>().LooseHealth(5);
                }
            }
            else if (collider.gameObject.CompareTag("Archer"))
            {
                if (Mathf.Abs(Vector3.Angle(transform.forward, collider.gameObject.transform.forward) - 180) >= 90)
                {
                    collider.gameObject.GetComponent<ArcherBehaviour>().LooseHealth(10);
                }
                else
                {
                    collider.gameObject.GetComponent<ArcherBehaviour>().LooseHealth(5);
                }
            }
            else if (collider.gameObject.CompareTag("Boss"))
            {
                if (Mathf.Abs(Vector3.Angle(transform.forward, collider.gameObject.transform.forward) - 180) >= 90)
                {
                    collider.gameObject.GetComponent<GolemBehavior>().LooseHealth(10);
                }
                else
                {
                    collider.gameObject.GetComponent<GolemBehavior>().LooseHealth(5);
                }
            }
        }
    }
    public void Ability2()
    {
        isInvisible=true;
        invisibleCountdown = 1;
        Material[] mats = Mat.GetComponent<SkinnedMeshRenderer>().materials;
        mats[0] = Invisible;
        Mat.GetComponent<SkinnedMeshRenderer>().materials = mats;
        Smoke.Play();
    }
}
