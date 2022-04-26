using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsController : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    public static SoundEffectsController Instance;

    private void Awake()
    {
        Instance = this;
        foreach(Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.GetAudioClip();
            s.audioSource.volume = s.volume;
        }
    }

    public void PlaySoundEffect(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.GetSoundName() == name);
        if(s == null)
        {
            Debug.LogWarning("Not have sound : " + name);
            return;
        }
        if (!s.audioSource.isPlaying)
            s.audioSource.Play();
    }
    public void StopSoundEffect(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.GetSoundName() == name);
        if (s == null)
        {
            Debug.LogWarning("Not have sound : " + name);
            return;
        }

        s.audioSource.Stop();
    }
}
