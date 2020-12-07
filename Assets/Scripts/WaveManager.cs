﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    private int WaveID = 1;
    private int LastWave = 1;
    private int CurrentWave = 1;
    [SerializeField]
    private GameObject EndgameCanvas;
    [SerializeField]
    private PlayerController controller;
    [SerializeField]
    private Text EndGameText;
    [SerializeField]
    private Text WaveNumber;
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
        if (WaveID == 11)
        {
            Win();
        }
        if (CheckEnemyCount() == 0)
        {
            WaveNumber.text = "Wave " + WaveID;
            StartCoroutine("Spawn");
        }
    }
    private void NewWave()
    {
        int NewWave = CurrentWave + LastWave;
        LastWave = CurrentWave;
        CurrentWave = NewWave;
        Debug.Log(""+LastWave);
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
        Debug.Log(""+PlayerPrefs.GetInt(string.Concat("WaveSaved", ButtonBehavior.CharacterSelection)));
    }
    private int CheckEnemyCount()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;

    }
    private void Lose()
    {
        controller.enabled = false;
        EndgameCanvas.SetActive(true);
        EndGameText.text = "GAME OVER";

    }
    private void Win()
    {
        controller.enabled = false;
        EndgameCanvas.SetActive(true);
        EndGameText.text = "YOU WON";
        PlayerPrefs.SetInt(string.Concat("Medal", ButtonBehavior.CharacterSelection), 1);
        PlayerPrefs.Save();
    }
}
                                      