using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    
    [SerializeField] private float movementSpeed = 1800f;
    [SerializeField] private float maxSpeed = 50f;
    [SerializeField] private float brakingSpeed = 2400f;
    [SerializeField] private float jumpForce = 270f;

    [SerializeField] private PlayerController playerController;

    private Rigidbody2D rb;
    private bool shouldAcc;
    private float sign = 1;
    private bool isGrounded = true;
    private float gravityScale;
    private Vector3 initialPos;

 
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        initialPos = transform.position;
    }
    
    // check if on ground
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) {
            
            isGrounded = true;
            playerController.canReload = true;
            
            return;
        }
        if (other.gameObject.CompareTag("Wall")) {
            isGrounded = true;

            return;
        }
        if (other.gameObject.CompareTag("Death")) {
            playerController.DropWeapon();
            transform.position = initialPos;
        }
        playerController.canReload= false;
    }
    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) {
            // isGrounded = false, if want to disable jump after falling from surface, will break wall jumps also
            playerController.canReload = false;
        }
    }
    
    public void OnJump(InputAction.CallbackContext ctx) {
        
        if (!isGrounded)
            return;
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        isGrounded = false;
        playerController.canReload = false;
    }
    
    public void OnMove(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            sign = ctx.ReadValue<float>();
            shouldAcc = true;
        }
        else if (ctx.canceled) {
            shouldAcc = false;
            rb.AddForce(new Vector2(-sign*brakingSpeed,0));
        }
    }
    
    public void OnCrouch(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            float grScale = rb.gravityScale;
            gravityScale = grScale;
            grScale *= 3;
            rb.gravityScale = grScale;
        }
        if (ctx.canceled) {
            rb.gravityScale = gravityScale;
        }
        
    }
    
    private void HandleXMovement() {
        
        if (!shouldAcc) {
            return;
        }
        float speedX = rb.velocity.x;
        
        
        Vector2 force = new(movementSpeed * sign, 0);
        // if speed is less than max speed, or applied force is in opposite direction
        if (speedX * speedX < maxSpeed * maxSpeed || force.x * speedX <= 0 ) {
            rb.AddForce(force);
        }


    }
    private void FixedUpdate() {
        HandleXMovement();


    }
}

