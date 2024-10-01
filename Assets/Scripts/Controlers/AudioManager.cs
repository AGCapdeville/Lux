using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public int MaxSFX = 3;
    private int _CurrentSFX = 0;


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("PianoFade");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Music Sound: " + name + " not found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySound(string name)
    {
        if (_CurrentSFX >= MaxSFX)
        {
            Debug.Log("Max sound effects limit reached.");
            return;
        }
        
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("SFX Sound: " + name + " not found");
        }
        else
        {
            // sfxSource.PlayOneShot(s.clip, 0.1f);
            sfxSource.PlayOneShot(s.clip, 0.1f);
            _CurrentSFX++;
            StartCoroutine(WaitForSoundToFinish(s.clip.length));
        }
    }

    private IEnumerator WaitForSoundToFinish(float duration)
    {
        yield return new WaitForSeconds(duration);
        _CurrentSFX--;
    }

}
