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
    private bool isDashing = false; // New variable to track dashing state
    public float playerSpeed = 5.0f;
    public float jumpHeight = 4.0f;
    public float gravityValue = 9.81f;
    public float dashSpeed = 10.0f; // Speed for dashing
    public float dashDuration = 0.5f; // Duration of dash in seconds

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
        verticalVelocity -= currentGravity * Time.deltaTime;

        // Gather lateral input control relative to camera
        Vector3 move = camera.transform.forward * Input.GetAxis("Vertical") + camera.transform.right * Input.GetAxis("Horizontal");

        // Restrict movement to X and Z axes (horizontal movement)
        move.y = 0;

        // Scale by speed
        move *= playerSpeed;

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
                verticalVelocity = Mathf.Sqrt(jumpHeight * 2 * gravityValue);
            }
            else
            {
                if (DoubleJump)
                {
                    DoubleJump = false;
                    verticalVelocity = Mathf.Sqrt(jumpHeight * 2 * gravityValue);
                }
            }
        }

        // Dash input handling
        if (Input.GetButtonDown("Fire3") && !isDashing)
        {
            StartCoroutine(Dash());
        }

        // Inject Y velocity before we use it
        move.y = verticalVelocity;

        // Call .Move() once only
        controller.Move(move * Time.deltaTime);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        float startTime = Time.time;

        // Dashing loop
        while (Time.time < startTime + dashDuration)
        {
            controller.Move(transform.forward * dashSpeed * Time.deltaTime); 
            
            yield return null;
        }

        isDashing = false;
    }
}
