using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class TrampolineController : MonoBehaviour
{
    [SerializeField] float _force;
    [SerializeField] float _radius;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            FirstPersonController _FPController = other.GetComponent<FirstPersonController>();
            Vector3 _forceDirection = (new Vector3(0f,1f,0f));
            ImpactReceiver _impactReceiver = other.GetComponent<ImpactReceiver>();
            _impactReceiver.AddImpact(_forceDirection, _force);
            Debug.Log("Colision con jugador");
        }
    }
}
