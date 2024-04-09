using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera camera;
    private CharacterController controller;
    private float verticalVelocity;
    private float groundedTimer;
    private bool DoubleJump = false;
    public float playerSpeed = 5.0f;
    public float jumpHeight = 4.0f;
    public float originalGravity = 9.81f;
    private float gravityValue = 9.81f;
    private bool isFalling = false;

    private void Start()
    {
        // always add a controller
        controller = gameObject.AddComponent<CharacterController>();
    }

    void Update()
    {
        bool groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            DoubleJump = true;
            // cooldown interval to allow reliable jumping even when coming down ramps
            groundedTimer = 0.2f;
            isFalling = false;
        }
        if (groundedTimer > 0)
        {
            groundedTimer -= Time.deltaTime;
        }

        // slam into the ground
        if (groundedPlayer && verticalVelocity < 0)
        {
            // hit ground
            verticalVelocity = 0f;
        }

        // Apply gravity
        float currentGravity = gravityValue;
        if (!groundedPlayer && verticalVelocity < 0)
        {
            // Reduce gravity only when falling and shift is pressed
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentGravity *= 0.1f;
            }
            isFalling = true;
        }

        verticalVelocity -= currentGravity * Time.deltaTime;

        // Gather lateral input control relative to camera
        Vector3 move = camera.transform.forward * Input.GetAxis("Vertical") + camera.transform.right * Input.GetAxis("Horizontal");

        // Restrict movement to X and Z axes (horizontal movement)
        move.y = 0;

        // Scale by speed
        move *= playerSpeed;

        // Increase movement speed when falling and shift is pressed
        if (!isFalling && Input.GetKey(KeyCode.LeftShift))
        {
            move *= 1.5f; // Doubling the movement speed
        }

        // Only align to motion if we are providing enough input
        if (move.magnitude > 0.05f)
        {
            // Align with the direction of movement
            transform.forward = move.normalized;
        }

        // Allow jump as long as the player is on the ground
        if (Input.GetButtonDown("Jump"))
        {
            // Must have been grounded recently to allow jump
            if (groundedTimer > 0)
            {
                // No more until we recontact ground
                groundedTimer = 0;

                // Physics dynamics formula for calculating jump up velocity based on height and gravity
                verticalVelocity = Mathf.Sqrt(jumpHeight * 2 * originalGravity);
            }
            else
            {
                if (DoubleJump)
                {
                    DoubleJump = false;
                    verticalVelocity = Mathf.Sqrt(jumpHeight * 2 * originalGravity);
                }
            }
        }

        // Inject Y velocity before we use it
        move.y = verticalVelocity;

        // Call .Move() once only
        controller.Move(move * Time.deltaTime);
    }
}
