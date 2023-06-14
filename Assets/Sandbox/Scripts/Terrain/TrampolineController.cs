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
            _playerController.StopVerticalMovement();
            _impactReceiver.OverrideForce(transform.up, _force);
        }
    }
}
