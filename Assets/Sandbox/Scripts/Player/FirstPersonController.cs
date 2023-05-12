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
	[SerializeField] float _currentSpeed;

	[Space(10)]
	[Tooltip("Acceleration in terrain")]
	[SerializeField] float _groundedAcceleration = 10.0f;
	[Tooltip("Deceleration in terrain")]
	[SerializeField] float _groundedDeceleration = 10.0f;

	[Space(10)]
	[SerializeField] Vector3 _horizontalVelocity = Vector3.zero;
	[SerializeField] float _verticalVelocity;

	[Space(10)]
	[Tooltip("The height the player can jump")]
	[SerializeField] float _jumpHeight = 1.2f;
	[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
	[SerializeField] float _gravity = -15.0f;
	[SerializeField] float _terminalVelocity = 53.0f;

	[Space(10)]
	[Tooltip("Acceleration while airbone")]
	[SerializeField] float _airboneAcceleration = 5.0f;
	[Tooltip(" and deceleration while airbone")]
	[SerializeField] float _airboneDeceleration = 2.0f;

	[Space(10)]
	[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
	[SerializeField] float _fallTimeout = 0.15f;


	[Header("Player Grounded")]
	[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
	[SerializeField] bool _grounded = true;
	[Tooltip("Useful for rough ground")]
	[SerializeField] float _groundedOffset = 0.05f;
	[Tooltip("The radius of the ground sphere check, should be slightly smaller than the character controller radius")]
	[SerializeField] float _groundedCheckRadius = .4f;
	//[SerializeField] Vector3 _groundedBoxSize = new(1, 0.1f, 1);
	[SerializeField] float _groundedCastDistance = 0.1f;
	[Tooltip("What layers the character uses as ground")]
	[SerializeField] LayerMask _groundLayers;

	[Space(10)]
	[SerializeField] float _edgeCheckOffset = 0.05f;
	[Tooltip("Should match the raius of the CharacterController")]
	[SerializeField] float _edgeCheckLength = .5f;
	[SerializeField] float _edgeSlipSpeed;

	[Space(10)]
	[Tooltip("Always bigger than the distance used for the grounded check")]
	[SerializeField] float _slopeCheckDistance = .2f;


	[Header("Player On Ice")]
	[SerializeField] int _onIce = 0;
	[SerializeField] float _iceMultiplier = 2f;


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
	[SerializeField] bool cursorLocked = true;

	// cinemachine
	float _cinemachineTargetPitch;

	// player
	float _targetSpeed;
	float _acceleration;
	float _deceleration;
	Vector3 _inputDirection;
	Vector3 _cumulatedMovement;

	// timers
	float _fallTimeoutDelta;

	// Player input
	PlayerInput _playerInput;
	InputAction _moveAction;
	InputAction _lookAction;
	InputAction _jumpAction;
	InputAction _sprintAction;
	
	CharacterController _controller;

	//Getters & Setters
	public int OnIce
    {
        get { return _onIce; }
		set { _onIce = value; }
    }

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
		//Checks
		GroundCheck();

		//Player movement
		ManageJump();
		ManageGravity();
		ManageMovement();

		//Apply movement
		ApplyMovement();
    }

    void LateUpdate()
    {
		CameraRotation();
    }

    private void GroundCheck()
    {
		Vector3 _boxCenter = new(transform.position.x, transform.position.y + _groundedCheckRadius + _groundedOffset, transform.position.z);
		_grounded = Physics.SphereCast(_boxCenter, _groundedCheckRadius, Vector3.down, out _, _groundedCastDistance, _groundLayers, QueryTriggerInteraction.Ignore);
		//Vector3 _boxCenter = new(transform.position.x, transform.position.y + _groundedOffset, transform.position.z);
		//_grounded = Physics.BoxCast(_boxCenter, _groundedBoxSize / 2, Vector3.down, Quaternion.identity, _groundedCastDistance, _groundLayers);

        if (!_grounded && _controller.velocity.y <= 0)
        {
			SlipCheck(); //Check if stuck on edge
        }
	}

	private void SlipCheck()
    {
		// Spawn a cross of 4 raycast to check for edged
		Vector3 _spawnPoint = new(transform.position.x, transform.position.y + _edgeCheckOffset, transform.position.z);

		Ray[] casts = new Ray[4];
		//Cardinal directions
		casts[0] = new Ray(_spawnPoint, transform.forward);
		casts[1] = new Ray(_spawnPoint, - transform.forward);
		casts[2] = new Ray(_spawnPoint, transform.right);
		casts[3] = new Ray(_spawnPoint, - transform.right);

		// If edged are found, move the player in the opposite direction to make him fall
		Vector3 slipDirection = Vector3.zero;
		foreach(Ray cast in casts)
        {
			if(Physics.Raycast(cast, _edgeCheckLength, _groundLayers))
            {
				slipDirection += -cast.direction;
            }
        }

		slipDirection.Normalize();
		MoveCharacter(_edgeSlipSpeed * Time.deltaTime * slipDirection);
	}

    private void AdjustForSlope(ref Vector3 _velocity)
    {
		bool _slopeCheck = Physics.Raycast(transform.position, Vector3.down, out RaycastHit _hit, _slopeCheckDistance, _groundLayers, QueryTriggerInteraction.Ignore);

		if (_slopeCheck && _grounded)
        {
            Quaternion _slopeRotation = Quaternion.FromToRotation(Vector3.up, _hit.normal);
            _velocity = _slopeRotation * _velocity;
        }
    }

	private void ManageMovement()
	{
		// set target speed based on move speed, sprint speed and if sprint is pressed
		if (_grounded)
		{
			_targetSpeed = _sprint ? _sprintSpeed : _moveSpeed;

			if (_onIce > 0 && _grounded) _targetSpeed *= _iceMultiplier;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_move == Vector2.zero) _targetSpeed = 0.0f;
			_acceleration = _groundedAcceleration;
			_deceleration = _groundedDeceleration;
		}
        else
        {
			if(_targetSpeed < _moveSpeed) _targetSpeed = _moveSpeed;
			if (_move == Vector2.zero) _targetSpeed = 0.0f;
			_acceleration = _airboneAcceleration;
			_deceleration = _airboneDeceleration;
		}

		// a reference to the players current horizontal velocity
		float _previousHorizontalSpeed = _horizontalVelocity.magnitude;

		float _speedOffset = 0.1f;
		// accelerate or decelerate to target speed
		if (_previousHorizontalSpeed < _targetSpeed - _speedOffset)
		{
			VelocityChange(_previousHorizontalSpeed, _targetSpeed, true);
		}
		else if (_previousHorizontalSpeed > _targetSpeed + _speedOffset)
		{
			VelocityChange(_previousHorizontalSpeed, _targetSpeed, false);
		}
		else
		{
			_currentSpeed = _targetSpeed;
		}
			
		// Make the input for the movement into a vector3
		_inputDirection += transform.right * _move.x + transform.forward * _move.y;
		_inputDirection.Normalize();

		// move the player
		_horizontalVelocity = _inputDirection * _currentSpeed;
		AdjustForSlope(ref _horizontalVelocity);
		MoveCharacter(_horizontalVelocity * Time.deltaTime);
	}

	private void VelocityChange(float _previousSpeed, float _targetSpeed, bool _isAccel)
	{
		float _speedChange;
		_speedChange = _isAccel ? _acceleration : _deceleration;

		// creates curved result rather than a linear one giving a more organic speed change
		// note T in Lerp is clamped, so we don't need to clamp our speed
		_currentSpeed = Mathf.Lerp(_previousSpeed, _targetSpeed * _move.magnitude, Time.deltaTime * _speedChange);
	}

	private void ManageGravity()
    {
        if (_grounded)
        {
			// reset the fall timeout timer
			_fallTimeoutDelta = _fallTimeout;

			// stop our velocity dropping infinitely when grounded
			if (_verticalVelocity < 0)
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

		MoveCharacter(Vector3.up * _verticalVelocity * Time.deltaTime);
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

	public void MoveCharacter(Vector3 movement)
	{
		_cumulatedMovement += movement;
	}

	private void ApplyMovement()
    {
		_controller.Move(_cumulatedMovement);
		_cumulatedMovement = Vector3.zero;
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

		//GROUNDED CHECK GIZMO
		//Vector3 _boxCenter = new(transform.position.x, transform.position.y + _groundedOffset, transform.position.z);
		Vector3 _boxCenter = new(transform.position.x, transform.position.y + _groundedCheckRadius + _groundedOffset - _groundedCastDistance, transform.position.z);

		Color transparentGreen = new(0.0f, 1.0f, 0.0f, 0.35f);
		Color transparentRed = new(1.0f, 0.0f, 0.0f, 0.35f);

		if (_grounded) Gizmos.color = transparentGreen;
		else Gizmos.color = transparentRed;

		//Gizmos.DrawCube(_boxCenter, new(_groundedBoxSize.x, _groundedBoxSize.y + _groundedCastDistance*2, _groundedBoxSize.z));
		Gizmos.DrawSphere(_boxCenter, _groundedCheckRadius);

		//EDGE CHECKS GIZMO
		Vector3 _spawnPoint = new(transform.position.x, transform.position.y + _edgeCheckOffset, transform.position.z);

		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(_spawnPoint, transform.forward * _edgeCheckLength);
		Gizmos.DrawRay(_spawnPoint, - transform.forward * _edgeCheckLength);
		Gizmos.DrawRay(_spawnPoint, transform.right * _edgeCheckLength);
		Gizmos.DrawRay(_spawnPoint, - transform.right * _edgeCheckLength);

		//SLOPE CHECK GIZMO
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, Vector3.down * _slopeCheckDistance);

		//UNCOMMENT THIS CODE TO SHOW THE DIRECTION OF THE INPUT + GRAVITY
		//Gizmos.color = Color.black;
		//Gizmos.DrawRay(new(transform.position.x, transform.position.y + _controller.center.y, transform.position.z), _finalInputMovement);
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
