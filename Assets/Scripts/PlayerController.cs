using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
    public float speed = 6f;

    CharacterController characterController;
    CollisionFlags collisionFlags;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void Move(Vector3 direction)
    {
        Vector3 motion = direction * speed * Time.deltaTime;
        collisionFlags = characterController.Move(motion);
    }

    public void Jump()
    {
        // TODO
    }
}
