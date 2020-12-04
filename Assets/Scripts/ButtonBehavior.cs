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
        UpdateText();
        SetText();
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
        for (int i = 0; i< 5; i++) {
            string SaveString = GameObject.Find(string.Concat("Achievements", i)).GetComponent<Text>().text = " Highest Wave Reached : " + PlayerPrefs.GetInt(string.Concat("WaveSaved", i), 0)+"\n"
                 + "Games Played : " + PlayerPrefs.GetInt(string.Concat("NbGames", i),0)+"\n"
                 + "(Medal)";
            PlayerPrefs.SetString(string.Concat("Stats", i), SaveString);
            PlayerPrefs.Save();
        }
    }
    public void SetText()
    {
        for (int i = 0; i < 5; i++)
        {
            string SaveString = GameObject.Find(string.Concat("Achievements", i)).GetComponent<Text>().text = PlayerPrefs.GetString(string.Concat("Stats", i));
        }
    }
}
