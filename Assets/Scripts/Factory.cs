using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField]
    private Arrow ArrowPrefab;
    [SerializeField]
    private Fireball FireballPrefab;
    [SerializeField]
    private Axe AxePrefab;
    private static Factory instance;
    public static Factory GetInstance()
    {
        return instance;
    }

    private Queue<Arrow> pool = new Queue<Arrow>();
    private Queue<Fireball> poolf = new Queue<Fireball>();
    private Queue<Axe> poola = new Queue<Axe>();

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
    public GameObject GetFireball()
    {
        //On a déjà des balles disponibles
        if (poolf.Count > 0)
        {
            GameObject bulletObj = poolf.Dequeue().gameObject;
            bulletObj.SetActive(true);
            return bulletObj;
        }
        else
        {
            return Instantiate(FireballPrefab, transform).gameObject;
        }
    }

    public void RemoveFireball(Fireball f)
    {
        f.gameObject.SetActive(false);
        poolf.Enqueue(f);
    }
    public void RemoveAxe(Axe a)
    {
        a.gameObject.SetActive(false);
        poola.Enqueue(a);
    }

    public GameObject GetAxe()
    {
        //On a déjà des balles disponibles
        if (pool.Count > 0)
        {
            GameObject bulletObj = poola.Dequeue().gameObject;
            bulletObj.SetActive(true);
            return bulletObj;
        }
        else
        {
            return Instantiate(AxePrefab, transform).gameObject;
        }
    }
}
