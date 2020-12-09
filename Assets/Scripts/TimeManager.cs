using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeManager : MonoBehaviour
{
    [SerializeField]
    private Text TextTime;
    private bool end=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!end)
        {
            TextTime.text = "TIME :" + Time.time.ToString("F1");
        }
    }
    public string EndTime()
    {
        end = true;
        return Time.time.ToString("f2");
    }
}
