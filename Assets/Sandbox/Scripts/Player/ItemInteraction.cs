using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    PlayerControls _input;
    IInteractable _detectedObj;

    bool _canInteract = false;


    private void Awake()
    {
        _input = new PlayerControls();
    }
    private void OnEnable()
    {
        EnableInput();
    }

    private void OnDisable()
    {
        DisableInput();
    }

    private void Update()
    {
        InteractPrompt();
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

    private void InteractPrompt()
    {
        if (!CheckInRadius())
        {
            SetPrompt(false);
            return;
        }

        if (CheckInFront())
        {
            SetPrompt(true);
        }
        else
        {
            SetPrompt(false);
        }
    }

    private void SetPrompt(bool enabled)
    {
        GameManager.Instance.SetInteractPromptActive(enabled);
        _canInteract = enabled;
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        if (!ctx.action.WasPerformedThisFrame()) return;

        if (GameManager.Instance._HUDTextNote.activeSelf)
        {
            GameManager.Instance.HideNote();
        }
        else
        {
            _detectedObj.TriggerInteraction();
        }
    }

    private void EnableInput()
    {
        _input.Ground.Enable();
        _input.Ground.Interact.performed += Interact;
    }

    private void DisableInput()
    {
        _input.Ground.Interact.performed -= Interact;
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
