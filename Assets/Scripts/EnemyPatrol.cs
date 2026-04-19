using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool movingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movingRight ? speed : -speed, rb.velocity.y);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, 1f, groundLayer);

        Debug.Log(movingRight);
        Debug.DrawRay(groundCheck.position, Vector2.down * 1f, Color.red);

        if (groundInfo.collider == null)
        {
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kalau nabrak tembok / object → balik arah
        if (collision.collider.CompareTag("Ground"))
        {
            Flip();
        }
    }

    void Flip()
    {
        movingRight = !movingRight;

        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}