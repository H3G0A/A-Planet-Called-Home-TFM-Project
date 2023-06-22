using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class DeathZone : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioClip _deathFallSound;

    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            _audioSource.PlayOneShot(_deathFallSound);
            other.GetComponent<FirstPersonController>().PlayerRespawn();
        }
    }
}
