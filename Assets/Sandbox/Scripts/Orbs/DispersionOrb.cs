using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispersionOrb : OrbBehaviour
{
    [SerializeField] float _force;
    [SerializeField] float _radius;

    protected override void ApplyEffect() //The orb pushes all objects inside radius away
    {
        Collider[] _colliders = Physics.OverlapSphere(transform.position, _radius); //Store every collider in range
        
        foreach(Collider col in _colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if(rb != null) //Range hits RigidBody
            {
                Vector3 _forceDirection = CalculateForceDirection(this.transform.position, col.transform.position);

                Debug.Log("box: " + col.transform.position);
                Debug.Log("orb: " + this.transform.position);
                Debug.Log("direction" + _forceDirection);

                rb.AddForce(_forceDirection * _force, ForceMode.VelocityChange); //VelocityChange disregards the object mass so it does not care that its mas is high
            }
            else if (col.gameObject.CompareTag(GlobalParameters.PLAYER_TAG)) //Range hits player
            {
                CharacterController _charController = col.GetComponent<CharacterController>();
                
                Vector3 _forceDirection = (this.transform.position - col.transform.TransformPoint(_charController.center));

                col.GetComponent<ImpactReceiver>().AddImpact(_forceDirection, _force);

                Debug.Log("direction" + _forceDirection);
                //Debug.Log(col.transform.TransformPoint(_charController.center));
                //Debug.Log("direction" + _forceDirection);

                //_charController.Move(_forceDirection * _force);
            }
        }
    }

    private Vector3 CalculateForceDirection(Vector3 explosionCenter, Vector3 objectPosition) 
    {
        Vector3 _forceDirection = objectPosition - explosionCenter;
        
        _forceDirection.y = 0; //Disable vertical push

        if(Mathf.Abs(_forceDirection.x) > Mathf.Abs(_forceDirection.z)) //Push only the greatest of the axis
        {
            _forceDirection.z = 0;
        }
        else
        {
            _forceDirection.x = 0;
        }

        return _forceDirection.normalized;
    }
}
