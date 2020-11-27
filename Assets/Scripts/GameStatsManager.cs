using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    int WaveReached;
    int NbGames;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SaveStats()
    {
        PlayerPrefs.SetInt(string.Concat("SavedWave",ButtonBehavior.CharacterSelection),WaveReached);
        PlayerPrefs.SetInt(string.Concat("SaveNbGame", ButtonBehavior.CharacterSelection), NbGames);
        PlayerPrefs.Save();
    }
}
