using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Represents the player's movement and interaction with the environment.
/// </summary>
public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float movementSpeed = 1800f;
    [SerializeField] private float maxSpeed = 50f;
    [SerializeField] private float brakingSpeed = 2400f;
    [SerializeField] private float jumpForce = 270f;

    [SerializeField] private PlayerController playerController;

    private Rigidbody2D Rb;
    private bool ShouldAcc;
    private float Sign = 1;
    private bool IsGrounded = true;
    private float GravityScale;
    private Vector3 InitialPos;

    /// <summary>
    /// Initializes the Rigidbody2D component and the initial position of the player.
    /// </summary>
    private void Awake() {
        Rb = GetComponent<Rigidbody2D>();
        InitialPos = transform.position;
    }

    /// <summary>
    /// Handles collisions with other 2D colliders in the environment.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) {
            IsGrounded = true;
            playerController.canReload = true;
        } else if (other.gameObject.CompareTag("Wall")) {
            IsGrounded = true;
        }
        if (other.gameObject.CompareTag("Death")) {
            playerController.DropWeapon();
            transform.position = InitialPos;
        }
        playerController.canReload = other.gameObject.CompareTag("Ground");
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) {
            playerController.canReload = false;
        }
    }

    /// <summary>
    /// Handles the player's jump action.
    /// </summary>
    public void OnJump(InputAction.CallbackContext ctx) {
        if (!IsGrounded) {
            return;
        }
        Rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        IsGrounded = false;
        playerController.canReload = false;
    }

    /// <summary>
    /// Handles the player's movement action.
    /// </summary>
    public void OnMove(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            Sign = ctx.ReadValue<float>();
            ShouldAcc = true;
        } else if (ctx.canceled) {
            ShouldAcc = false;
            Rb.AddForce(new Vector2(-Sign * brakingSpeed, 0));
        }
    }

    /// <summary>
    /// Handles the player's crouch action.
    /// </summary>
    public void OnCrouch(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            var grScale = Rb.gravityScale;
            GravityScale = grScale;
            grScale *= 3;
            Rb.gravityScale = grScale;
        } else if (ctx.canceled) {
            Rb.gravityScale = GravityScale;
        }
    }

    private void HandleXMovement() {
        if (!ShouldAcc) {
            return;
        }
        var speedX = Rb.velocity.x;
        Vector2 force = new(movementSpeed * Sign, 0);
        if (speedX * speedX < maxSpeed * maxSpeed || force.x * speedX <= 0) {
            Rb.AddForce(force);
        }
    }

    private void FixedUpdate() {
        HandleXMovement();
    }
}
