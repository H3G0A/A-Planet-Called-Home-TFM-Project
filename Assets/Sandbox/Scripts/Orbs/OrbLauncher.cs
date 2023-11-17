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
    [SerializeField] bool _augmentWeigthOrb;


    [Header("Aiming")]
    [SerializeField] Transform _firePoint;
    [SerializeField] Camera _mainCamera;
    [SerializeField] LayerMask _layersToAim;
    [SerializeField] OrbTrajectoryPrediction _trajectoryPrediction;


    [Header("Player")]
    [SerializeField] FirstPersonController _FPController;


    [Header("OrbRotation")]
    //CylinderOrb
    [SerializeField] GameObject _cilinderOrb;
    [SerializeField] float _rotationSpeed;

    [Header("OrbsCapsules")]
    [SerializeField] GameObject _firstOrbCapsule;
    [SerializeField] GameObject _secondOrbCapsule;
    [SerializeField] GameObject _thirdOrbCapsuleHiggerGravity;
    [SerializeField] GameObject _thirdOrbCapsuleLesserGravity;

    [Header("Audio")]
    [SerializeField] AudioClip _shootingSound;
    [SerializeField] AudioClip _orbChangeSound;
    [SerializeField] AudioClip _changeGravityModeSound;

    //Audio
    AudioSource _audioSource;

    // Launcher
    private int _indexOrb;
    private int _indexLastOrb;
    
    // Timers
    private float _fireRateDelta = 0;

    // Orbs
    private DispersionOrb _firedDispersion;

    // Controls
    PlayerInputController _inputController;
    bool _isAiming = false;

    private void OnEnable()
    {
        Application.onBeforeRender += TakeAim;
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= TakeAim;
    }

    void Awake()
    {
        _inputController = GetComponentInParent<PlayerInputController>();
        _audioSource = GetComponent<AudioSource>();

        // Position launcher point at the middle of the screen
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
        ChangeOrbRotation();
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
            if(_augmentWeigthOrb){
                _thirdOrbCapsuleHiggerGravity.SetActive(true);
                _thirdOrbCapsuleLesserGravity.SetActive(false);
            } else {
                _thirdOrbCapsuleLesserGravity.SetActive(true);
                _thirdOrbCapsuleHiggerGravity.SetActive(false);
            }
        } else
        {
            _thirdOrbCapsuleHiggerGravity.SetActive(false);
            _thirdOrbCapsuleLesserGravity.SetActive(false);
        }
    }

    internal void TakeAim()
    {
        if (!IsEnabled || !_isAiming || WaitingForDispersionOrb()) return;

        OrbBehaviour orb = _selectedOrb.GetComponent<OrbBehaviour>();
        Rigidbody orbRb = _selectedOrb.GetComponent<Rigidbody>();

        float g = orb.Gravity;
        Vector3 v0 = GetForceDirection() * _force / orbRb.mass;

        _trajectoryPrediction.DrawPrediction(g, v0);
    }

    public void AimAction(InputAction.CallbackContext ctx)
    {
        float aimAction = ctx.ReadValue<float>();

        if(WaitingForDispersionOrb() && ctx.action.WasPressedThisFrame())
        {
            ShootOrb();
        }
        else if(_fireRateDelta <= 0 && aimAction > 0.5)
        {
            _isAiming = true;
        }
        else
        { 
            _isAiming = false; 
        }
    }

    public void ShootOrb()
    {
        if (!IsEnabled) return;

        //First, if a dispersion orb has already been shot, make it explode
        if (WaitingForDispersionOrb())
        {
            _firedDispersion.Activate();
            return;
        }

        // If not on cooldown, shoot
        if (_fireRateDelta <= 0)
        {
            _audioSource.PlayOneShot(_shootingSound);
            InstantiateOrb(GetForceDirection());
        }

    }

    public void ShootAction(InputAction.CallbackContext ctx)
    {
        bool wasAiming = _trajectoryPrediction.HidePrediction();

        if (!WaitingForDispersionOrb() && wasAiming)
            ShootOrb();
    }

    private bool WaitingForDispersionOrb()
    {
        return _selectedOrb.GetComponent<OrbBehaviour>().ID == (int)GlobalParameters.Orbs.DISPERSION && _firedDispersion != null;
    }

    private Vector3 GetForceDirection()
    {
        // By default nothing has been hit, so simulate a far point
        Vector3 _forceDirection = _mainCamera.ScreenToWorldPoint(new(Screen.width / 2, Screen.height / 2, 100)) - _firePoint.transform.position;

        //// Raycast from camera to get impact point and shoot accordingly
        //bool _inRangeCollision = Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out RaycastHit _hit, _range, _layersToAim, QueryTriggerInteraction.Ignore);

        //// If something was hit shoot the orb in it's direction
        //if (_inRangeCollision)
        //{
        //    _forceDirection = _hit.point - _firePoint.transform.position;
        //};

        return _forceDirection.normalized;
    }

    void InstantiateOrb(Vector3 _forceDirection)
    {
        // Instantiate and give initial speed boost
        GameObject _orbInstance = GameObject.Instantiate(_selectedOrb, _firePoint.position, _firePoint.rotation);

        if (_orbInstance.GetComponent<WeigthOrb>() != null)
        {
            _orbInstance.GetComponent<WeigthOrb>().changeAugment(_augmentWeigthOrb);
        }
        else if (_orbInstance.GetComponent<OrbBehaviour>().ID == (int)GlobalParameters.Orbs.DISPERSION)
        {
            _firedDispersion = _orbInstance.GetComponent<DispersionOrb>();
        }

        Vector3 _forceVector = _forceDirection * _force;
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

        if(_chargedOrbs.Count > 1) _audioSource.PlayOneShot(_orbChangeSound);

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
        ChangeOrbRotation();

        if(WaitingForDispersionOrb()) _trajectoryPrediction.HidePrediction();
    }

    public void ChangeOrbDirectly(InputAction.CallbackContext ctx){
        if (!IsEnabled) return;

        if (_chargedOrbs.Count > 1) _audioSource.PlayOneShot(_orbChangeSound);

        _indexLastOrb = _indexOrb;
        _indexOrb = (int) ctx.ReadValue<float>();   
        _selectedOrb = _chargedOrbs[_indexOrb];
        ChangeOrbRotation();
    }

    public void ChangeOrbWeigth(InputAction.CallbackContext ctx){
        if (!IsEnabled) return;

        if (_selectedOrb.GetComponent<WeigthOrb>() != null){
            _audioSource.PlayOneShot(_changeGravityModeSound);

            _augmentWeigthOrb = !_augmentWeigthOrb;
            if(_augmentWeigthOrb)
            {
                
            }
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
}
