﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightPlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    private Image img1;
    [SerializeField]
    private Image img2;
    [SerializeField]
    private Image img3;
    [SerializeField]
    private Animator animator;
    private KnightBehaviour KnightBehaviour;
    public bool is_jumping;
    [SerializeField]
    private GameObject StarParticle;
    [SerializeField]
    private AudioSource slash;
    private float _ability1Time = 1;
    private float _ability1Timer = 1;
    private float _ability2Time = 5;
    private float _ability2Timer = 5;
    private float _ability3Time = 5;
    private float _ability3Timer = 5;
    // Start is called before the first frame update
    void Start()
    {
        is_jumping = false;
    }

    // Update is called once per frame
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
        if (Input.GetMouseButtonDown(1) && !animator.GetCurrentAnimatorStateInfo(1).IsTag("1") && _ability2Timer >= _ability2Time && !is_jumping)
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
            else if (collider.gameObject.CompareTag("Boss"))
            {
               collider.gameObject.GetComponent<GolemBehavior>().LooseHealth(5);
            }
        }
    }

    public void Ability2()
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity) && hit.transform.tag == "Floor")
        {
            StartCoroutine(Jump());
            _ability2Timer = 0;
        }

    }

    public void Ability3()
    {
        StartCoroutine(Spin());
    }

    IEnumerator Spin()    {
        float attacktime = 0f;
        PlayerController player = gameObject.GetComponent<PlayerController>();
        player.maxVelocity *= 1.5f;
        for (float i = 0; i < 3; i+= Time.deltaTime)
        {
            if (attacktime > 0.5f)
            {
                Vector3 attackCenter = transform.position;
                Collider[] colliders = Physics.OverlapSphere(attackCenter, 3f);
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject.CompareTag("Enemy"))
                    {
                        collider.gameObject.GetComponent<KnightBehaviour>().LooseHealth(1);
                    }
                    else if (collider.gameObject.CompareTag("Archer"))
                    {
                        collider.gameObject.GetComponent<ArcherBehaviour>().LooseHealth(1);
                    }
                    else if (collider.gameObject.CompareTag("Boss"))
                    {
                        collider.gameObject.GetComponent<GolemBehavior>().LooseHealth(1);
                    }
                }
                attacktime = 0f;
            }
            attacktime += Time.deltaTime;
            yield return null;
        }
        player.maxVelocity /= 1.5f;
    }

    IEnumerator Jump()
    {
        /**Vector3 position = transform.position;
        Vector3 goal = transform.position + 10 * transform.forward;
        for (float i = 0; i < 1; i+=Time.deltaTime)
        {
            transform.position = Vector3.Lerp(position, goal, i);
            yield return null;
        }**/
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * 10 + transform.up * 10;
        yield return new WaitForSeconds(0.1f);
        is_jumping = true;
        
    }

    IEnumerator PushKnight(GameObject knight)
    {
        Rigidbody rigidKnight = knight.GetComponent<Rigidbody>();
        KnightBehaviour knightB = knight.GetComponent<KnightBehaviour>();
        knightB.is_pushed = true;
        rigidKnight.isKinematic = false;
        rigidKnight.AddForce(transform.forward*3000);
        yield return new WaitForSeconds(0.5f);
        if (knight != null)
        {
            knightB.is_pushed = false;
        }
    }

    IEnumerator PushArcher(GameObject archer)
    {
        ArcherBehaviour archerB = archer.GetComponent<ArcherBehaviour>();
        Rigidbody rigidArcher = archer.GetComponent<Rigidbody>();
        archerB.is_pushed = true;
        rigidArcher.isKinematic = false;
        rigidArcher.AddForce(transform.forward * 3000);
        yield return new WaitForSeconds(0.5f);
        if (archer != null)
        {
            archerB.is_pushed = false;
        }
    }
    public void UpdateUI()
    {
        img1.fillAmount = _ability1Timer < _ability1Time ? _ability1Timer / _ability1Time : 0;
        img2.fillAmount = _ability2Timer < _ability2Time ? _ability2Timer / _ability2Time : 0;
        img3.fillAmount = _ability3Timer < _ability3Time ? _ability3Timer / _ability3Time : 0;
    }
    private void OnCollisionEnter(Collision collision)
    {
        is_jumping = false;
        if ((collision.collider.CompareTag("Floor")|| collision.collider.CompareTag("Enemy")|| collision.collider.CompareTag("Archer")) && is_jumping)
        {
            Instantiate(StarParticle, transform.position,transform.rotation);
            Vector3 attackCenter = transform.position;
            Collider[] colliders = Physics.OverlapSphere(attackCenter, 4f);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    collider.gameObject.GetComponent<KnightBehaviour>().LooseHealth(2);
                    StartCoroutine(PushKnight(collider.gameObject));
                }
                else if (collider.gameObject.CompareTag("Archer"))
                {
                    collider.gameObject.GetComponent<ArcherBehaviour>().LooseHealth(2);
                    StartCoroutine(PushArcher(collider.gameObject));
                }
                else if (collider.gameObject.CompareTag("Boss"))
                {
                    collider.gameObject.GetComponent<GolemBehavior>().LooseHealth(2);
                }

            }
        }
    }
}
