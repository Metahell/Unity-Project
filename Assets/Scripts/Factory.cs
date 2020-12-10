using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField]
    private Arrow ArrowPrefab;

    private static Factory instance;

    public static Factory GetInstance()
    {
        return instance;
    }

    private Queue<Arrow> pool = new Queue<Arrow>();

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else instance = this;
    }

    public void RemoveArrow(Arrow a)
    {
        a.gameObject.SetActive(false);
        pool.Enqueue(a);
    }

    public GameObject GetArrow()
    {
        //On a déjà des balles disponibles
        if (pool.Count > 0)
        {
            GameObject bulletObj = pool.Dequeue().gameObject;
            bulletObj.SetActive(true);
            return bulletObj;
        }
        else
        {
            return Instantiate(ArrowPrefab, transform).gameObject;
        }
    }
}
