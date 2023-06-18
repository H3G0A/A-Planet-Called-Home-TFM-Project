using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource SFXAudioSource;
    [SerializeField] AudioSource MusicAudioSource;

    [SerializeField] List<AudioClip> SFXClips;
    [SerializeField] List<AudioClip> MusicSources;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        SFXAudioSource.PlayOneShot(sfxClip);
    }

    public void ChangeMusic(AudioClip musicClip)
    {
        MusicAudioSource.Stop();
        MusicAudioSource.clip = musicClip;
        MusicAudioSource.Play();
    }
}