using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GlobalParameters;

public class PlayerInputController : MonoBehaviour
{
    // Player
    FirstPersonController _playerController;
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    public bool Sprint { get; private set; }
    public bool Jump { get; set; }

    // Orb Launcher
    OrbLauncher _launcherController;

    // Player Input Component
    PlayerInput _playerInput;

    // Player Input Actions
    InputAction _moveAction;
    InputAction _lookAction;
    InputAction _jumpAction;
    InputAction _sprintAction;
    InputAction _changeWeightAction;

    // Orb Launcher Input Actions
    InputAction _shootAction;
    InputAction _changeOrb;
    InputAction _changeOrbWeigth;
    InputAction _changeOrbDirectly;

    void Awake()
    {
        // COMPONENTS
        _playerInput = GetComponent<PlayerInput>();
        _playerController = GetComponent<FirstPersonController>();
        _launcherController = GetComponentInChildren<OrbLauncher>();

        // PLAYER INPUTS
        _moveAction = _playerInput.actions[MOVE_ACTION];
        _lookAction = _playerInput.actions[LOOK_ACTION];
        _sprintAction = _playerInput.actions[SPRINT_ACTION];
        _jumpAction = _playerInput.actions[JUMP_ACTION];
        _changeWeightAction = _playerInput.actions[CHANGE_WEIGHT_ACTION];

        //ORB LAUNCHER INPUTS
        _shootAction = _playerInput.actions[SHOOT_ACTION];
        _changeOrb = _playerInput.actions[CHANGEORB_ACTION];
        _changeOrbWeigth = _playerInput.actions[CHANGEORBWEIGTH_ACTION];
        _changeOrbDirectly = _playerInput.actions[CHANGEORBDIRECTLY_ACTION];
    }

    void OnEnable()
    {
        EnableInputCallbacks();
    }

    void OnDisable()
    {
        DisableInputCallbacks();
    }

    void EnableInputCallbacks()
    {
        //////////////////////// PLAYER INPUTS ////////////////////////////////
        // MOVE
        _moveAction.performed += SetMove;
        _moveAction.canceled += SetMove;

        // LOOK
        _lookAction.performed += SetLook;
        _lookAction.canceled += SetLook;

        // SPRINT
        _sprintAction.performed += SetSprint;
        _sprintAction.canceled += SetSprint;

        // JUMP
        _jumpAction.performed += SetJump;

        // CHANGE WEIGHT
        _changeWeightAction.performed += _playerController.ChangeWeight;


        //////////////////////// ORB LAUNCHER INPUTS ////////////////////////////////
        // SHOOTING
        _shootAction.performed += _launcherController.ShootOrb;

        // ROTATE WEAPON CHANGE
        _changeOrb.performed += _launcherController.ChangeOrb;

        // DIRECT WEAPON CHANGE
        _changeOrbDirectly.performed += _launcherController.ChangeOrbDirectly;

        // CHANGE WEIGHT ORB GRAVITY
        _changeOrbWeigth.performed += _launcherController.ChangeOrbWeigth;
    }

    void DisableInputCallbacks()
    {
        //////////////////////// PLAYER INPUTS ////////////////////////////////
        // MOVE
        _moveAction.performed -= SetMove;
        _moveAction.canceled -= SetMove;

        // LOOK
        _lookAction.performed -= SetLook;
        _lookAction.canceled -= SetLook;

        // SPRINT
        _sprintAction.performed -= SetSprint;
        _sprintAction.canceled -= SetSprint;

        // JUMP
        _jumpAction.performed -= SetJump;

        // CHANGE WEIGHT
        _changeWeightAction.performed -= _playerController.ChangeWeight;


        //////////////////////// ORB LAUNCHER INPUTS ////////////////////////////////
        // SHOOTING
        _shootAction.performed -= _launcherController.ShootOrb;

        // ROTATE WEAPON CHANGE
        _changeOrb.performed -= _launcherController.ChangeOrb;

        // DIRECT WEAPON CHANGE
        _changeOrbDirectly.performed -= _launcherController.ChangeOrbDirectly;

        // CHANGE WEIGHT ORB GRAVITY
        _changeOrbWeigth.performed -= _launcherController.ChangeOrbWeigth;
    }


    void SetMove(InputAction.CallbackContext ctx)
    {
        Move = ctx.ReadValue<Vector2>();
    }

    void SetLook(InputAction.CallbackContext ctx)
    {
        Look = ctx.ReadValue<Vector2>();
    }

    void SetSprint(InputAction.CallbackContext ctx)
    {
        Sprint = ctx.ReadValue<float>() == 1;
    }

    void SetJump(InputAction.CallbackContext ctx)
    {
        Jump = true;
    }
}
