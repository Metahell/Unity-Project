using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMusicPlayer : MonoBehaviour
{
    [SerializeField]
    private List<AudioSource> Song = new List<AudioSource>();
    [SerializeField]
    private List<AudioSource> GameSong = new List<AudioSource>();
    private int SongNum;
    private int GameSongNum;
    private static GameMusicPlayer instance;
    public static GameMusicPlayer Instance
    {
        get { return instance; }
    }
    private void Start()
    {
        SongNum = Random.Range(0, Song.Capacity);
        GameSongNum = Random.Range(0, Song.Capacity);
        Song[SongNum].Play();
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            if (GameSong[GameSongNum].isPlaying)
            {
                GameSong[GameSongNum].Stop();
            }
            if (!Song[SongNum].isPlaying || Input.GetKeyDown(KeyCode.Space))
            {
                Song[SongNum].Stop();
                SongNum = (SongNum + 1 >= Song.Capacity) ? 0 : SongNum + 1;
                Debug.Log(SongNum);
                Debug.Log(Song.Capacity);
                Song[SongNum].Play();
            }
        }
        if (SceneManager.GetActiveScene().name == "Map")
        {
            if (Song[SongNum].isPlaying)
            {
                Song[SongNum].Stop();
            }
            if (!GameSong[GameSongNum].isPlaying)
            {
                GameSongNum = (GameSongNum + 1 >= GameSong.Capacity) ? 0 : GameSongNum + 1;
                Debug.Log(GameSongNum);
                Debug.Log(GameSong.Capacity);
                GameSong[GameSongNum].Play();
            }
        }
    }
}
