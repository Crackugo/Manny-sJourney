using System.Collections;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    // [Unity References]
    public Camera camera;
    private CharacterController controller;

    // [Movement Parameters]
    public float playerMaxSpeed = 10.0f;
    public float jumpSpeed = 4.0f;
    public float gravityValue = 9.81f;
    public float maxFallSpeed = -20f;
    public float dashSpeed = 10.0f;
    public float dashDuration = 0.5f;
    public float raycastDistance = 1f;

    // [Character State Flags]
    private bool doubleJump = false;
    private bool isDashing = false;
    private bool canFly = false;
    private bool canDash = false;
    private bool touchingWall = false;
    private bool isCtrlPressed = false;

    // [Input Handling Related Variables]
    private float ctrlPressedTime;

    // [Private Movement Variables]
    private float originalMaxFallSpeed;
    private float yVelocity;
    private float frontVelocity;
    private float sideVelocity;
    private float bonusV;
    private float lastVelocity = 0;
    private Vector3 move;

    public Animator animator;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        originalMaxFallSpeed = maxFallSpeed;
    }


    public GameObject bench; // Reference to the last bench touched by the player

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Bench")) // Assuming the benches have a tag named "Bench"
        {

            bench = other.gameObject; // Update the reference to the last bench touched
        }
        else if (other.CompareTag("RespawnBox")) // Assuming the respawn boxes have a tag named "RespawnBox"
        {
            if (bench != null)
            {
                TeleportPlayerToLastBench(); // Teleport the player to the last bench touched
            }
            else
            {
                Debug.LogWarning("No last bench touched!");
            }
        }
        else if (other.CompareTag("Spike"))
        {
            if (bench != null)
            {
                TeleportPlayerToLastBench();
            }
            else
            {
                Debug.LogWarning("No last bench touched!");
            }
        }
        else if (other.CompareTag("PowerUp"))
        {
            canDash = true;
        }
        else if (other.CompareTag("JumpPad"))
        {
            jumpSpeed = 50;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting collider belongs to the object you're interested in
        if (other.CompareTag("JumpPad"))
        {
            jumpSpeed = 20;
        }
    }

    private void TeleportPlayerToLastBench()
    {
        controller.enabled = false;
        controller.transform.position = bench.transform.position + new Vector3(0, 0, 0);
        move = new Vector3(0, 0, 0);
        yVelocity = 0;
        controller.enabled = true;
    }


    void Update()
    {
        bool groundedPlayer = (controller.collisionFlags & CollisionFlags.Below) != 0;
        float currentGravity = gravityValue;



        if (groundedPlayer)
        {

            animator.SetBool("isJumping", false);
            frontVelocity = 0;
            sideVelocity = 0;
            canDash = true;
            doubleJump = true;
            canFly = true;
            yVelocity = -1;
            if (bonusV > 0)
            {
                bonusV *= 0.9f;
            }
        }
        else
        {
            if (touchingWall && bonusV > 0 && Input.GetAxis("Vertical") <= 0.5)
            {
                bonusV *= 0.9f;

            }
            yVelocity = Mathf.Clamp(yVelocity - currentGravity * Time.deltaTime, maxFallSpeed, Mathf.Infinity);
            yVelocity = Mathf.Max(yVelocity, maxFallSpeed);
        }



        frontVelocity = Input.GetAxis("Vertical") * playerMaxSpeed + bonusV;
        sideVelocity = Input.GetAxis("Horizontal") * playerMaxSpeed + Input.GetAxis("Horizontal");

        if ((lastVelocity > frontVelocity) && bonusV > 0)
        {
            bonusV = lastVelocity - playerMaxSpeed;
            if (bonusV < 0)
            {
                bonusV = 0;
            }
            lastVelocity = frontVelocity;
        }


        if (Input.GetButtonDown("Jump"))
        {
            if (groundedPlayer)
            {
                animator.SetBool("isJumping", true);
                yVelocity = jumpSpeed;
            }
        }

        RaycastHit hitLeft;
        RaycastHit hitRight;
        if (Physics.Raycast(transform.position, -transform.right, out hitLeft, raycastDistance))
        {
            if (hitLeft.collider.CompareTag("Wall"))
            {
                touchingWall = true;
                maxFallSpeed = -1f;
                canDash = true;
            }
        }
        else if (Physics.Raycast(transform.position, transform.right, out hitRight, raycastDistance))
        {

            if (hitRight.collider.CompareTag("Wall"))
            {
                touchingWall = true;
                maxFallSpeed = -1f;
                canDash = true;

            }
        }
        else
        {
            if (touchingWall)
            {
                maxFallSpeed = originalMaxFallSpeed;
                touchingWall = false;
            }

        }


        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) && canFly)
        {
            if (yVelocity > 0)
            {
                yVelocity = yVelocity / 2;
            }
            bonusV = 0;
            isCtrlPressed = true;
            ctrlPressedTime = Time.time;
            maxFallSpeed = -1f; // Set max fall speed to -1
        }

        // Check if Ctrl key is released
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            isCtrlPressed = false;
            maxFallSpeed = originalMaxFallSpeed;
        }

        // If Ctrl key is continuously pressed for more than 2 seconds, reset max fall speed
        if (isCtrlPressed && (Time.time - ctrlPressedTime >= 2f))
        {
            canFly = false;
            isCtrlPressed = false;
            maxFallSpeed = originalMaxFallSpeed;
        }

        // Check if the player is grounded to reset max fall speed
        if (!isCtrlPressed && groundedPlayer)
        {
            maxFallSpeed = originalMaxFallSpeed;
        }

        if (Input.GetButtonDown("Fire3") && !isDashing && canDash)
        {
            canDash = false;
            if (Input.GetAxis("Vertical") == 1)
            {
                bonusV = 10;
            }
            StartCoroutine(Dash(groundedPlayer, transform.forward));
            frontVelocity = playerMaxSpeed + bonusV;
            lastVelocity = frontVelocity;
        }
        if (isDashing)
        {
            yVelocity = 0;
        }

        if (controller.collisionFlags == CollisionFlags.Above && yVelocity > 0)
        {
            yVelocity = 0;
        }

        Vector3 horizontalMove = camera.transform.forward * frontVelocity;
        Vector3 verticalMove = camera.transform.right * sideVelocity;
        move = horizontalMove + verticalMove;
        move.y = yVelocity;
        if (frontVelocity != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        controller.Move(move * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0);
    }



    IEnumerator Dash(bool ground, Vector3 dashDirection)
    {
        isDashing = true;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            if (Input.GetButtonDown("Jump") && ground)
            {
                if (ground)
                {
                    animator.SetBool("isJumping", true);
                    yVelocity = jumpSpeed;
                    doubleJump = true;
                }

                isDashing = false; // Exit dash
                canDash = true;
                break;
            }
            Vector3 moveDash = dashDirection * dashSpeed * Time.deltaTime;
            moveDash.y = 0;
            controller.Move(moveDash);
            yield return null;
        }

        isDashing = false;

    }



}