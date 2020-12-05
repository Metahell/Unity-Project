using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthOrb : MonoBehaviour
{
    [SerializeField]
    private int hpmax=100;
    private int currentHp;
    [SerializeField]
    private GameObject healthOrb;
    [SerializeField]
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = hpmax;
        currentHp = hpmax;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = currentHp;
        if (Input.GetKey(KeyCode.Space))
        {
            currentHp -= 1;
        }
    }
    public void UpdateOrb()
    {
        healthOrb.transform.position -= new Vector3(0,(slider.maxValue-slider.value)*2.62f/slider.maxValue,0);
    }
}
