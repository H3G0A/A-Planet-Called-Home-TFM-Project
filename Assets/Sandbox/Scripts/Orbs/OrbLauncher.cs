using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static GlobalParameters;

public class OrbLauncher : MonoBehaviour
{
    public bool IsEnabled = true;

    [Header("Stats")]
    [SerializeField] float _force = .035f;
    [SerializeField] float _range = 100;
    [SerializeField] float _fireRate = .5f;

    
    [Header("Orbs")]
    [SerializeField] GameObject _selectedOrb;
    [SerializeField] List<GameObject> _chargedOrbs;
    
    [Space(10)]
    [SerializeField] TMP_Text _orbText;
    [SerializeField] TMP_Text _orbWeigthText;
    [SerializeField] bool _augmentWeigthOrb;


    [Header("Aiming")]
    [SerializeField] Transform _firePoint;
    [SerializeField] Camera _mainCamera;
    [SerializeField] LayerMask _layersToAim;


    [Header("Player")]
    [SerializeField] FirstPersonController _FPController;

    // Launcher
    private int _indexOrb;
    private int _indexLastOrb;

    [Header("OrbRotation")]
    //CylinderOrb
    [SerializeField] GameObject _cilinderOrb;
    [SerializeField] float _rotationSpeed;

    [Header("OrbsCapsules")]
    [SerializeField] GameObject _firstOrbCapsule;
    [SerializeField] GameObject _secondOrbCapsule;
    [SerializeField] GameObject _thirdOrbCapsule;
    // Timers
    private float _fireRateDelta = 0;


    void Awake()
    {        
        // Make launcher point at the middle of the screen
        transform.LookAt(_mainCamera.ScreenToWorldPoint(new(Screen.width / 2, Screen.height / 2, 100)));
        transform.Rotate(90, 0, 0); // This rotation is only for the launcher's placeholder cylinder
    }

    void Start()
    {
        LoadOrbs();

        //Set reference in GameManager
        GameManager.Instance.OrbLauncher_ = this;

        
        _selectedOrb = _chargedOrbs[0];
        _indexOrb = 0;
        ChangeOrbText();
        ChangeOrbRotation();
        ChangeAugmentText();
    }

    void Update()
    {
        ManageCooldown();
        visualizeChargedOrbs();
    }

    public void LoadOrbs()
    {
        _chargedOrbs = new List<GameObject>();

        foreach(GameManager.Orb orb in GameManager.Instance.OrbStash)
        {
            if (orb.Available)
            {
                _chargedOrbs.Add(orb.Prefab);

                if (orb.Prefab.GetComponent<WeigthOrb>() != null)
                {
                    _FPController._canChangeWeight = true;
                }
            }
        }
    }

    public void visualizeChargedOrbs(){
        if(_chargedOrbs.Count > 0)
        {
            _firstOrbCapsule.SetActive(true);
        } else
        {
            _firstOrbCapsule.SetActive(false);
        }
        if(_chargedOrbs.Count > 1)
        {
             _secondOrbCapsule.SetActive(true);
        } else
        {
            _secondOrbCapsule.SetActive(false);
        }
        if(_chargedOrbs.Count > 2)
        {
             _thirdOrbCapsule.SetActive(true);
        } else
        {
            _thirdOrbCapsule.SetActive(false);
        }
    }

    public void ShootOrb(InputAction.CallbackContext ctx)
    {
        if (!IsEnabled) return;

        // By default nothing has been hit, so simulate a far point
        Vector3 _forceDirection = _mainCamera.ScreenToWorldPoint(new(Screen.width / 2, Screen.height / 2, 100)) - _firePoint.transform.position;

        // Raycast from camera to get impact point and shoot accordingly
        bool _inRangeCollision = Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out RaycastHit _hit, _range, _layersToAim, QueryTriggerInteraction.Ignore);

        // If something was shoot the orb in it's direction
        if (_inRangeCollision)
        {
            _forceDirection = _hit.point - _firePoint.transform.position;
        }

        // If not on cooldown, shoot
        if (_fireRateDelta <= 0) InstantiateOrb(_forceDirection);
    }

    void InstantiateOrb(Vector3 _forceDirection)
    {
        // Instantiate and give initial speed boost
        GameObject _orbInstance = GameObject.Instantiate(_selectedOrb, _firePoint.position, _firePoint.rotation);
        if (_orbInstance.GetComponent<WeigthOrb>() != null)
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

    public void ChangeOrb(InputAction.CallbackContext ctx){
        if (!IsEnabled) return;
        _indexLastOrb = _indexOrb;
        int _nextValueOrbs = (int) ctx.ReadValue<float>();      
        _indexOrb += _nextValueOrbs;
        if(_indexOrb >= _chargedOrbs.Count){
            _indexOrb = 0;
        }
        if(_indexOrb < 0){
            _indexOrb = (_chargedOrbs.Count - 1);
        }
        Debug.Log("Next Value Orb:" + ctx.ReadValue<float>());
        _selectedOrb = _chargedOrbs[_indexOrb];
        ChangeOrbText();
        ChangeOrbRotation();
    }

    public void ChangeOrbDirectly(InputAction.CallbackContext ctx){
        if (!IsEnabled) return;
        _indexLastOrb = _indexOrb;
        _indexOrb = (int) ctx.ReadValue<float>();   
        _selectedOrb = _chargedOrbs[_indexOrb];
        ChangeOrbText();
        ChangeOrbRotation();
    }

    public void ChangeOrbWeigth(InputAction.CallbackContext ctx){
        if (!IsEnabled) return;

        if (_selectedOrb.GetComponent<WeigthOrb>() != null){
            _augmentWeigthOrb = !_augmentWeigthOrb;
            ChangeAugmentText();
        } 
    }

    private void ChangeOrbText(){
        switch(_indexOrb){
            case 0:
                _orbText.text = "Orbe de dispersión";
                break;
            case 1:
                _orbText.text = "Orbe de hielo";
                break;
            case 2:
                _orbText.text = "Orbe de gravedad";
                break;
        }
    }

    private void ChangeOrbRotation(){
        Vector3 newRotation = new Vector3(0,0,0);
        switch(_indexOrb){
            case 0:
                if(_indexLastOrb == 1)
                {
                    newRotation = new Vector3(0, -120, 0);
                } else 
                {
                    if (_indexLastOrb == 2)
                    {
                        newRotation = new Vector3(0, -240, 0);
                    }
                }
                break;
            case 1:
                if(_indexLastOrb == 0)
                {
                    newRotation = new Vector3(0, 120, 0);
                } else 
                {
                    if (_indexLastOrb == 2)
                    {
                        newRotation = new Vector3(0, -120, 0);
                    }
                }
                break;
            case 2:
                if(_indexLastOrb == 1)
                {
                    newRotation = new Vector3(0, 120, 0);
                } else 
                {
                    if (_indexLastOrb == 0)
                    {
                        newRotation = new Vector3(0, 240, 0);
                    }
                }
                break;
        }
        _cilinderOrb.transform.Rotate(newRotation);
    }

    private void ChangeAugmentText(){
        if(_augmentWeigthOrb){
            _orbWeigthText.text = "Aumento de peso";
        }else{
            _orbWeigthText.text = "Reducción de peso";
        }
    }
}
