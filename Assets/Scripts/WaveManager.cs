using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    private int WaveID = 1;
    private int LastWave = 1;
    private int CurrentWave = 1;
    private bool end=false;
    [SerializeField]
    private GameObject EndgameCanvas;
    [SerializeField]
    private PlayerController controller;
    [SerializeField]
    private Text EndGameText;
    [SerializeField]
    private Text WaveNumber;
    [SerializeField]
    private TimeManager tm;
    [SerializeField]
    private List<GameObject> SpawnPoints = new List<GameObject>();
    [SerializeField]
    private List<GameObject> EnnemyPool= new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Spawn");
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckEnemyCount() == 0)
        {
            if (WaveID == 11)
            {
                if (!end)
                {
                    end = true;
                    Win();
                }
            }
            else
            {
                WaveNumber.text = "Wave " + WaveID;
                StartCoroutine("Spawn");
            }
        }
    }
    private void NewWave()
    {
        int NewWave = CurrentWave + LastWave;
        LastWave = CurrentWave;
        CurrentWave = NewWave;
    }
    private IEnumerator Spawn()
    {
        for(int i = 0; i < CurrentWave; i++)
        {
            GameObject mob=Instantiate(EnnemyPool[(i%EnnemyPool.Count)]);
            mob.transform.position = SpawnPoints[i % SpawnPoints.Count].transform.position;

            yield return new WaitForSeconds(1);
        }
        NewWave();
        WaveID++;
        PlayerPrefs.SetInt(string.Concat("WaveSaved", ButtonBehavior.CharacterSelection),WaveID);
    }
    private int CheckEnemyCount()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;

    }
    public void Lose()
    {
        controller.enabled = false;
        EndgameCanvas.SetActive(true);
        EndGameText.text = "GAME OVER\n TIME :"+tm.EndTime();

    }
    private void Win()
    {
        controller.enabled = false;
        EndgameCanvas.SetActive(true);
        EndGameText.text = "YOU WON\n TIME :" +tm.EndTime();
        PlayerPrefs.SetInt(string.Concat("Medal", ButtonBehavior.CharacterSelection), 1);
        PlayerPrefs.Save();
    }
}
                                      