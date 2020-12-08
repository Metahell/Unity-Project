using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private int health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void LooseHealth(int healthLoss)
    {
        health -= healthLoss;
        StartCoroutine("Red");
    }

    IEnumerator Red()
    {


        Transform cube = gameObject.transform.Find("Cube.002");
        Material[] materials = cube.GetComponent<Renderer>().materials;
        Color color0 = materials[0].color;
        Color color1 = materials[1].color;
        Color color2 = materials[2].color;
        Color color3 = materials[3].color;
        materials[0].color = Color.red;
        materials[1].color = Color.red;
        materials[2].color = Color.red;
        materials[3].color = Color.red;
        yield return new WaitForSeconds(0.1f);
        materials[0].color = color0;
        materials[1].color = color1;
        materials[2].color = color2;
        materials[3].color = color3;
    }
}
