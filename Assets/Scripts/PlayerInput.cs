using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerController))]
public class PlayerInput : NetworkBehaviour {
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

    private bool jumpButtonPressedLastFrame = false;
    private float timePassedSinceFirstButtonPress = 0.0f;

    private float sensitivityX = 15.0F;
    private float sensitivityY = 15.0F;

	private PlayerController _controller;
	private Hud _hud;

    void Start()
    {
        _controller = GetComponent<PlayerController>();
		_hud = GetComponent<Hud>();
            
        LockMouseCursorToWindow();
    }

    void Update () {
        if (!isLocalPlayer)
        {
            return;
        }

        if (Cursor.lockState != CursorLockMode.None) { 
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
            float rotationY = Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = 0; // deactivated look up/down
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }

        Vector3 direction = new Vector3(
            Input.GetAxisRaw(horizontalAxis), 
            0, 
            Input.GetAxisRaw(verticalAxis)
        );
        direction = Quaternion.Euler(0, transform.localEulerAngles.y, 0) * direction;
        _controller.Move(direction * _controller.speed * Time.deltaTime);

        if (Input.GetAxisRaw("Fire1") == 1)
        {
			CmdFireWeapon();
        }

        Jump();

        if (Input.GetAxisRaw("Fire2") == 1)
        {
            LockMouseCursorToWindow();
        }
        if (Input.GetKeyDown("escape"))
        { 
            UnlockMouseCursorWhenKeyPressed();
        }

		if(Input.GetKeyDown(KeyCode.Tab))
		{
			ShowScoreboard(true);
		}
		if(Input.GetKeyUp(KeyCode.Tab))
		{
			ShowScoreboard(false);
		}
    }

    private void Jump()
    {
        bool jumpDown = Input.GetAxisRaw("Jump") == 1;
        bool grounded = _controller.isGrounded();

        bool isFirstPress = jumpDown && !jumpButtonPressedLastFrame;

        if (isFirstPress && grounded)
        {
            timePassedSinceFirstButtonPress = 0.0f;
        }

        if (jumpDown && timePassedSinceFirstButtonPress < this._controller.maxJumpAccelerationTime)
        {
            this._controller.Jump();
        }

        timePassedSinceFirstButtonPress += Time.deltaTime;
        jumpButtonPressedLastFrame = jumpDown;
    }

    private void LockMouseCursorToWindow()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UnlockMouseCursorWhenKeyPressed()
    {
        Cursor.lockState = CursorLockMode.None;
    }

	[Command]
	private void CmdFireWeapon()
	{
		_controller.Shoot();
	}

	private void ShowScoreboard(bool isShown)
	{
		_hud.ShowScoreboard(isShown);
	}
}
