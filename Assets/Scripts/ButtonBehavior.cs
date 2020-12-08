using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    public static int CharacterSelection; /*0=Knight, 1=Archer, 2=Mage,3=Druid,4=Thief*/
    private void Start()
    {
        
        
    }
    public void Play()
    {
        SceneManager.LoadScene(0);
        PlayerPrefs.SetInt(string.Concat("NbGames", CharacterSelection), PlayerPrefs.GetInt(string.Concat("NbGames", CharacterSelection)) + 1);
        Debug.Log("" + PlayerPrefs.GetInt(string.Concat("NbGames", CharacterSelection)));
    }
    public void Option()
    {
        SceneManager.LoadScene(2);
    }
    public void Return()
    {
        SceneManager.LoadScene(1);
        Debug.Log("" + CharacterSelection);
    }
    public void Leave()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
    public void Choose()
    {
        SceneManager.LoadScene(3);
    }
    public void Select()
    {
        if(EventSystem.current.currentSelectedGameObject.name == "Select Knight")
        {
            CharacterSelection = 0;
        }
        if(EventSystem.current.currentSelectedGameObject.name == "Select Archer")
        {
            CharacterSelection = 1;
        }
        if (EventSystem.current.currentSelectedGameObject.name == "Select Mage")
        {
            CharacterSelection = 2;
        }
        if (EventSystem.current.currentSelectedGameObject.name == "Select Druid")
        {
            CharacterSelection = 3;
        }
        if (EventSystem.current.currentSelectedGameObject.name == "Select Thief")
        {
            CharacterSelection = 4;
        }
    }
}
