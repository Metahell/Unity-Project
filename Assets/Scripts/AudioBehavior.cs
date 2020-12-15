using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioBehavior : MonoBehaviour
{
    [SerializeField]
    private AudioMixer Mastermixer;
    public string parameterName = "MasterVolume";
    [SerializeField]
    private Slider MasterSlider;
    [SerializeField]
    private Slider SoundSlider;
    [SerializeField]
    private Slider MusicSlider;
    private float musicvalue;
    private float soundvalue;
    private float mastervalue;

    private void Start()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("Music",0.75f);
        MasterSlider.value = PlayerPrefs.GetFloat("Master", 0.75f);
        SoundSlider.value = PlayerPrefs.GetFloat("Sound", 0.75f);
    }
    public void SetMasterLevel()
    {
        mastervalue = MasterSlider.value;
        Mastermixer.SetFloat("Master", Mathf.Log10(mastervalue) * 20);
        PlayerPrefs.SetFloat("Master", mastervalue);
    }
    public void SetMusicLevel()
    {
        musicvalue = MusicSlider.value;
        Mastermixer.SetFloat("Music", Mathf.Log10(musicvalue) * 20);
        PlayerPrefs.SetFloat("Music",musicvalue);
    }
    public void SetSoundLevel()
    {
        soundvalue= SoundSlider.value;
        Mastermixer.SetFloat("Sound", Mathf.Log10(soundvalue) * 20);
        PlayerPrefs.SetFloat("Sound", soundvalue);
    }
}
