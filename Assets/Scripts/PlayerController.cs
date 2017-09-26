using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
    public float speed = 6f;

    CharacterController characterController;
    InstagibController instagibController;
    CollisionFlags collisionFlags;

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
        Vector3 motion = direction * speed * Time.deltaTime;
        collisionFlags = characterController.Move(motion);
    }

    public void Jump()
    {
        // TODO
    }

    public void Shot()
    {
        instagibController.Shoot();
    }
}
