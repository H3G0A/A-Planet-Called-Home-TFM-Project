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
    public bool Interact { get; set; }

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
    InputAction _interactAction;

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
        _interactAction = _playerInput.actions[INTERACT_ACTION];

        //ORB LAUNCHER INPUTS
        _shootAction = _playerInput.actions[SHOOT_ACTION];
        _changeOrb = _playerInput.actions[CHANGEORB_ACTION];
        _changeOrbWeigth = _playerInput.actions[CHANGEORBWEIGTH_ACTION];
        _changeOrbDirectly = _playerInput.actions[CHANGEORBDIRECTLY_ACTION];
    }

    private void Start()
    {
        //Set reference in GameManager
        GameManager.Instance.PlayerInputController_ = this;
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
        _playerInput.ActivateInput();
        GameManager.Instance.CurrentControlScheme = _playerInput.currentControlScheme;

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

        // INTERACT
        _interactAction.performed += SetInteract;
        _interactAction.canceled += SetInteract;


        //////////////////////// ORB LAUNCHER INPUTS ////////////////////////////////
        // SHOOTING
        _shootAction.started += _launcherController.AimAction;
        _shootAction.performed += _launcherController.AimAction;
        _shootAction.performed += _launcherController.ShootAction;

        // ROTATE WEAPON CHANGE
        _changeOrb.performed += _launcherController.ChangeOrb;

        // DIRECT WEAPON CHANGE
        _changeOrbDirectly.performed += _launcherController.ChangeOrbDirectly;

        // CHANGE WEIGHT ORB GRAVITY
        _changeOrbWeigth.performed += _launcherController.ChangeOrbWeigth;
    }

    void DisableInputCallbacks()
    {
        _playerInput.DeactivateInput();

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

        // INTERACT
        _interactAction.performed -= SetInteract;
        _interactAction.canceled -= SetInteract;


        //////////////////////// ORB LAUNCHER INPUTS ////////////////////////////////
        // SHOOTING
        _shootAction.started -= _launcherController.AimAction;
        _shootAction.canceled -= _launcherController.AimAction;
        _shootAction.canceled -= _launcherController.ShootAction;

        // ROTATE WEAPON CHANGE
        _changeOrb.performed -= _launcherController.ChangeOrb;

        // DIRECT WEAPON CHANGE
        _changeOrbDirectly.performed -= _launcherController.ChangeOrbDirectly;

        // CHANGE WEIGHT ORB GRAVITY
        _changeOrbWeigth.performed -= _launcherController.ChangeOrbWeigth;
    }

    //This version does not work

    //void SetScheme(PlayerInput input)
    //{
    //    Debug.Log("Current Scheme: " + input.currentControlScheme);
    //    GameManager.Instance.CurrentControlScheme = input.currentControlScheme;
    //}

    public void SetScheme()
    {
        GameManager.Instance.CurrentControlScheme = _playerInput.currentControlScheme;
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

    void SetInteract(InputAction.CallbackContext ctx)
    {
        Interact = ctx.action.WasPerformedThisFrame();
    }
}
