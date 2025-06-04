using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Velocidad de movimiento horizontal
    [SerializeField] private float jumpForce = 10f; // Fuerza del salto
    [SerializeField] private LayerMask groundLayer; // Layer para detectar el suelo
    [SerializeField] private Transform groundCheck; // Punto para verificar el suelo
    [SerializeField] private float groundCheckRadius = 0.2f; // Radio de verificación del suelo

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Obtener entrada horizontal (izquierda/derecha)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Verificar si está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Saltar si está en el suelo y se presiona la tecla de salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Aplicar movimiento horizontal
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    // Opcional: Visualizar el área de verificación del suelo en el editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}