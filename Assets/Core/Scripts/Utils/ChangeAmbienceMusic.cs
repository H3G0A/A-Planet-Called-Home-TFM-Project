using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAmbienceMusic : MonoBehaviour
{
    [SerializeField] AudioClip _musicClip;
    [SerializeField] AudioSource _ambienceMusicAudioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GlobalParameters.PLAYER_TAG) && _ambienceMusicAudioSource.clip != _musicClip)
        {
            _ambienceMusicAudioSource.clip = _musicClip;
            _ambienceMusicAudioSource.Play();
        }
    }
}
