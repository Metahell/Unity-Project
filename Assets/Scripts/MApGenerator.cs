using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MApGenerator : MonoBehaviour
{
    public GameObject maptile;
    public int _length;
    public int _width;
    public int radius;
    public Transform startingpoint;
    public Color color;
    public GameObject Wall;
    void Start()
    {
        Material[] mat=maptile.GetComponent<MeshRenderer>().sharedMaterials;
        mat[0].color = color;
        maptile.GetComponent<MeshRenderer>().sharedMaterials[0] = mat[0];
        int count = 0;
        for (int x = -radius; x <= radius; x++)
        {
            for (int z = -radius; z <= radius; z++)
            {
                GameObject temp = Instantiate(maptile, new Vector3(x * _width+startingpoint.position.x, 0, z * _length+startingpoint.position.z), Quaternion.identity) as GameObject;
                temp.transform.parent = startingpoint.transform;
                temp.name = count.ToString();
                count++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
