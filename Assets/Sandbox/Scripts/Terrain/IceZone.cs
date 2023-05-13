using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class IceZone : MonoBehaviour
{
    [SerializeField] bool permanent = false;
    [SerializeField] float _delayDestroyTimer;
    [SerializeField] float _height;
    [SerializeField] float _radius;
    [SerializeField] LayerMask _iceLayers;
    
    BoxCollider _zoneCollider;
    Projector _projector;
    Vector3 _maxHeight;
    Vector3 _minHeight;

    FirstPersonController _FPController = null;

    private void Awake()
    {
        _zoneCollider = GetComponent<BoxCollider>();
        _projector = GetComponentInChildren<Projector>();
        
        //_zoneCollider.enabled = false;
    }

    void Start()
    {
        if(!permanent) Destroy(gameObject, _delayDestroyTimer);


        //GetColliderHeights();
        SetColliderBounds2();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    PlayerIsOn(other);
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    PlayerIsOff(other);
    //}

    //private void OnDestroy()
    //{
    //    if (_isPlayerOn) _FPController.OnIce -= 1;
    //}

    //private void GetColliderHeights()
    //{
    //    Vector3 _castPoint = new(transform.position.x, transform.position.y + _radius.y, transform.position.z);
    //    Vector3 _halfExtents = new(_radius.x, .1f, _radius.z);
    //    Debug.Log("Cast point: " + _castPoint);
    //    RaycastHit[] _collisions =  Physics.BoxCastAll(_castPoint, _halfExtents, Vector3.down, Quaternion.identity, _radius.y*2, _iceLayers, QueryTriggerInteraction.Ignore);

    //    //Vector3 _origin = new(transform.position.x, transform.position.y + .1f, transform.position.z);
    //    int i = 0;
    //    foreach(RaycastHit _hit in _collisions)
    //    {
    //        Debug.Log("Name: " + _hit.collider.gameObject + "; HitPoint: " + _hit.point);
    //        //Vector3 _distance = _origin - hit.point;
    //        //RaycastHit _hit;
    //        //bool _obstacle = Physics.Raycast(hit.point, _distance, out _hit, _distance.magnitude, _iceLayers, QueryTriggerInteraction.Ignore);

    //        //if(_obstacle) Debug.Log("obstacle: " + _hit.collider.gameObject);
    //        //if (!_obstacle)
    //        //{

    //        //}
    //        //Debug.Log(_hit.collider.gameObject);

    //        //Get local coordinates of the hit point
            
    //        if(i == 0)
    //        {
    //            _maxHeight = _hit.point;
    //            _minHeight = _hit.point;
    //        }
    //        else
    //        {
    //            if (_hit.point.y > _maxHeight.y) _maxHeight = _hit.point;
    //            if (_hit.point.y < _minHeight.y) _minHeight = _hit.point;
    //        }

    //        i++;
    //    }
    //}

    //private void SetColliderBounds()
    //{
    //    //Vector3 _hitPoint = transform.InverseTransformPoint(_hit.point);
    //    float _colliderHeight = _maxHeight.y - _minHeight.y;
    //    float _extraSize = .4f; //Extra size to ensure the player always hits the trigger
    //    float _triggerOffset = (_maxHeight.y + _minHeight.y) * 0.5f;

    //    //_zoneCollider.center = new(_zoneCollider.center.x, -_triggerOffset, _zoneCollider.center.z);
    //    transform.position = new(transform.position.x, _triggerOffset, transform.position.z);
    //    _zoneCollider.size = new(_radius.x * 2, _colliderHeight + _extraSize, _radius.z * 2);

    //    _zoneCollider.enabled = true;

    //    // Match projector size with collider
    //    Vector3 _projectorPos = new(0, _zoneCollider.center.y + (_zoneCollider.size.y * 0.5f) + _projector.nearClipPlane, 0);

    //    _projector.orthographicSize = _radius.x;
    //    _projector.transform.localPosition = _projectorPos;
    //    _projector.farClipPlane = _colliderHeight + _projector.nearClipPlane + _extraSize;
    //}
    
    private void SetColliderBounds2()
    {
        _zoneCollider.size = new(_radius * 2, _height, _radius * 2);

        // Match projector size with collider
        Vector3 _projectorPos = new(0, _zoneCollider.center.y + (_zoneCollider.size.y * 0.5f) + _projector.nearClipPlane, 0);

        _projector.orthographicSize = _radius;
        _projector.transform.localPosition = _projectorPos;
        _projector.farClipPlane = _height + _projector.nearClipPlane;
    }

    //private void PlayerIsOn(Collider _playerCollider)
    //{
    //    if (_playerCollider.CompareTag(PLAYER_TAG))
    //    {
    //        if (_FPController == null) _FPController = _playerCollider.GetComponent<FirstPersonController>();
    //        _FPController.OnIce += 1;
    //        _isPlayerOn = true;
    //    }
    //}

    //private void PlayerIsOff(Collider _playerCollider)
    //{
    //    if (_playerCollider.CompareTag(PLAYER_TAG))
    //    {
    //        if (_FPController == null) _FPController = _playerCollider.GetComponent<FirstPersonController>();
    //        _FPController.OnIce -= 1;
    //        _isPlayerOn = false;
    //    }
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    //HEIGHTS CHECK GIZMO
    //    Gizmos.color = new(1.0f, 0.0f, 0.0f, 0.08f); //Transparent red
        
    //    Vector3 _boxCenter = transform.position;
    //    Vector3 _halfExtents = new(_radius.x, _radius.y, _radius.z);

    //    Gizmos.DrawCube(_boxCenter, _halfExtents * 2);
    //}
}
