using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispersionOrb : OrbBehaviour
{
    [SerializeField] float _force;
    [SerializeField] float _radius;
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

                rb.AddForce(_forceDirection * _force, ForceMode.Impulse); //VelocityChange disregards the object mass so it does not care that its mas is high
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

        //Push only in the direction of the strongest axis
        float _YAxis = Mathf.Abs(_forceDirection.y);
        float _XAxis = Mathf.Abs(_forceDirection.x);
        float _ZAxis = Mathf.Abs(_forceDirection.z);

        if ((_YAxis > _XAxis) && (_YAxis > _ZAxis)) //If "y" is the strongest axis, then don't move the object
        {
            _forceDirection.x = 0;
            _forceDirection.z = 0;
        }
        //else if (_XAxis > _ZAxis) 
        //{
        //    _forceDirection.z = 0;
        //}
        //else
        //{
        //    _forceDirection.x = 0;
        //}

        _forceDirection.y = 0; //Disable vertical push
        
        return _forceDirection.normalized;
    }
}
