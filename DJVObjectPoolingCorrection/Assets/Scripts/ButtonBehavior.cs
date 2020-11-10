using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(0);
    }
    public void Option()
    {
        SceneManager.LoadScene(2);
    }
    public void Return()
    {
        SceneManager.LoadScene(1);
    }
    public void Leave()
    {
        EditorApplication.Exit(0);
    }
}
