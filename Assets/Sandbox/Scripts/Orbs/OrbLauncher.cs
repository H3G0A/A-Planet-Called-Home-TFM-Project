using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OrbLauncher : MonoBehaviour
{
    [SerializeField] private GameObject _orb;
    [SerializeField] private float _force = 1000;
    [SerializeField] private Transform _firePoint;
    [SerializeField] Camera _mainCamera;
    private Quaternion _initialLauncherRotation;
    void Awake()
    {
        _initialLauncherRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnShoot(InputValue input)
    {
        if (input.isPressed) 
        {
            Aim();
        }
        else
        {
            ShootOrb();
        }
    }

    private void Aim()
    {
        transform.LookAt(_mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 50)));
        transform.Rotate(90, 0, 0);
    }
    private void ShootOrb()
    {
        GameObject _orbInstance = GameObject.Instantiate(_orb, _firePoint.position, Quaternion.identity);

        Vector3 _forceVector = _firePoint.forward * _force;
        _orbInstance.GetComponent<Rigidbody>().AddForce(_forceVector);

        ResetLauncher();
    }

    private void ResetLauncher()
    {
        
    }
}
