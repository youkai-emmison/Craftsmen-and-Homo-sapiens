// Script purpose: Controls side-scrolling player movement and jumping.
// Key Inspector variables:
// - moveSpeed: Horizontal movement speed.
// - jumpForce: Upward jump velocity.
// - groundChecker: Ground check component that decides whether jumping is allowed.
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Horizontal speed applied from left/right input.
    public float moveSpeed = 5f;

    // Vertical velocity applied when the player jumps.
    public float jumpForce = 12f;

    // Required reference used to prevent infinite jumping.
    public GroundChecker groundChecker;

    // Rigidbody2D moved by this component.
    private Rigidbody2D playerRigidbody;

    // Cached horizontal input read in Update and applied in FixedUpdate.
    private float horizontalInput;

    // Stores a jump button press until the next physics tick.
    private bool jumpRequested;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        ValidateRequiredReferences();
    }

    private void Update()
    {
        // Read input in Update so short key presses are not missed between physics ticks.
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        // Rigidbody2D movement belongs in FixedUpdate to keep physics stable.
        if (!HasRequiredReferences())
        {
            jumpRequested = false;
            return;
        }

        ApplyHorizontalMovement();
        HandleJumpRequest();
    }

    private void ValidateRequiredReferences()
    {
        if (playerRigidbody == null)
        {
            Debug.LogError("PlayerMovement: This GameObject needs a Rigidbody2D component for side-scrolling movement.", this);
        }

        if (groundChecker == null)
        {
            Debug.LogError("PlayerMovement: GroundChecker is not assigned. Add a GroundChecker component and drag it into this field.", this);
        }
    }

    private bool HasRequiredReferences()
    {
        return playerRigidbody != null && groundChecker != null;
    }

    private void ApplyHorizontalMovement()
    {
        Vector2 currentVelocity = playerRigidbody.velocity;
        currentVelocity.x = horizontalInput * moveSpeed;
        playerRigidbody.velocity = currentVelocity;
    }

    private void HandleJumpRequest()
    {
        if (!jumpRequested)
        {
            return;
        }

        if (groundChecker.IsGrounded)
        {
            // GroundChecker prevents infinite jumping while the player is airborne.
            Jump();
        }

        jumpRequested = false;
    }

    private void Jump()
    {
        Vector2 currentVelocity = playerRigidbody.velocity;
        currentVelocity.y = jumpForce;
        playerRigidbody.velocity = currentVelocity;
    }
}
