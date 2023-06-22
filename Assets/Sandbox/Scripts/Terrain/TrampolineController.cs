using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class TrampolineController : MonoBehaviour
{
    [Header("Impact")]
    [SerializeField] float _force;

    ImpactReceiver _impactReceiver;
    FirstPersonController _playerController;
    AudioSource _audioSource;


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag(PLAYER_TAG))
        {
            if (_impactReceiver == null) _impactReceiver = other.GetComponent<ImpactReceiver>();
            if (_playerController == null) _playerController = other.GetComponent<FirstPersonController>();

            ApplyForce();
        }
    }

    private void ApplyForce()
    {
        if (_playerController.PlayerWeight != 1)
        {
            _audioSource.Play();

            _playerController.StopVerticalMovement();
            _impactReceiver.OverrideForce(Vector3.up, _force);
        }
    }
}
