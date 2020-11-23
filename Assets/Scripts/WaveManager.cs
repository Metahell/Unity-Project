using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private int LastWave = 0;
    private int CurrentWave = 1;
    [SerializeField]
    private List<GameObject> SpawnPoints = new List<GameObject>();
    [SerializeField]
    private List<GameObject> EnnemyPool= new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void NewWave()
    {
        int NewWave = CurrentWave + LastWave;
        LastWave = CurrentWave;
        CurrentWave = NewWave;
    }
    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(1);
        for(int i = 0; i < CurrentWave; i++)
        {
            Debug.Log("CurrentWave :" + CurrentWave);
            GameObject mob=Instantiate(EnnemyPool[(i%EnnemyPool.Count)]);
            mob.transform.position = SpawnPoints[i % SpawnPoints.Count].transform.position;

            yield return new WaitForSeconds(1);
        }
    }
}
                                      