using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GlobalParameters;

public class InputManager : MonoBehaviour
{
    [Header("Character Input Values")]
    [SerializeField] Vector2 _move;
    [SerializeField] Vector2 _look;
    [SerializeField] bool _jump;
    [SerializeField] bool _sprint;
    [SerializeField] bool _shoot;
    [SerializeField] bool _aim;

    [Header("Mouse Cursor Settings")]
    [SerializeField] bool _cursorLocked = true;

    //Input
    private PlayerInput _playerInput;
    
    private InputAction _shootAction;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _jumpAction;
    private InputAction _sprintAction;
    private InputAction _aimAction;

    //auto-implemented properties
    public Vector2 Move
    {
        get { return _move; }
        private set { _move = value; }
    }
    public Vector2 Look
    {
        get { return _look; }
        private set { _look = value; }
    }
    public bool Jump
    {
        get { return _jump; }
        private set { _jump = value; }
    }
    public bool Sprint
    {
        get { return _sprint; }
        private set { _sprint = value; }
    }
    public bool Shoot
    {
        get { return _shoot; }
        private set { _shoot = value; }
    }
    public bool Aim
    {
        get { return _aim; }
        private set { _aim = value; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        //assign actions to variables
        
        _aimAction = _playerInput.actions[AIM_ACTION];
        _shootAction = _playerInput.actions[SHOOT_ACTION];
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetInputCallbacks()
    {
        // MOVE
        _moveAction.performed += ctx => _move = ctx.ReadValue<Vector2>();

        // LOOK
        _lookAction.performed += ctx => _look = ctx.ReadValue<Vector2>();

        // SPRINT
        _sprintAction.performed += ctx => _sprint = true;

        // JUMP
        _jumpAction.performed += ctx => _jump = true;

        // AIM
        _aimAction.performed += ctx => _aim = true;
        _aimAction.canceled += ctx => _aim = false;

        // SHOOT
        _sprintAction.performed += ctx => _sprint = true;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(_cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
