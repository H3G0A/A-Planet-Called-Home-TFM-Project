using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactReceiver : MonoBehaviour
{
    [SerializeField] float mass = 1;

    Vector3 _impact = Vector3.zero;
    FirstPersonController _firstPersonController;

    private void Start()
    {
        //_charController = this.GetComponent<CharacterController>();
        _firstPersonController = this.GetComponent<FirstPersonController>();
    }

    private void Update()
    {
        ManageForces();
    }
    
    public void AddImpact(Vector3 dir, float force) //Simulate Rigidbody.AddForce()
    {
        dir.Normalize();
        _impact += dir * force / mass;
    }

    private void ManageForces()
    {
        if (_impact.magnitude > 0.2)
        {
            _firstPersonController.MoveCharacter(_impact * Time.deltaTime);
            _impact = Vector3.Lerp(_impact, Vector3.zero, 5 * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = collision.GetContact(0).normal;

        Debug.Log(normal);

        if(normal == -transform.up)
        {
            _impact.y = 0;
        }
    }
}
