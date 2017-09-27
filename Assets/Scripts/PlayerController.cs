using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
    public float speed = 6f;

    CharacterController characterController;
    InstagibController instagibController;
    CollisionFlags collisionFlags;

    public float gravityAcceleration = -2.0f;
    public float jumpAcceleration = 25.0f;
    public float linearDamping = 0.7f;
    public float superMarioScale = 8.0f;
    public float maxJumpAccelerationTime = 0.1f;

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
        characterController = GetComponent<CharacterController>();
        instagibController = GetComponentInChildren<InstagibController>();
        if (instagibController == null)
        {
            Debug.LogError("InstagibController has to be set");
        }
    }

    public void Move(Vector3 direction)
    {
        collisionFlags = characterController.Move(direction);
    }

    private void integrateMovement()
    {
        this.Move(currentVelocity);

        float gravityScale = currentVelocity.y < 0.0f ? superMarioScale : 1.0f;

        currentVelocity.y += gravityAcceleration * gravityScale * Time.deltaTime;
        currentVelocity *= linearDamping;
    }

    public bool isGrounded()
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

    public void Shot()
    {
        instagibController.Shoot();
    }
}
