////////////////////////////////////////////////////////////
// File: SoundManager.cs
// Author: Charles Carter
// Date Created: 17/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 17/05/22
// Brief: A script to play sounds when needed
//////////////////////////////////////////////////////////// 

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//A class to hold necessary scene sounds
[System.Serializable]
public class SoundDetails
{
    public AudioClip clip;
    public bool looping;
    public float volume = 1f;
}

public class SoundManager : MonoBehaviour
{
    #region Variables

    public static SoundManager instance;

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioSource sfxSource;

    //This happens throughout all the UI so it's easier and is more effective to save it once
    [SerializeField]
    private AudioClip buttonClick;

    [SerializeField]
    private List<SoundDetails> sceneSoundsToPlay = new List<SoundDetails>();

    [SerializeField]
    private AudioMixerGroup masterMixer;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if(instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        //Going through all of the predetermined sounds to play at the start
        foreach(SoundDetails sound in sceneSoundsToPlay)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = masterMixer;
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.loop = sound.looping;
            source.Play();
        }
    }

    #endregion

    #region Public Methods

    public void PlaySFX(AudioClip sound)
    {
        sfxSource.clip = sound;
        sfxSource.Play();
    }

    public void PlaySFX(AudioClip sound, float volume)
    {
        sfxSource.PlayOneShot(sound, volume);
    }

    public void PlayButtonClick()
    {
        sfxSource.clip = buttonClick;
        sfxSource.pitch = (Random.Range(0.1f, 0.9f));
        sfxSource.Play();
        sfxSource.pitch = 0.5f;
    }

    public void PlayMusic(AudioClip track)
    {
        musicSource.clip = track;
        musicSource.Play();
    }

    #endregion
}
