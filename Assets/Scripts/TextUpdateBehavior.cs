using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TextUpdateBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject Medal0;
    [SerializeField]
    private GameObject Medal1;
    [SerializeField]
    private GameObject Medal2;
    [SerializeField]
    private GameObject Medal3;
    [SerializeField]
    private GameObject Medal4;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "CharacterSelection")
        {
            UpdateText();
            SetText();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetText()
    {
        for (int i = 0; i < 5; i++)
        {
            string SaveString = GameObject.Find(string.Concat("Achievements", i)).GetComponent<Text>().text = PlayerPrefs.GetString(string.Concat("Stats", i));
        }
    }
    public void UpdateText() //Regarde le Personnage actuel et change les infos de la boîte de texte correspondante en concaténant et en cherchant le nom
    {
        for (int i = 0; i < 5; i++)
        {
            string SaveString = GameObject.Find(string.Concat("Achievements", i)).GetComponent<Text>().text = " Highest Wave Reached : " + PlayerPrefs.GetInt(string.Concat("WaveSaved", i), 0) + "\n"
                 + "Games Played : " + PlayerPrefs.GetInt(string.Concat("NbGames", i), 0) + "\n";
            PlayerPrefs.SetString(string.Concat("Stats", i), SaveString);
            if (PlayerPrefs.GetInt(string.Concat("Medal", i), 0) == 1)
            {
                switch (i)
                {
                    case 0: Medal0.SetActive(true);break;
                    case 1: Medal1.SetActive(true); break;
                    case 2: Medal2.SetActive(true); break;
                    case 3: Medal3.SetActive(true); break;
                    case 4: Medal4.SetActive(true); break;
                    default:; break;
                }
            }
        }
        PlayerPrefs.Save();
    }

}
          