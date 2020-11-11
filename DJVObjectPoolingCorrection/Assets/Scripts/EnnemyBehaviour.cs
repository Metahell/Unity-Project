using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBehaviour : MonoBehaviour
{
    int velocity;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] positionArray = new [] { new Vector3(1f,0f,0f), new Vector3(-1f,0f,0f),new Vector3(0f,1f,0f),new Vector3(0f,-1f,0f),new Vector3(0f,0f,1f),new Vector3(0f,0f,-1f) }
        int direction = Random.range(0,6);
        transform.
    }
}
