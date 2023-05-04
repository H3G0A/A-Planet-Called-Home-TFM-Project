using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispersionOrb : OrbBehaviour
{
    [SerializeField] float _force;
    [SerializeField] float _radius;
    [SerializeField] bool _diagonalPush = true;
    [SerializeField] bool _verticalPush = false;
    ImpactReceiver _impactReceiver;

    protected override void ApplyEffect(Collision collision) //The orb pushes all objects inside radius away
    {
        Collider[] _colliders = Physics.OverlapSphere(transform.position, _radius); //Store every collider in range
        
        foreach(Collider col in _colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if(rb != null) //Range hits RigidBody
            {
                Vector3 _forceDirection = CalculateForceDirection(this.transform.position, col.transform.position);

                if (_forceDirection.y > 0)
                {
                    _forceDirection.y *= 3;

                }

                rb.AddForce(_forceDirection * _force, ForceMode.Impulse);
            }
            else if (col.gameObject.CompareTag(GlobalParameters.PLAYER_TAG)) //Range hits player
            {
                CharacterController _charController = col.GetComponent<CharacterController>();
                
                Vector3 _forceDirection = (col.transform.TransformPoint(_charController.center) - this.transform.position);

                if (!_impactReceiver)
                {
                    _impactReceiver = col.GetComponent<ImpactReceiver>();
                }

                _impactReceiver.AddImpact(_forceDirection, _force);
            }
        }
    }

    private Vector3 CalculateForceDirection(Vector3 explosionCenter, Vector3 objectPosition) //Direction when pushing a non-player object
    {
        Vector3 _forceDirection = objectPosition - explosionCenter;


        if (!_diagonalPush)
        {
            //Push only in the direction of the strongest axis
            float _XAxis = Mathf.Abs(_forceDirection.x);
            float _ZAxis = Mathf.Abs(_forceDirection.z);
            
            if (_XAxis > _ZAxis)
            {
                _forceDirection.z = 0;
            }
            else
            {
                _forceDirection.x = 0;
            }
        }

        if (!_verticalPush) // Disable vertical movement
        {
            _forceDirection.y = 0; 
        }
        
        return _forceDirection.normalized;
    }
}
