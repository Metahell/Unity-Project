using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthOrb : MonoBehaviour
{
    [SerializeField]
    private int hpmax;
    private int currentHp;
    private bool end = false;
    [SerializeField]
    private GameObject healthOrb;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private WaveManager WaveManager;
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = hpmax;
        currentHp = hpmax;

    }

    // Update is called once per frame
    void Update()
    {
        if (currentHp <= 0)
        {
            if (!end)
            {
                end= true;
                WaveManager.Lose();
            }
        }
    }

    public void Damage(int dmg)
    {
        currentHp -= dmg;
        currentHp = currentHp > hpmax ? hpmax : currentHp;
        slider.value = currentHp;
        Debug.Log(currentHp);
    }
    public void UpdateOrb()
    {
        healthOrb.transform.position = new Vector3(healthOrb.transform.position.x,-(slider.maxValue-slider.value)*133f/slider.maxValue+78.37385f,healthOrb.transform.position.z);
    }

    public int getHealth()
    {
        return currentHp;
    }
}
