using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    
    public void SetMasterLevel(float level)
    {
        audioMixer.SetFloat("MasterVolume", level);
    }

    public void SetSFXLevel(float level)
    {
        audioMixer.SetFloat("SFXVolume", level);
    }

    public void SetMusicLevel(float level)
    {
        audioMixer.SetFloat("MusicVolume", level);
    }
}
