using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GlobalParameters;

public class OrbLauncher : MonoBehaviour
{
    [SerializeField] private GameObject _orb;
    [SerializeField] private float _force = 1000;
    [SerializeField] private Transform _firePoint;
    [SerializeField] Camera _mainCamera;
    private Quaternion _initialLauncherRotation;
    private PlayerInput _playerInput;
    private InputAction _shootAction;


    //////////////////////////////////////////////////////////////////////////////////////////////////////

    void Awake()
    {
        _initialLauncherRotation = transform.rotation;
        _playerInput = transform.parent.GetComponentInParent<PlayerInput>();
        _shootAction = _playerInput.actions[SHOOT_ACTION];

        SetInputCallbacks();
    }

    private void Aim()
    {
        transform.LookAt(_mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 50)));
        transform.Rotate(90, 0, 0);

    }
    private void ShootOrb()
    {
        GameObject _orbInstance = GameObject.Instantiate(_orb, _firePoint.position, _firePoint.rotation);

        Vector3 _forceVector = _firePoint.forward * _force;
        Rigidbody _rb = _orbInstance.GetComponent<Rigidbody>();
        _rb.AddForce(_forceVector, ForceMode.Impulse);

        ResetLauncher();
    }

    private void ResetLauncher()
    {
        
    }
    private void SetInputCallbacks()
    {
        _shootAction.started += context => Aim();
        _shootAction.canceled += context => ShootOrb();
    }
}
