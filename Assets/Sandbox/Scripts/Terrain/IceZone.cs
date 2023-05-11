using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class IceZone : MonoBehaviour
{
    [SerializeField] float _delayDestroyTimer;
    [SerializeField] float _radius;
    [SerializeField] LayerMask _iceLayers;
    [SerializeField] bool _isPlayerOn = false;
    
    BoxCollider _zoneCollider;
    Projector _projector;
    float _maxHeight;
    float _minHeight;

    FirstPersonController _FPController = null;

    private void Awake()
    {
        _maxHeight = 0;
        _minHeight = 0;

        _zoneCollider = GetComponent<BoxCollider>();
        _projector = GetComponentInChildren<Projector>();
        
        GetColliderHeights();
        SetColliderBounds();
    }

    void Start()
    {
        Destroy(gameObject, _delayDestroyTimer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            if (_FPController == null) _FPController = other.GetComponent<FirstPersonController>();
            _FPController.OnIce += 1;
            _isPlayerOn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            if (_FPController == null) _FPController = other.GetComponent<FirstPersonController>();
            _FPController.OnIce -= 1;
            _isPlayerOn = false;
        }
    }

    private void OnDestroy()
    {
        if (_isPlayerOn) _FPController.OnIce -= 1;
    }

    private void GetColliderHeights()
    {
        Vector3 _castPoint = new(transform.position.x, transform.position.y + _radius, transform.position.z);
        Vector3 _halfExtents = new(_radius, .1f, _radius);
        RaycastHit[] _collisions =  Physics.BoxCastAll(_castPoint, _halfExtents, Vector3.down, Quaternion.identity, _radius*2, _iceLayers, QueryTriggerInteraction.Ignore);


        //Vector3 _origin = new(transform.position.x, transform.position.y + .1f, transform.position.z);
        foreach(RaycastHit _hit in _collisions)
        {
            //Vector3 _distance = _origin - hit.point;
            //RaycastHit _hit;
            //bool _obstacle = Physics.Raycast(hit.point, _distance, out _hit, _distance.magnitude, _iceLayers, QueryTriggerInteraction.Ignore);

            //if(_obstacle) Debug.Log("obstacle: " + _hit.collider.gameObject);
            //if (!_obstacle)
            //{

            //}
            //Debug.Log(_hit.collider.gameObject);
            if (_hit.point.y > _maxHeight) _maxHeight = _hit.point.y;
            if (_hit.point.y < _minHeight) _minHeight = _hit.point.y;
            Debug.Log("Max: " + _maxHeight);
            Debug.Log("Min: " + _minHeight);
        }
    }

    private void SetColliderBounds()
    {
        float _colliderHeight = _maxHeight - _minHeight;
        _zoneCollider.size = new(_radius * 2, _colliderHeight, _radius * 2);
        //Some offset to ensure the player always hits the trigger
        float _triggerOffset = .3f;
        _zoneCollider.center = new(_zoneCollider.center.x, _zoneCollider.center.x + _triggerOffset, _zoneCollider.center.z);
        
        // Match projector size with collider
        _projector.orthographicSize = _radius;
        _projector.transform.localPosition = new(0, (_colliderHeight * .5f) + _projector.nearClipPlane + _triggerOffset + .1f, 0);
        _projector.farClipPlane = _colliderHeight + _projector.nearClipPlane + .1f;
    }

    private void OnDrawGizmosSelected()
    {
        //HEIGHTS CHECK GIZMO
        Gizmos.color = new(1.0f, 0.0f, 0.0f, 0.08f); //Transparent red
        
        Vector3 _boxCenter = new(transform.position.x, transform.position.y, transform.position.z);
        Vector3 _halfExtents = new(_radius, _radius, _radius);

        Gizmos.DrawCube(_boxCenter, _halfExtents*2);
    }
}
