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
    public void Play()
    {
        SceneManager.LoadScene(0);
        PlayerPrefs.SetInt(string.Concat("NbGames", CharacterSelection), PlayerPrefs.GetInt(string.Concat("NbGames", CharacterSelection)) + 1);
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
        EditorApplication.Exit(0);
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
    public void UpdateText() //Regarde le Personnage actuel et change les infos de la boîte de texte correspondante en concaténant et en cherchant le nom
    {
       string SaveString=GameObject.Find(string.Concat("Achievements", CharacterSelection)).GetComponent<Text>().text = " Highest Wave Reached : "+PlayerPrefs.GetString(string.Concat("WaveSaved",CharacterSelection),"0")
            + "Games Played : "+PlayerPrefs.GetString(string.Concat("NbGames", CharacterSelection), "0")
            + "(Medal)";
        PlayerPrefs.SetString(string.Concat("Stats", CharacterSelection), SaveString);
        PlayerPrefs.Save();
    }
}
