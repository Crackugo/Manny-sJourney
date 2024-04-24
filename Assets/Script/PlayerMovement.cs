using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera camera;
    public HealthBar healthBar;
    private CharacterController controller;
    public float verticalVelocity;
    private bool doubleJump = false;
    private bool isDashing = false;
    private bool canDash = false;
    private bool isCtrlPressed = false;
    private bool canFly = false;
    private float originalMaxFallSpeed;
    private float ctrlPressedTime;
    public float playerSpeed = 5.0f;
    public float jumpSpeed = 4.0f;
    public float gravityValue = 9.81f;
    public float maxFallSpeed = -20f; 
    public float dashSpeed = 10.0f; 
    public float dashDuration = 0.5f; 
    public Vector3 move;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        originalMaxFallSpeed = maxFallSpeed;
    }


    public GameObject bench; // Reference to the last bench touched by the player

    private void OnTriggerEnter(Collider other){

        if (other.CompareTag("Projectile"))
        {
            
            Destroy(other.gameObject); // Destroy the projectile
            healthBar.takeDamage(20);
        }

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
        else if (other.CompareTag("Spike")){
            if (bench != null)
            {
                TeleportPlayerToLastBench();
            }
            else
            {
                Debug.LogWarning("No last bench touched!");
            }
        }   
    }

    private void TeleportPlayerToLastBench()
    {
        controller.enabled = false;
        controller.transform.position = bench.transform.position+new Vector3(0,0,0);
        move=new Vector3(0,0,0);
        verticalVelocity=0;    
        controller.enabled = true;
    }


    void Update()
    {
         bool groundedPlayer = controller.isGrounded;
        float currentGravity = gravityValue;
        if (groundedPlayer)
        {
            canDash = true;
            doubleJump = true;
            canFly=true;
            verticalVelocity=-1;
        }else{
            verticalVelocity = Mathf.Clamp(verticalVelocity - currentGravity * Time.deltaTime, maxFallSpeed, Mathf.Infinity);

            verticalVelocity = Mathf.Max(verticalVelocity, maxFallSpeed);
        }

        move = camera.transform.forward * Input.GetAxis("Vertical") + camera.transform.right * Input.GetAxis("Horizontal");
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
                verticalVelocity = jumpSpeed;
            }
            else
            {
                if (doubleJump)
                {
                    doubleJump = false;
                    verticalVelocity = jumpSpeed;
                }
            }
        }

       
        // Check if Ctrl key is pressed
        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))&&canFly)
        {
            isCtrlPressed = true;
            ctrlPressedTime = Time.time;
            maxFallSpeed = -1f; // Set max fall speed to -1
            playerSpeed/=1.5f;
        }

        // Check if Ctrl key is released
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            isCtrlPressed = false;
            maxFallSpeed = originalMaxFallSpeed;
            playerSpeed*=1.5f;
        }

        // If Ctrl key is continuously pressed for more than 2 seconds, reset max fall speed
        if (isCtrlPressed && (Time.time - ctrlPressedTime >= 2f))
        {   
            canFly=false;
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
            StartCoroutine(Dash(groundedPlayer,transform.forward ));
        }
        if (isDashing)
        {
            verticalVelocity = 0;
        }
        
        if (controller.collisionFlags == CollisionFlags.Above && verticalVelocity > 0)
        {
            // If there's a collision from below and the vertical velocity is positive,
            // set the vertical velocity to 0 to prevent further upward movement.
            verticalVelocity = 0;
        }
        move.y = verticalVelocity;
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
                if (ground){
                    verticalVelocity = jumpSpeed;
                    doubleJump=true;
                }

                isDashing = false; // Exit dash
                canDash=true;

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
