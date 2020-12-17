using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Internal;
public class ButtonBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject Eyes;
    [SerializeField]
    private AudioSource Button;
    [SerializeField]
    private Dropdown MapSelec;
    [SerializeField]
    private AudioSource ReturnSound;
    [SerializeField]
    private AudioSource SelectSound;
    public static int CharacterSelection; /*0=Knight, 1=Archer, 2=Mage,3=Druid,4=Thief*/
    public static string MapSelection;
    private void Start()
    {

        
    }
    public void Play()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            Button.Play();
            StartCoroutine("LightEyes");
        }
        else
        {
            SceneManager.LoadScene(1);
            PlayerPrefs.SetInt(string.Concat("NbGames", CharacterSelection), PlayerPrefs.GetInt(string.Concat("NbGames", CharacterSelection)) + 1);
            Debug.Log("" + PlayerPrefs.GetInt(string.Concat("NbGames", CharacterSelection)));
        }
    }
    public void Option()
    {
        Button.Play();
        SceneManager.LoadScene(2);
    }
    public void Return()
    {
        ReturnSound.Play();
        SceneManager.LoadScene(0);
        Debug.Log("" + CharacterSelection);
    }
    public void Leave()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
    public void Choose()
    {
        Button.Play();
        SceneManager.LoadScene(3);
    }
    public void SelectMap()
    {
        MapSelection = MapSelec.options[MapSelec.value].text;
    }
    public void Select()
    {
        SelectSound.Play();
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
    public IEnumerator LightEyes()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            Eyes.SetActive(true);
            GameObject Door = GameObject.Find("CastleDoor");
            float t;
            float nbFrame = 2 / Time.deltaTime;
            for (int i = 1; i < nbFrame + 1; i++)
            {
                t = i / nbFrame;
                Door.transform.position = Vector3.Lerp(Door.transform.position, Door.transform.position -new Vector3(0,0.5f,0), t);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(2);
            var op =SceneManager.LoadSceneAsync(1);
        }
    }
}
