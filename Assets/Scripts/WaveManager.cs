﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private int WaveID = 1;
    private int LastWave = 0;
    private int CurrentWave = 1;
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
            StartCoroutine("Spawn");
        }
        if (WaveID == 11)
        {
            Win();
        }
    }
    private void NewWave()
    {
        int NewWave = CurrentWave + LastWave;
        LastWave = CurrentWave;
        CurrentWave = NewWave;
        Debug.Log(""+CurrentWave);
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
    }
    private int CheckEnemyCount()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;

    }
    private void Win()
    {
        //Affiche texte, renvoie au menu, update stats de l'écran de sélection des personnages
    }
}
                                      