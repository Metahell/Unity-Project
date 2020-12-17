using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    [SerializeField]
    private Bullet bulletPrefab;

    private static BulletFactory instance;

    public static BulletFactory GetInstance()
    {
        return instance;
    }

    private Queue<Bullet> pool = new Queue<Bullet>();

    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Il y a déjà une instance de la classe BulletFactory " + name);
            Destroy(this);
        }
        else instance = this;
    }

    public void RemoveBullet(Bullet b)
    {
        b.gameObject.SetActive(false);
        pool.Enqueue(b);
    }

    public GameObject GetBullet()
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
            return Instantiate(bulletPrefab, transform).gameObject;
        }
    }
}
