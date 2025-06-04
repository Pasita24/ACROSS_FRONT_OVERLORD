using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private float moveSpeed = 3f; // Velocidad de patrulla
    [SerializeField] private float patrolDistance = 5f; // Distancia de patrulla
    [SerializeField] private LayerMask groundLayer; // Capa del suelo

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck; // Punto de verificaci�n del suelo
    [SerializeField] private float groundCheckRadius = 0.2f; // Radio de verificaci�n

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private bool movingRight = true;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    void Update()
    {
        // Verificar si est� en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        // Mover en la direcci�n actual
        float moveDirection = movingRight ? 1f : -1f;
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        // Verificar si ha alcanzado los l�mites de patrulla
        if (movingRight && transform.position.x >= startPosition.x + patrolDistance)
        {
            movingRight = false;
            Flip();
        }
        else if (!movingRight && transform.position.x <= startPosition.x - patrolDistance)
        {
            movingRight = true;
            Flip();
        }
    }

    void Flip()
    {
        // Invertir la direcci�n del sprite
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    // Visualizar el punto de verificaci�n del suelo en el editor
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}