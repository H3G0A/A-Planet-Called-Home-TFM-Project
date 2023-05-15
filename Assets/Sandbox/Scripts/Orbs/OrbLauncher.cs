using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static GlobalParameters;

public class OrbLauncher : MonoBehaviour
{
    [SerializeField] private GameObject _selectedOrb;
    [SerializeField] private float _force = 1000;
    [SerializeField] private Transform _firePoint;
    [SerializeField] Camera _mainCamera;
    [SerializeField] private List<GameObject> _chargedOrbs;
    [SerializeField] TMP_Text _orbText;
    [SerializeField] TMP_Text _orbWeigthText;
    [SerializeField] bool _augmentWeigthOrb;
    private Quaternion _initialRotation;
    private Quaternion _aimRotation;
    
    //Player input
    private PlayerInput _playerInput;

    //Input actions
    private InputAction _shootAction;
    private InputAction _aimAction;
    private InputAction _changeOrb;
    private InputAction _changeOrbWeigth;
    
    private int _indexOrb;

    void Awake()
    {
        _playerInput = transform.parent.GetComponentInParent<PlayerInput>();
        _shootAction = _playerInput.actions[SHOOT_ACTION];
        _changeOrb = _playerInput.actions[CHANGEORB_ACTION];
        _aimAction = _playerInput.actions[AIM_ACTION];
        _changeOrbWeigth = _playerInput.actions[CHANGEORBWEIGTH_ACTION];

        _initialRotation = transform.localRotation;
        _selectedOrb = _chargedOrbs[0];
        SetInputCallbacks();
        Aim(); //Provisionally aim midscreen
    }

    void Start()
    {
        _selectedOrb = _chargedOrbs[0];
        _indexOrb = 0;
        changeOrbText();
        changeAugmentText();
    }

    private void Aim()
    {
        transform.LookAt(_mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 50)));
        transform.Rotate(90, 0, 0);

    }

    private void Rest()
    {
        transform.localRotation = _initialRotation;
    }

    private void ShootOrb()
    {
        // Instantiate and give initial speed boost
        GameObject _orbInstance = GameObject.Instantiate(_selectedOrb, _firePoint.position, _firePoint.rotation);
        if(_indexOrb == 1){
            _orbInstance.GetComponent<WeigthOrb>().changeAugment(_augmentWeigthOrb);
        } 
        Vector3 _forceVector = _firePoint.forward * _force;
        Rigidbody _rb = _orbInstance.GetComponent<Rigidbody>();
        _rb.AddForce(_forceVector, ForceMode.Impulse);
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
        // AIMING
        _aimAction.performed += ctx => Aim();
        //_aimAction.canceled += ctx => Rest();

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
