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

    PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    void Update () {
        if (!isLocalPlayer) return;
        Vector3 direction = new Vector3(Input.GetAxisRaw(horizontalAxis), 0, Input.GetAxisRaw(verticalAxis));
        controller.Move(direction * controller.speed * Time.deltaTime);

        if (Input.GetAxisRaw("Fire1") == 1)
        {
            controller.Shot();
        }

        bool jumpDown = Input.GetAxisRaw("Jump") == 1;
        bool grounded = controller.isGrounded();

        bool isFirstPress = jumpDown && !jumpButtonPressedLastFrame;

        if (isFirstPress && grounded)
        {
            timePassedSinceFirstButtonPress = 0.0f;
        }

        if (jumpDown && timePassedSinceFirstButtonPress < this.controller.maxJumpAccelerationTime)
        {
            this.controller.Jump();
        }

        timePassedSinceFirstButtonPress += Time.deltaTime;
        jumpButtonPressedLastFrame = jumpDown;
	}

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}
