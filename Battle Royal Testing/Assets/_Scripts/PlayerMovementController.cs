using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour {

    private float InputX, InputZ, Speed, gravity;
    private Camera cam;
    private CharacterController characterController;

    private Vector3 desiredMoveDirection;

    [SerializeField]
    float rotationSpeed = 0.3f;

    [SerializeField]
    float allowRotation = 0.1f;

    [SerializeField]
    float moveSpeed = 1f;

    [SerializeField]
    float gravityMultiplier = 1f;

    // Use this for initialization
    void Start () {
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        InputDecider();
        MovementManager();
	}    

    void InputDecider() {
        Speed = new Vector2(InputX, InputZ).sqrMagnitude;

        if (Speed > allowRotation)
        {
            RotationManager();
        }
        else {
            desiredMoveDirection = Vector3.zero;
        }
    }

    void RotationManager()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputZ + right * InputX;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);
    }

    void MovementManager()
    {
        gravity -= 9.8f * Time.deltaTime;
        gravity = gravity * gravityMultiplier;
        Vector3 moveDir = desiredMoveDirection * moveSpeed * Time.deltaTime;
        moveDir = new Vector3(moveDir.x, gravity, moveDir.z);
        characterController.Move(moveDir);

        if (characterController.isGrounded) {
            gravity = 0;
        }
    }
}
