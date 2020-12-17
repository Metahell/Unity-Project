using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> MapPool = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InitiateMap();
    }

    private void InitiateMap()
    {
        for (int i = 0; i < MapPool.Count; i++)
        {
            MapPool[i].gameObject.SetActive(false);
        }
        MapPool[Random.Range(0, MapPool.Count)].gameObject.SetActive(true);
    }
}
