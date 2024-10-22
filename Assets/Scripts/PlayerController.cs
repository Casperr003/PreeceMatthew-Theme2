using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isPlayerOne = true;

    public LayerMask groundLayer;     // Layer for ground
    public LayerMask doorLayer;       // Layer for regular doors
    public LayerMask finalDoorLayer;  // Layer for the FinalDoor

    private Rigidbody2D rb;
    private bool facingRight = true;
    private InputSystem_Actions inputActions;
    private Vector2 moveInput;

    // Reference to the UI controller
    private UIcontroller uiController;

    // Track if the player is currently collecting a key
    private bool isCollectingKey = false;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();

        if (isPlayerOne)
        {
            inputActions.Player1.Enable();
            inputActions.Player1.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            inputActions.Player1.Move.canceled += ctx => moveInput = Vector2.zero;
        }
        else
        {
            inputActions.Player2.Enable();
            inputActions.Player2.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            inputActions.Player2.Move.canceled += ctx => moveInput = Vector2.zero;
            inputActions.Player2.Collect.performed += ctx => TryCollectKey(); // Bind key collection to East button
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        // Find the UI controller in the scene
        uiController = FindObjectOfType<UIcontroller>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 targetPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;

        // Check for collisions with ground and doors
        if (!IsCollidingWithGround(targetPosition) && !IsCollidingWithDoors(targetPosition) && !IsCollidingWithFinalDoor(targetPosition))
        {
            rb.MovePosition(targetPosition);
        }

        // Flip player sprite based on direction
        if (moveInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && facingRight)
        {
            Flip();
        }
    }

    private bool IsCollidingWithGround(Vector2 targetPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, targetPosition - rb.position, Vector2.Distance(rb.position, targetPosition), groundLayer);
        return hit.collider != null;
    }

    private bool IsCollidingWithDoors(Vector2 targetPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, targetPosition - rb.position, Vector2.Distance(rb.position, targetPosition), doorLayer);

        // If Player 1 collides with any door, prevent movement
        if (isPlayerOne && hit.collider != null)
        {
            return true; // Player 1 cannot pass through any doors
        }

        // If Player 2 collides with a door, allow passage
        return false; // Player 2 can pass through doors
    }

    private bool IsCollidingWithFinalDoor(Vector2 targetPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, targetPosition - rb.position, Vector2.Distance(rb.position, targetPosition), finalDoorLayer);
        return hit.collider != null; // Block movement if colliding with FinalDoor
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // Method to collect keys
    public void CollectKey()
    {
        // Call the UpdateKeyCount method from the UIcontroller to increment the key count
        if (uiController != null)
        {
            uiController.UpdateKeyCount(1); // Increment key count by 1
        }
    }

    // Set collecting key status
    public void SetCollectingKey(bool value)
    {
        isCollectingKey = value;
    }

    // Attempt to collect key
    private void TryCollectKey()
    {
        if (isCollectingKey)
        {
            CollectKey();
            isCollectingKey = false; // Reset collecting key status after collection
        }
    }

    private void OnDrawGizmos()
    {
        // Gizmos drawing logic if necessary
    }
}
