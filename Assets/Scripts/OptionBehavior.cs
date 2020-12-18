﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
public class OptionBehavior : MonoBehaviour
{
    [SerializeField]
    private Dropdown resolution;
    [SerializeField]
    private Dropdown quality;
    public static bool godmode;
    [SerializeField]
    private Button godmodeButton;
    // Start is called before the first frame update
    void Start()
    {
        quality.value = PlayerPrefs.GetInt("Quality", 3);
        godmode = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeResolution()
    {
        string input = resolution.options[resolution.value].text;
        string[] resDim = input.Split('x');
        Screen.SetResolution(int.Parse(resDim[0]), int.Parse(resDim[1]), Screen.fullScreen, 60);
    }
    public void ChangeGraphics()
    {
        QualitySettings.SetQualityLevel(quality.value);
        PlayerPrefs.SetInt("Quality",quality.value);
    }

    public void ToglleGodmode()
    {
        godmode = !godmode;
        godmodeButton.GetComponentInChildren<Text>().text = godmode ? "Godmode on" : "Godmode off";
    }
}
