using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispersionOrb : OrbBehaviour
{
    [SerializeField] float _force;
    [SerializeField] float _radius;

    //The orb pushes all objects inside radius away
    protected override void ApplyEffect()
    {
        Collider[] _colliders = Physics.OverlapSphere(transform.position, _radius);
        
        foreach(Collider col in _colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if(rb != null)
            {
                Debug.Log("box: " + col.transform.position);
                Debug.Log("orb: " + this.transform.position);
                Vector3 _forceDirection = (col.transform.position - this.transform.position).normalized;
                Debug.Log("direction" + _forceDirection);
                rb.AddForce(_forceDirection * _force, ForceMode.Impulse);
                //rb.AddExplosionForce(_force, transform.position, _radius);
            }
        }
    }
}
