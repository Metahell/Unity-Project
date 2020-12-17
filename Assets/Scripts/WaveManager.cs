using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource GameOver;
    [SerializeField]
    private AudioSource WinSound;
    private int WaveID = 1;
    private int LastWave = 1;
    private int CurrentWave = 1;
    private bool end=false;
    [SerializeField]
    private GameObject EndgameCanvas;
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
    [SerializeField]
    private GameObject Boss;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>();
        StartCoroutine("Spawn");
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckEnemyCount() == 0)
        {
            if (WaveID == 10)
            {
                WaveNumber.text = "Wave " + WaveID;
                StartCoroutine("Spawn");
                StartCoroutine("SpawnBoss");
            }
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
            int end = WaveID > 1 ? (WaveID > 2 ? EnnemyPool.Count : EnnemyPool.Count-1) : EnnemyPool.Count-2; //pas de shaman avant wave 3, pas d'archer avant wave 2
            GameObject mob=Instantiate(EnnemyPool[Random.Range(0,end)]);
            mob.transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Count)].transform.position; //i % SpawnPoints.Count

            yield return new WaitForSeconds(1);
        }
        NewWave();
        WaveID++;
        PlayerPrefs.SetInt(string.Concat("WaveSaved", ButtonBehavior.CharacterSelection),WaveID);
    }
    private IEnumerator SpawnBoss()
    {
        Boss.SetActive(true);
        Boss.transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Count)].transform.position; //i % SpawnPoints.Count
        yield return new WaitForSeconds(1);
        NewWave();
        WaveID++;
        PlayerPrefs.SetInt(string.Concat("WaveSaved", ButtonBehavior.CharacterSelection), WaveID);
    }
    private int CheckEnemyCount()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length + GameObject.FindGameObjectsWithTag("Archer").Length + GameObject.FindGameObjectsWithTag("Shaman").Length + GameObject.FindGameObjectsWithTag("Boss").Length;
    }
    public void Lose()
    {
        controller.Death();
        EndgameCanvas.SetActive(true);
        GameOver.Play();
        EndGameText.text = "GAME OVER\n TIME :"+tm.EndTime();

    }
    private void Win()
    {
        controller.enabled = false;
        EndgameCanvas.SetActive(true);
        WinSound.Play();
        EndGameText.text = "YOU WON\n TIME :" + tm.EndTime();
        PlayerPrefs.SetInt(string.Concat("Medal", ButtonBehavior.CharacterSelection), 1);
        PlayerPrefs.Save();
    }
}
                                      