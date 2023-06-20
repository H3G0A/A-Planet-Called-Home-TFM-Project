using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactReceiver : MonoBehaviour
{
    [Header("Mass")]
    [SerializeField] float _lightMass = .2f;
    [SerializeField] float _regularMass = .8f;
    [SerializeField] float _heavyMass = 1000f;
    [SerializeField] float _currentMass;

    [Header("Drag")]
    [SerializeField] float _groundDrag = 5;
    [SerializeField] float _airDrag = 1;
    [SerializeField] float _iceDrag = 1;

    Vector3 _impact = Vector3.zero;
    float _drag;
    bool _leftGround = false;

    [SerializeField] GameObject _characterMassIndicator;
    FirstPersonController _firstPersonController;

    private void Awake()
    {
        _firstPersonController = this.GetComponent<FirstPersonController>();
    }

    private void Update()
    {
        ManageForces();
        ManageMassIndicator();
    }
    
    public void AddImpact(Vector3 dir, float force) //Simulate Rigidbody.AddForce()
    {
        dir.Normalize();
        _impact += dir * force / _currentMass;
    }

    public void OverrideForce(Vector3 dir, float force)
    {
        dir.Normalize();
        _impact = dir * force / _currentMass;
    }

    private void ManageForces()
    {
        if (_impact.magnitude > 0.2)
        {
            SetDrag();
            PreventBouncing();

            _firstPersonController.MoveCharacter(_impact * Time.deltaTime);

            _impact = Vector3.Lerp(_impact, Vector3.zero, Time.deltaTime * _drag);
        }
        else
        {
            _impact = Vector3.zero;
        }
    }

    private void SetDrag()
    {
        if(_firstPersonController.OnIce)
        {
            _drag = _iceDrag;
        }
        else if (_firstPersonController.Grounded)
        {
            _drag = _groundDrag;
        }
        else
        {
            _drag = _airDrag;
        }
    }

    private void PreventBouncing()
    {
        if (!_firstPersonController.Grounded) _leftGround = true;

        bool _backOnGround = (_firstPersonController.Grounded || _firstPersonController.InWater) && _leftGround && _firstPersonController.CumulatedMovement.y <= 0;
        if (_backOnGround)
        {
            _impact.y = 0;
            _leftGround = false;
        }
    }

    ///<summary>
    /// 0 = Regular Weight;
    /// 1 = Heavy Weight;
    /// -1 = Light Weight;
    /// </summary>
    public void ChangeMass(int _playerWeight)
    {
        Debug.Log("ChangeMass " + _playerWeight);
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
    
    private void ManageMassIndicator()
    {
        if(_currentMass < _regularMass)
        {
            _characterMassIndicator.transform.position = Vector3.MoveTowards(_characterMassIndicator.transform.position, new Vector3(0.0f,0.25f,0.0f), 1f * Time.deltaTime);
        } else 
        {
            if(_currentMass > _regularMass)
            {
                _characterMassIndicator.transform.position = Vector3.MoveTowards(_characterMassIndicator.transform.position, new Vector3(0.0f,-0.25f,0.0f), 1f * Time.deltaTime);
            }
            else 
            {
                _characterMassIndicator.transform.position = Vector3.MoveTowards(_characterMassIndicator.transform.position, new Vector3(0.0f,0.0f,0.0f), 1f * Time.deltaTime);
            }
        }
    }
}
