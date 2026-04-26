using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 8f;
    public float jumpPower = 16f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Header("Jump Setting")]
    public int maxJump = 2; // double jump

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isFacingRight = true;

    private int jumpCount = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // INPUT GERAK
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // INPUT LOMPAT
    //private void OnJump(InputValue value)
    //{
    //    if (value.isPressed && jumpCount < maxJump)
    //    {
    //        rb.velocity = new Vector2(rb.velocity.x, 0f); // reset dulu
    //        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    //        jumpCount++;
    //    }

    //    if (!value.isPressed && rb.velocity.y > 0f)
    //    {
    //        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
    //    }

    //    if (Keyboard.current.spaceKey.wasPressedThisFrame)
    //    {
    //        rb.AddForce(Vector2.up * 20f, ForceMode2D.Impulse);
    //    }
    //}

    private void Update()
    {
        // reset jump kalau nyentuh tanah
        if (IsGrounded())
        {
            jumpCount = 0;
        }
    }

    private void FixedUpdate()
    {
        // GERAK KIRI KANAN (TIDAK GANGGU GRAVITY)
        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);

        Flip();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
    }

    private void Flip()
    {
        if ((isFacingRight && moveInput.x < 0f) || (!isFacingRight && moveInput.x > 0f))
        {
            isFacingRight = !isFacingRight;

            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.5f);
        }
    }


}