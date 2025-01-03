using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Config
{
    [Header("Audio Settings")]
    public int DefaultAudioSourcePoolSize;
    public AudioMixerGroup MusicMixerGroup;
    public AudioMixerGroup SFXMixerGroup;
}