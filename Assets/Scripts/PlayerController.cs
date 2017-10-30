using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour {
    public float speed = 6f;

    private CharacterController characterController;
    private InstagibController instagibController;

    public float gravityAcceleration = -2.0f;
    public float jumpAcceleration = 25.0f;
    public float linearDamping = 0.7f;
    public float superMarioScale = 8.0f;
    public float maxJumpAccelerationTime = 0.1f;

	[SyncVar]
	private string _playerId;

	private ScoreManager _scoreManager;
	private CustomNetworkManager _networkManager;
	private bool _isEnabled;

    private Vector3 currentVelocity = Vector3.zero;

    private Vector3 BottomPosition
    {
        get
        {
            Vector3 bottomPos = this.transform.position;
            bottomPos.y -= this.characterController.height * 0.5f;

            return bottomPos;
        }
    }

    private void Start()
    {
        GetComponentInChildren<Camera>().enabled = isLocalPlayer;
        GetComponentInChildren<AudioListener>().enabled = isLocalPlayer;
        characterController = GetComponent<CharacterController>();
        instagibController = GetComponentInChildren<InstagibController>();
        if (instagibController == null)
        {
            Debug.LogError("InstagibController has to be set");
        }
		_scoreManager = GameObject.FindObjectOfType<ScoreManager>();
		_networkManager = GameObject.FindObjectOfType<CustomNetworkManager>();
		RequestPlayerName();
		Enable(true);
    }

    public void Move(Vector3 direction)
    {
        characterController.Move(direction);
    }

    private void integrateMovement()
    {
        this.Move(currentVelocity);

        float gravityScale = currentVelocity.y < 0.0f ? superMarioScale : 1.0f;

        currentVelocity.y += gravityAcceleration * gravityScale * Time.deltaTime;
        currentVelocity *= linearDamping;
    }

    public bool IsGrounded()
    {
        const float rayMaxDistance = 0.2f;

        Ray r = new Ray();
        r.origin = this.BottomPosition;
        r.direction = Vector3.down;

        return Physics.Raycast(r, rayMaxDistance);
    }

    public void Update()
    {
        this.integrateMovement();
    }

    public void Jump()
    {
        currentVelocity.y += jumpAcceleration * Time.deltaTime;
    }

    public void Shoot()
    {
        instagibController.Shoot();
    }

	[Command]
	private void CmdAssignPlayerName(string name)
	{
        _playerId = Guid.NewGuid().ToString();

        _scoreManager.RegisterPlayer(_playerId, name);
	}

	private void RequestPlayerName()
	{
        if (!isLocalPlayer)
        {
            return;
        }
		var name = _networkManager.GetUsername();

		CmdAssignPlayerName(name);
	}
		
	public string GetPlayerId()
	{
		return _playerId;
	}

	[ClientRpc]
	public void RpcEnable(bool isEnabled)
	{
        Enable(isEnabled);
	}

    public void Enable(bool isEnabled)
    {
        _isEnabled = isEnabled;
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.enabled = _isEnabled;
        }
    }

    public bool IsEnabled()
	{
		return _isEnabled;
	}
}
