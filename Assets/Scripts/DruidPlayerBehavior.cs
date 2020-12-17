using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DruidPlayerBehavior : MonoBehaviour
{
    [SerializeField]
    private Image img1;
    [SerializeField]
    private Image img2;
    [SerializeField]
    private Image img3;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject wolf;
    private float radius;
    private float _ability1Time = 1;
    private float _ability1Timer = 1;
    private float _ability2Time = 10;
    private float _ability2Timer = 10;
    private float _ability3Time = 20;
    private float _ability3Timer = 20;
    private void Start()
    {
        radius =wolf.GetComponent<CapsuleCollider>().radius;
    }
    void Update()
    {
        _ability1Timer += Time.deltaTime;
        _ability2Timer += Time.deltaTime;
        _ability3Timer += Time.deltaTime;
        UpdateUI();
        if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(1).IsTag("1") && _ability1Timer >= _ability1Time)
        {
            animator.SetTrigger("1st Ability");
            _ability1Timer = 0;
        }
        if (Input.GetMouseButtonDown(1) && !animator.GetCurrentAnimatorStateInfo(1).IsTag("1") && _ability2Timer >= _ability2Time)
        {
            animator.SetTrigger("2nd Ability");
            _ability2Timer = 0;
        }
        if (Input.GetKey(KeyCode.Space) && !animator.GetCurrentAnimatorStateInfo(1).IsTag("1") && _ability3Timer >= _ability3Time && GameObject.FindGameObjectsWithTag("Wolf").Length == 0)
        {
            animator.SetTrigger("3rd Ability");
            Ability3();
            _ability3Timer = 0;
        }
    }

    public void Ability1()
    {
        Vector3 direction = transform.forward;
        GameObject axe = Factory.GetInstance().GetAxe();
        axe.transform.position = spawnPoint.position;
        axe.GetComponent<Axe>().setDirection(transform.forward);
        axe.transform.right = direction;

    } 
    
    public void Ability2()
    {
        gameObject.GetComponent<HealthOrb>().Damage(-10);
        if (GameObject.FindGameObjectsWithTag("Wolf").GetLength(0) > 0)
        {
            GameObject.FindGameObjectsWithTag("Wolf")[0].GetComponent<Wolf>().LooseHealth(-10);
        }
        
    }

    public void Ability3()
    {
        Vector3 DruidPos = transform.position;
        Vector3 Spawn = new Vector3(Random.Range(DruidPos.x - 4,DruidPos.x + 4), 0.017f, Random.Range(DruidPos.z - 4, DruidPos.z + 4));
        while (!CheckSpawn(Spawn,radius))
        {
            Spawn = new Vector3(Random.Range(DruidPos.x - 4, DruidPos.x + 4), 0.017f, Random.Range(DruidPos.z - 4, DruidPos.z + 4));
        }
        Instantiate(wolf, Spawn,transform.rotation);
    }
   
    private bool CheckSpawn(Vector3 center,float radius)
    {
        return Physics.CheckSphere(center,radius);
    }
    public void UpdateUI()
    {
        img1.fillAmount = _ability1Timer < _ability1Time ? _ability1Timer / _ability1Time : 0;
        img2.fillAmount = _ability2Timer < _ability2Time ? _ability2Timer / _ability2Time : 0;
        img3.fillAmount = _ability3Timer < _ability3Time ? _ability3Timer / _ability3Time : 0;
    }
}
