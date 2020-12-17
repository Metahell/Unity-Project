using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> Maps=new List<GameObject>();
    void Start()
    {
        if (ButtonBehavior.MapSelection == 0)
        {
            Maps[Random.Range(0, Maps.Capacity)].SetActive(true);
        }
        else
        {
            Maps[ButtonBehavior.MapSelection-1].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
