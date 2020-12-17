using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject SpawnPoint;
    [SerializeField]
    List<GameObject> Characters = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        SpawnSelection(ButtonBehavior.CharacterSelection);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnSelection(int i)
    {
        GameObject target;
        switch (i)
        {
            case 1: target = Characters[1]; break;
            case 2: target = Characters[2]; break;
            case 3: target = Characters[3]; break;
            case 4: target = Characters[4]; break;
            default: target = Characters[0]; break;

        }
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            GameObject res = Instantiate(target, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
        }
        else
        {
            target.SetActive(true);
            target.transform.position = SpawnPoint.transform.position;
        }
    }
}
