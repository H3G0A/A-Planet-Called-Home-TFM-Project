using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactReceiver : MonoBehaviour
{
    [SerializeField] float _lightMass = .2f;
    [SerializeField] float _regularMass = .8f;
    [SerializeField] float _heavyMass = 1000f;

    [SerializeField] float _currentMass;
    Vector3 _impact = Vector3.zero;
    FirstPersonController _firstPersonController;

    private void Awake()
    {
        _firstPersonController = this.GetComponent<FirstPersonController>();
    }

    private void Update()
    {
        ManageForces();
    }
    
    public void AddImpact(Vector3 dir, float force) //Simulate Rigidbody.AddForce()
    {
        dir.Normalize();
        _impact += dir * force / _currentMass;
    }

    private void ManageForces()
    {
        if (_impact.magnitude > 0.2)
        {
            _firstPersonController.MoveCharacter(_impact * Time.deltaTime);
            _impact = Vector3.Lerp(_impact, Vector3.zero, 5 * Time.deltaTime);
        }
    }

    ///<summary>
    /// 0 = Regular Weight;
    /// 1 = Heavy Weight;
    /// -1 = Light Weight;
    /// </summary>
    public void ChangeMass(int _playerWeight)
    {
        switch (_playerWeight)
        {
            case 0:
                _currentMass = _regularMass;
                break;

            case 1:
                _currentMass = _heavyMass;
                break;

            case -1:
                _currentMass = _lightMass;
                break;
        }
    }
}
