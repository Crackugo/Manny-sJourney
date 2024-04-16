using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera camera;
    private CharacterController controller;
    private float verticalVelocity;
    private bool doubleJump = false;
    private bool isDashing = false;
    private bool canDash = false;
    private bool isCtrlPressed = false;
    private float originalMaxFallSpeed;
    private float ctrlPressedTime;
    public float playerSpeed = 5.0f;
    public float jumpHeight = 4.0f;
    public float gravityValue = 9.81f;
    public float maxFallSpeed = -20f; 
    public float dashSpeed = 10.0f; 
    public float dashDuration = 0.5f; 

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        originalMaxFallSpeed = maxFallSpeed;
    }

    void Update()
    {
        bool groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            canDash = true;
            doubleJump = true;
        }

        float currentGravity = gravityValue;

        verticalVelocity = Mathf.Clamp(verticalVelocity - currentGravity * Time.deltaTime, maxFallSpeed, Mathf.Infinity);

        verticalVelocity = Mathf.Max(verticalVelocity, maxFallSpeed);

        Vector3 move = camera.transform.forward * Input.GetAxis("Vertical") + camera.transform.right * Input.GetAxis("Horizontal");
        move.y = 0;
        move *= playerSpeed;

        if (move.magnitude > 0.05f)
        {
            transform.forward = move.normalized;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (groundedPlayer)
            {
                verticalVelocity = jumpHeight;
            }
            else
            {
                if (doubleJump)
                {
                    doubleJump = false;
                    verticalVelocity = jumpHeight;
                }
            }
        }

        // Check if Ctrl key is pressed
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            isCtrlPressed = true;
            ctrlPressedTime = Time.time;
            maxFallSpeed = -1f; // Set max fall speed to -1
        }

        // Reset max fall speed after 2 seconds or when touching the ground
        if (isCtrlPressed && (Time.time - ctrlPressedTime >= 2f || groundedPlayer))
        {
            isCtrlPressed = false;
            maxFallSpeed = originalMaxFallSpeed;
        }

        if (Input.GetButtonDown("Fire3") && !isDashing && canDash)
        {
            canDash = false;
            StartCoroutine(Dash());
        }
        if (isDashing)
        {
            verticalVelocity = 0;
        }
        move.y = verticalVelocity;
        controller.Move(move * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            Vector3 moveDash = transform.forward * dashSpeed * Time.deltaTime;
            moveDash.y = 0;

            controller.Move(moveDash);
            yield return null;
        }

        isDashing = false;
    }
}
