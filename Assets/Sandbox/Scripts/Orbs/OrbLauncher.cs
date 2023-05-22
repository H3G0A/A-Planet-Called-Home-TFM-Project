using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static GlobalParameters;

public class OrbLauncher : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float _force = 1000;
    [SerializeField] float _range = 100;
    [SerializeField] float _fireRate = .5f;

    
    [Header("Orbs")]
    [SerializeField] GameObject _selectedOrb;
    [SerializeField] List<GameObject> _chargedOrbs;
    
    [Space(10)]
    [SerializeField] TMP_Text _orbText;
    [SerializeField] TMP_Text _orbWeigthText;
    [SerializeField] bool _augmentWeigthOrb;

    [Header("Others")]
    [SerializeField] Transform _firePoint;
    [SerializeField] Camera _mainCamera;
    
    //Player input
    PlayerInput _playerInput;

    //Input actions
    InputAction _shootAction;
    InputAction _changeOrb;
    InputAction _changeOrbWeigth;
    
    // Launcher
    private int _indexOrb;
    
    // Timers
    private float _fireRateDelta = 0;

    void Awake()
    {
        _playerInput = transform.parent.GetComponentInParent<PlayerInput>();
        _shootAction = _playerInput.actions[SHOOT_ACTION];
        _changeOrb = _playerInput.actions[CHANGEORB_ACTION];
        _changeOrbWeigth = _playerInput.actions[CHANGEORBWEIGTH_ACTION];

        _selectedOrb = _chargedOrbs[0];
        SetInputCallbacks();
        
        // Make launcher point at the middle of the screen
        transform.LookAt(_mainCamera.ScreenToWorldPoint(new(Screen.width / 2, Screen.height / 2, 100)));
        transform.Rotate(90, 0, 0); // This rotation is only for the launcher's placeholder cylinder
    }

    void Start()
    {
        _selectedOrb = _chargedOrbs[0];
        _indexOrb = 0;
        changeOrbText();
        changeAugmentText();
    }

    void Update()
    {
        ManageCooldown();
    }

    private void ShootOrb()
    {
        // By default nothing has been hit, so simulate a far point
        Vector3 _forceDirection = _mainCamera.ScreenToWorldPoint(new(Screen.width / 2, Screen.height / 2, 100)) - _firePoint.transform.position;

        // Raycast from camera to get impact point and shoot accordingly
        bool _inRangeCollision = Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out RaycastHit _hit, _range);
        
        // If something was shoot the orb in it's direction
        if (_inRangeCollision)
        {
            _forceDirection = _hit.point - _firePoint.transform.position;
        }

        // If not on cooldown, shoot
        if(_fireRateDelta <= 0) InstantiateOrb(_forceDirection);
    }

    void InstantiateOrb(Vector3 _forceDirection)
    {
        // Instantiate and give initial speed boost
        GameObject _orbInstance = GameObject.Instantiate(_selectedOrb, _firePoint.position, _firePoint.rotation);
        if (_indexOrb == 1)
        {
            _orbInstance.GetComponent<WeigthOrb>().changeAugment(_augmentWeigthOrb);
        }
        Vector3 _forceVector = _forceDirection.normalized * _force;
        Rigidbody _rb = _orbInstance.GetComponent<Rigidbody>();
        _rb.AddForce(_forceVector, ForceMode.Impulse);

        // Reset cooldown
        _fireRateDelta = _fireRate;
    }

    void ManageCooldown()
    {
        if (_fireRateDelta > 0) _fireRateDelta -= Time.deltaTime;
    }

    private void ChangeOrb(){
        if(Keyboard.current.digit1Key.wasPressedThisFrame){
            _selectedOrb = _chargedOrbs[0];
            _indexOrb = 0;
            Debug.Log(_selectedOrb.ToString());
        }
        if(Keyboard.current.digit2Key.wasPressedThisFrame){
            _selectedOrb = _chargedOrbs[1];
            _indexOrb = 1;
            Debug.Log(_selectedOrb.ToString());
        }
        if(Keyboard.current.digit3Key.wasPressedThisFrame){
            _selectedOrb = _chargedOrbs[2];
            _indexOrb = 2;
            Debug.Log(_selectedOrb.ToString());
        }
        if(Keyboard.current.qKey.wasPressedThisFrame){
            _indexOrb--;
            if(_indexOrb < 0){
                _indexOrb = (_chargedOrbs.Count - 1);
            }
            _selectedOrb = _chargedOrbs[_indexOrb];
            Debug.Log(_selectedOrb.ToString());
        }
        if(Keyboard.current.eKey.wasPressedThisFrame){
            _indexOrb++;
            if(_indexOrb >= _chargedOrbs.Count){
                _indexOrb = 0;
            }
            _selectedOrb = _chargedOrbs[_indexOrb];
            Debug.Log(_selectedOrb.ToString());
        }
        changeOrbText();
    }

    private void SetInputCallbacks()
    {
        //SHOOTING
        _shootAction.performed += ctx => ShootOrb();

        //Change weapons.
        _changeOrb.performed += ctx => ChangeOrb();

        // Change gravity of weigth orb.
        _changeOrbWeigth.performed += ctx => ChangeOrbWeigth();
    }

    private void ChangeOrbWeigth(){
        Debug.Log("Cambio de peso");
        _augmentWeigthOrb = !_augmentWeigthOrb;
        changeAugmentText();
    }

    private void changeOrbText(){
        switch(_indexOrb){
            case 0:
                _orbText.text = "Orbe de dispersión";
                break;
            case 1:
                _orbText.text = "Orbe de gravedad";
                break;
            case 2:
                _orbText.text = "Orbe de hielo";
                break;
        }
    }

    private void changeAugmentText(){
        if(_augmentWeigthOrb){
            _orbWeigthText.text = "Aumento de peso";
        }else{
            _orbWeigthText.text = "Reducción de peso";
        }
    }
}
