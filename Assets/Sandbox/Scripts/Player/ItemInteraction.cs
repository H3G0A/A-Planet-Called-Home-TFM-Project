using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    [Header("Range Detection")]
    [SerializeField]
    [Range(1, 10)] float _radius;
    [SerializeField] LayerMask _layers;
    [SerializeField] Transform _cameraRoot;

    [Header("Interact Cast")]
    [SerializeField] 
    [Range(1, 10)] float _range;

    [Header("Debugging")]
    [SerializeField] bool _drawGizmos = false;

    // Scripts
    PlayerInputController _inputController;
    IInteractable _detectedObj;


    private void Awake()
    {
        _inputController = GetComponent<PlayerInputController>();
    }

    private void Update()
    {
        Interact();
    }

    private bool CheckInRadius()
    {
        Vector3 center = _cameraRoot.position;
        return Physics.CheckSphere(center, _radius, _layers);
    }

    private bool CheckInFront()
    {
        Vector3 origin = _cameraRoot.position;
        Vector3 direction = _cameraRoot.forward;
        Physics.Raycast(origin, direction, out RaycastHit hit, _range, _layers, QueryTriggerInteraction.Collide);

        bool wasDetected = hit.collider != null;
        if (wasDetected)
        {
            _detectedObj = hit.collider.gameObject.GetComponent<IInteractable>();
            //TODO: show "Interact" text hint
        }

        return wasDetected;
    }

    private void Interact()
    {
        if (!CheckInRadius()) return;

        if (CheckInFront() && _inputController.Interact)
        {
            _detectedObj.TriggerInteraction();
        }
        
        _inputController.Interact = false;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////
    private void OnDrawGizmos()
    {
        if (!_drawGizmos) return;

        Color transparentGreen = new(0.0f, 1.0f, 0.0f, 0.35f);

        Gizmos.color = transparentGreen;
        Gizmos.DrawSphere(_cameraRoot.position, _radius);

        Gizmos.color = Color.red;
        Vector3 endPoint = _cameraRoot.position + _cameraRoot.forward * _range;
        Gizmos.DrawLine(_cameraRoot.position, endPoint);
    }
}
