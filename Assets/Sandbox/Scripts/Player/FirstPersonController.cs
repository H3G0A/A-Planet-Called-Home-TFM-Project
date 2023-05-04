using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GlobalParameters;

public class FirstPersonController : MonoBehaviour
{
	[Header("Player")]
	[Tooltip("Move speed of the character in m/s")]
	[SerializeField] float _moveSpeed = 4.0f;
	[Tooltip("Sprint speed of the character in m/s")]
	[SerializeField] float _sprintSpeed = 6.0f;
	[SerializeField] float _slipMultiplier = 2f;
	[Tooltip("Acceleration and deceleration")]
	[SerializeField] float _speedChangeRate = 10.0f;

	[Space(10)]
	[SerializeField] Vector3 _finalMovement = Vector3.zero;

	[Space(10)]
	[Tooltip("The height the player can jump")]
	[SerializeField] float _jumpHeight = 1.2f;
	[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
	[SerializeField] float _gravity = -15.0f;
	[SerializeField] float _terminalVelocity = 53.0f;

	[Space(10)]
	[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
	[SerializeField] float _fallTimeout = 0.15f;

	[Header("Player Grounded")]
	[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
	[SerializeField] bool _grounded = true;
	[SerializeField] bool _sliped = false;
	[Tooltip("Useful for rough ground")]
	[SerializeField] float _groundedOffset = 0.05f;
	[Tooltip("Half the side of the grounded checkbox. Should match the radius of the CharacterController")]
	[SerializeField] Vector3 _groundedBoxSize = new(1, 0.1f, 1);
	[Tooltip("Half the side of the grounded check. Should match the radius of the CharacterController")]
	[SerializeField] float _groundedCastDistance = 0.1f;
	[Tooltip("What layers the character uses as ground")]
	[SerializeField] LayerMask _groundLayers;
	[SerializeField] LayerMask _slipLayers;

	[Header("Cinemachine")]
	[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
	[SerializeField] GameObject _cinemachineCameraTarget;
	[Tooltip("How far in degrees can you move the camera up")]
	[SerializeField] float _topClamp = 90.0f;
	[Tooltip("How far in degrees can you move the camera down")]
	[SerializeField] float _bottomClamp = -90.0f;

	[Header("Character Input Values")]
	[SerializeField] Vector2 _move;
	[SerializeField] Vector2 _look;
	[SerializeField] bool _sprint;
	[SerializeField] bool _jump;

	[Header("Mouse Cursor Settings")]
	public bool cursorLocked = true;

	// cinemachine
	private float _cinemachineTargetPitch;

	// player
	private float _speed;
	private float _verticalVelocity;
	private Vector3 _cumulatedMovement;

	// timers
	private float _fallTimeoutDelta;

	// Player input
	private PlayerInput _playerInput;
	private InputAction _moveAction;
	private InputAction _lookAction;
	private InputAction _jumpAction;
	private InputAction _sprintAction;
	
	private CharacterController _controller;

	private void Awake()
	{
		_controller = GetComponent<CharacterController>();
		_playerInput = GetComponent<PlayerInput>();

		//assign actions to variables
		_moveAction = _playerInput.actions[MOVE_ACTION];
		_lookAction = _playerInput.actions[LOOK_ACTION];
		_sprintAction = _playerInput.actions[SPRINT_ACTION];
		_jumpAction = _playerInput.actions[JUMP_ACTION];
	}

	void Start()
    {
		_fallTimeoutDelta = _fallTimeout; // reset our timeouts on start

		SetInputCallbacks();
	}

    void Update()
    {
		//Player movement
		GroundCheck();
		ManageGravity();
		ManageJump();
		ManageMovement();
		ApplyMovement();

		//Ice orb
		SlipCheck();
    }

    void LateUpdate()
    {
		CameraRotation();
    }

    private void GroundCheck()
    {
		Vector3 _boxCenter = new(transform.position.x, transform.position.y + _groundedOffset, transform.position.z);
		_grounded = Physics.BoxCast(_boxCenter, _groundedBoxSize / 2, -transform.up, transform.rotation, _groundedCastDistance, _groundLayers);
	}

	private void SlipCheck()
	{
		Vector3 _boxCenter = new(transform.position.x, transform.position.y + _groundedOffset, transform.position.z);
		_sliped = Physics.BoxCast(_boxCenter, _groundedBoxSize / 2, -transform.up, transform.rotation, _groundedCastDistance, _slipLayers, QueryTriggerInteraction.Ignore);
	}

	private void ManageMovement()
    {
		// set target speed based on move speed, sprint speed and if sprint is pressed
		float _targetSpeed = _sprint ? _sprintSpeed : _moveSpeed;

		_targetSpeed = _sliped ? _targetSpeed * _slipMultiplier : _targetSpeed;

		// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

		// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
		// if there is no input, set the target speed to 0
		if (_move == Vector2.zero) _targetSpeed = 0.0f;

		// a reference to the players current horizontal velocity
		float _currentHorizontalSpeed = new Vector3(_finalMovement.x, 0.0f, _finalMovement.z).magnitude;

		float _speedOffset = 0.1f;

		// accelerate or decelerate to target speed
		if (_currentHorizontalSpeed < _targetSpeed - _speedOffset || _currentHorizontalSpeed > _targetSpeed + _speedOffset)
		{
			// creates curved result rather than a linear one giving a more organic speed change
			// note T in Lerp is clamped, so we don't need to clamp our speed
			_speed = Mathf.Lerp(_currentHorizontalSpeed, _targetSpeed * _move.magnitude, Time.deltaTime * _speedChangeRate);

			// round speed to 3 decimal places
			_speed = Mathf.Round(_speed * 1000f) / 1000f;
		}
		else
		{
			_speed = _targetSpeed;
		}

		// Make the input for the movement into a vector3
		Vector3 _inputDirection = transform.right * _move.x + transform.forward * _move.y;


		// move the player
		_finalMovement = (_inputDirection.normalized * _speed + new Vector3(0.0f, _verticalVelocity, 0.0f));
		MoveCharacter(_finalMovement * Time.deltaTime);
	}

	private void ManageGravity()
    {
        if (_grounded)
        {
			// reset the fall timeout timer
			_fallTimeoutDelta = _fallTimeout;

			// stop our velocity dropping infinitely when grounded
			if (_verticalVelocity < 0.0f)
			{
				_verticalVelocity = -2f;
			}
		}
        else
        {
			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (Mathf.Abs(_verticalVelocity) < Mathf.Abs(_terminalVelocity))
			{
				_verticalVelocity += _gravity * Time.deltaTime;
			}
		}
    }

	private void ManageJump()
    {
        if (_grounded)
        {
			// Jump
			if (_jump)
			{
				// the square root of H * -2 * G = how much velocity needed to reach desired height
				_verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
			}
        }
		else
		{
			// fall timeout
			if (_fallTimeoutDelta >= 0.0f)
			{
				_fallTimeoutDelta -= Time.deltaTime;
			}

			_jump = false;
		}
	}

    private void CameraRotation()
    {
        _cinemachineTargetPitch += _look.y;

        // clamp our pitch rotation
        _cinemachineTargetPitch = Mathf.Clamp(_cinemachineTargetPitch, _bottomClamp, _topClamp);

        // Update Cinemachine camera target pitch
        _cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

        // rotate the player left and right
        transform.Rotate(Vector3.up * _look.x);
    }

    public void MoveCharacter(Vector3 movement)
    {
		_cumulatedMovement += movement;
    }

	private void ApplyMovement()
    {
		_controller.Move(_cumulatedMovement);
		_cumulatedMovement = Vector3.zero;
	}

	private void SetInputCallbacks()
    {
		// MOVE
		_moveAction.performed += ctx => _move = ctx.ReadValue<Vector2>();
		_moveAction.canceled += ctx => _move = Vector2.zero;

		// LOOK
		_lookAction.performed += ctx => _look = ctx.ReadValue<Vector2>();
		_lookAction.canceled += ctx => _look = Vector2.zero;

		// SPRINT
		_sprintAction.performed += ctx => _sprint = true;
		_sprintAction.canceled += ctx => _sprint = false;

		// JUMP
		_jumpAction.performed += ctx => _jump = true;
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////

	private void OnDrawGizmosSelected()
    {
		Color transparentGreen = new(0.0f, 1.0f, 0.0f, 0.35f);
		Color transparentRed = new(1.0f, 0.0f, 0.0f, 0.35f);

		if (_grounded) Gizmos.color = transparentGreen;
		else Gizmos.color = transparentRed;

		Gizmos.DrawCube(new(transform.position.x, transform.position.y + _groundedOffset, transform.position.z), 
						new(_groundedBoxSize.x, _groundedBoxSize.y + _groundedCastDistance*2, _groundedBoxSize.z));
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}
