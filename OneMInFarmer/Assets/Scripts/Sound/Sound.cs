using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [SerializeField]private string _soundName;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private bool _loop;
    [Range(0f,1f)]
    public float volume;

    public AudioClip GetAudioClip()
    {
        return _clip;
    }
    public string GetSoundName()
    {
        return _soundName;
    }

    public AudioSource audioSource { get; set; }

}
