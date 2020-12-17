using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
public class OptionBehavior : MonoBehaviour
{
    [SerializeField]
    private Dropdown resolution;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeResolution()
    {
        string input = resolution.options[resolution.value].text;
        string[] resDim = input.Split('x');
        Screen.SetResolution(int.Parse(resDim[0]), int.Parse(resDim[1]), false, 60);
    }
}
