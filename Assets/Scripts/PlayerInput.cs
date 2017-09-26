using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerController))]
public class PlayerInput : NetworkBehaviour {
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

    PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    void Update () {
        if (!isLocalPlayer) return;
        Vector3 direction = new Vector3(Input.GetAxisRaw(horizontalAxis), 0, Input.GetAxisRaw(verticalAxis));
        controller.Move(direction);
	}

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}
