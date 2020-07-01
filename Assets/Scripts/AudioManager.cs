﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    bool playing = false;


    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }


    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            // Debug.LogWarning("Sound not found: " + name);
            return;
        }
        else
        {
            // Debug.Log("Toistetaan " + name);
        }

        s.source.Play();
    }


    public void KeepPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found: " + name);
            return;
        }
        else
            Debug.Log("Toistetaan " + name);

        // Start playingin sound
        if (!playing)
        {
            s.source.Play();
            playing = true;
        }

    }

}
