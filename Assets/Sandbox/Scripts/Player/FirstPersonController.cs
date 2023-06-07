using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GlobalParameters;

public class FirstPersonController : MonoBehaviour, IDataPersistence
{
	[Header("Player")]
	[Tooltip("Move speed of the character in m/s")]
	[SerializeField] float _moveSpeed = 4.0f;
	[Tooltip("Sprint speed of the character in m/s")]
	[SerializeField] float _sprintSpeed = 6.0f;
	[SerializeField] float _currentSpeed;
	public Vector3 _lastCheckpoint;

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
	[SerializeField] float _airboneDeceleration = 1.0f;

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

	[Space(10)]
	[Tooltip("0: Regular Weight \n 1: Heavy Weight \n -1: Light Weight")]
	[SerializeField] int _playerWeight = 0;


	[Header("Player On Ice")]
	[SerializeField] bool _onIce = false;
	[SerializeField] float _iceSpeedMultiplier = 2f;
	[SerializeField] float _iceAcceleration = 5;
	[SerializeField] float _iceDeceleration = 5;


	[Header("Player In Water")]
	[SerializeField] bool _inWater = false;
	[SerializeField] bool _underWater = false;
	[SerializeField] LayerMask _waterLayers;


	[Header("Cinemachine")]
	[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
	[SerializeField] GameObject _cinemachineCameraTarget;
	[Tooltip("How far in degrees can you move the camera up")]
	[SerializeField] float _topClamp = 90.0f;
	[Tooltip("How far in degrees can you move the camera down")]
	[SerializeField] float _bottomClamp = -90.0f;
	// cinemachine

	[SerializeField] GameObject _dmgImage;
	float _cinemachineTargetPitch;

	// player
	float _targetSpeed;
	float _acceleration;
	float _deceleration;
	Vector3 _inputDirection;
	Vector3 _cumulatedMovement;
	bool _touchingWater;
	bool _inHeatZone;
	float _waterCheckRadius = .01f;
	float _speedChangeRate;
	float _heatPercentage;
	// Timers
	float _fallTimeoutDelta;

	// Components
	CharacterController _controller;
	ImpactReceiver _impactReceiver;
	PlayerInputController _inputController;
	DmgEffect _dmgEffect;

	// Getter and setters
	public int PlayerWeight
    {
        get { return _playerWeight; }
		private set { _playerWeight = Mathf.Clamp(value, -1, 1); }
    }
	public bool Grounded
    {
        get { return _grounded; }
    }
	public bool OnIce
    {
        get { return _onIce; }
    }
	public bool InWater
    {
        get { return _inWater; }
    }
	public Vector3 CumulatedMovement
    {
        get { return _cumulatedMovement; }
    }


	private void Awake()
	{
		_controller = GetComponent<CharacterController>();
		_impactReceiver = GetComponent<ImpactReceiver>();
		_inputController = GetComponent<PlayerInputController>();
		_dmgEffect = _dmgImage.GetComponent<DmgEffect>();
	}

	void Start()
    {
		_impactReceiver.ChangeMass(PlayerWeight);
		_heatPercentage = 0.00f;
		_inHeatZone = false;
		_fallTimeoutDelta = _fallTimeout; // reset our timeouts on start
	}

    void Update()
    {
		//Checks
		GroundCheck();
		IceCheck();
		WaterCheck();
		HeatZoneCheck();

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

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
			case WATER_TAG:
				_touchingWater = true;
				break;
			case HEAT_ZONE_TAG:
				Debug.Log("Entrando zona de calor");
				_inHeatZone = true;
				break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
		switch (other.tag)
		{
			case WATER_TAG:
				_touchingWater = false;
				break;
			case HEAT_ZONE_TAG:
				Debug.Log("Saliendo zona de calor");
				_inHeatZone = false;
				break;
		}
	}

    private void GroundCheck()
    {
		Vector3 _boxCenter = new(transform.position.x, transform.position.y + _groundedCheckRadius + _groundedOffset, transform.position.z);
		_grounded = Physics.SphereCast(_boxCenter, _groundedCheckRadius, Vector3.down, out _, _groundedCastDistance, _groundLayers, QueryTriggerInteraction.Ignore);

        if (!_grounded && _controller.velocity.y <= 0 && !_inWater)
        {
			SlipCheck(); //Check if stuck on edge
        }
	}

	private void WaterCheck()
    {
        if (_touchingWater)
        {
			Vector3 _spawnIn = new(transform.position.x, transform.position.y + _controller.center.y, transform.position.z);
			_inWater = Physics.CheckSphere(_spawnIn, _waterCheckRadius, _waterLayers, QueryTriggerInteraction.Collide);

			Vector3 _spawnUnder = new(transform.position.x, transform.position.y + _controller.center.y + (_waterCheckRadius * 2) + .03f, transform.position.z);
			_underWater = Physics.CheckSphere(_spawnUnder, _waterCheckRadius, _waterLayers, QueryTriggerInteraction.Collide);

			if(_inWater) WaterLogic();
        }
    }

	private void HeatZoneCheck(){
		if(_inHeatZone)
		{
			if( _heatPercentage < 1.0f)
			{
				_heatPercentage += 0.01f;
			}
		} else 
		{
			if( _heatPercentage> 0.0f)
			{
				_heatPercentage -= 0.01f;
			}
		}
		_dmgEffect.ChangeColor(_heatPercentage);
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

	private void IceCheck()
    {
		float _radius = .01f;
		float _offset = .09f;
		Vector3 _pos = new(transform.position.x, transform.position.y + _radius - _offset, transform.position.z);
		
		if (Physics.CheckSphere(_pos, .01f, 1 << ICE_LAYER, QueryTriggerInteraction.Collide) && (_grounded))
		{
			_onIce = true;
		}
		else
		{
			_onIce = false;
		}
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
		SetSpeedAndAcceleration();

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
		_inputDirection += transform.right * _inputController.Move.x + transform.forward * _inputController.Move.y;
		_inputDirection.Normalize();

        // move the player
        if (_grounded)
        {
			_horizontalVelocity = _inputDirection * _currentSpeed;
			AdjustForSlope(ref _horizontalVelocity);
		}
        else 
        {
			Vector3 _airVelocity = _inputDirection * _targetSpeed;
			_horizontalVelocity = Vector3.Lerp(_horizontalVelocity, _airVelocity, Time.deltaTime * _speedChangeRate);
		}
		
		MoveCharacter(_horizontalVelocity * Time.deltaTime);
	}

	private void VelocityChange(float _previousSpeed, float _targetSpeed, bool _isAccel)
	{
		_speedChangeRate = _isAccel ? _acceleration : _deceleration;
		
		_currentSpeed = Mathf.Lerp(_previousSpeed, _targetSpeed * _inputController.Move.magnitude, Time.deltaTime * _speedChangeRate);
	}

	private void SetSpeedAndAcceleration()
	{
		if (_grounded)
		{
			_targetSpeed = _inputController.Sprint ? _sprintSpeed : _moveSpeed;
			if (_onIce)
			{
				_targetSpeed *= _iceSpeedMultiplier;
				_acceleration = _iceAcceleration;
				_deceleration = _iceDeceleration;
			}
			else
			{
				_acceleration = _groundedAcceleration;
				_deceleration = _groundedDeceleration;
			}
		}
		// On air
		else
		{
			_targetSpeed = _currentSpeed > _moveSpeed ? _currentSpeed : _moveSpeed;
			_acceleration = _airboneAcceleration;
			_deceleration = _airboneDeceleration;
		}

		// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
		// if there is no input, set the target speed to 0
		if (_inputController.Move == Vector2.zero) _targetSpeed = 0.0f;
	}

	private void ManageGravity()
    {
		// Keep player on water surface when not heavy
        if (_inWater && (PlayerWeight != 1))
        {
			if (_verticalVelocity < 0)
			{
				_verticalVelocity = 0;
			}
		}
        else if (_grounded)
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

		MoveCharacter(_verticalVelocity * Time.deltaTime * Vector3.up);
    }

	private void WaterLogic()
    {
        if ((PlayerWeight != 1) && _underWater)
        {
			//Float back to surface
			_verticalVelocity = 4f;
        }
		else if (PlayerWeight != 1)
        {
			//Don't wait for next gravity update to stop; prevents bouncing on surface
			_verticalVelocity = 0;
        }
    }

	private void ManageJump()
    {
		bool _groundJump = _grounded && !_inWater;
		bool _underWaterJump = _grounded && (PlayerWeight == 1);
		bool _waterJump = _inWater && (PlayerWeight != 1) && !_underWater;

		if (_groundJump || _underWaterJump || _waterJump)
        {
			// Jump
			if (_inputController.Jump)
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

			_inputController.Jump = false;
		}
	}

    private void CameraRotation()
    {
        if (!GameManager.Instance.GamePaused)
        {
			_cinemachineTargetPitch += _inputController.Look.y;

			// clamp our pitch rotation
			_cinemachineTargetPitch = Mathf.Clamp(_cinemachineTargetPitch, _bottomClamp, _topClamp);

			// Update Cinemachine camera target pitch
			_cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

			// rotate the player left and right
			transform.Rotate(Vector3.up * _inputController.Look.x);
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

	public void ChangeWeight(InputAction.CallbackContext ctx)
    {
		PlayerWeight += (int) ctx.ReadValue<float>();

		_impactReceiver.ChangeMass(PlayerWeight);
    }

	public void StopMovement()
    {
		_verticalVelocity = 0;
		_horizontalVelocity = Vector3.zero;
    }

	/////////////////////////////////////////////////////////////////////////////////////////////////////

	private void OnDrawGizmosSelected()
    {

		//GROUNDED CHECK GIZMO
		Vector3 _boxCenter = new(transform.position.x, transform.position.y + _groundedCheckRadius + _groundedOffset - _groundedCastDistance, transform.position.z);

		Color transparentGreen = new(0.0f, 1.0f, 0.0f, 0.35f);
		Color transparentRed = new(1.0f, 0.0f, 0.0f, 0.35f);

		if (_grounded) Gizmos.color = transparentGreen;
		else Gizmos.color = transparentRed;

		Gizmos.DrawSphere(_boxCenter, _groundedCheckRadius);

		//EDGE CHECKS GIZMO
		Vector3 _edgeSpawnPoint = new(transform.position.x, transform.position.y + _edgeCheckOffset, transform.position.z);

		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(_edgeSpawnPoint, transform.forward * _edgeCheckLength);
		Gizmos.DrawRay(_edgeSpawnPoint, - transform.forward * _edgeCheckLength);
		Gizmos.DrawRay(_edgeSpawnPoint, transform.right * _edgeCheckLength);
		Gizmos.DrawRay(_edgeSpawnPoint, - transform.right * _edgeCheckLength);

		//SLOPE CHECK GIZMO
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, Vector3.down * _slopeCheckDistance);

        //WATER CHECK GIZMO
        if (_touchingWater)
        {
			Gizmos.color = Color.yellow;
			Vector3 _inWaterCheckPos = new(transform.position.x, transform.position.y + _controller.center.y, transform.position.z);
			Gizmos.DrawSphere(_inWaterCheckPos, _waterCheckRadius);
			
			Gizmos.color = Color.red;
			Vector3 _underWaterCheckPos = new(transform.position.x, transform.position.y + _controller.center.y + (_waterCheckRadius * 2) + .03f, transform.position.z);
			Gizmos.DrawSphere(_underWaterCheckPos, _waterCheckRadius);
        }

		//ICE CHECK GIZMO
		Gizmos.color = Color.red;
		float _iceCheckRadius = .01f;
		float _iceCheckOffset = .09f;
		Vector3 _iceCheckPos = new(transform.position.x, transform.position.y + _iceCheckRadius - _iceCheckOffset, transform.position.z);
		Gizmos.DrawSphere(_iceCheckPos, _iceCheckRadius);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void LoadData(GameData data)
    {
		this._lastCheckpoint = new(data.LastCheckpoint[0], data.LastCheckpoint[1], data.LastCheckpoint[2]);

		_controller.enabled = false;
		this.transform.position = this._lastCheckpoint;
		_controller.enabled = true;
    }

    public void SaveData(ref GameData data)
    {
		data.LastCheckpoint = new float[] { _lastCheckpoint.x, _lastCheckpoint.y, _lastCheckpoint.z};
    }
}
