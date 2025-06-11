using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Velocidad de movimiento horizontal
    [SerializeField] private float jumpForce = 10f; // Fuerza del salto
    [SerializeField] private LayerMask groundLayer; // Layer para detectar el suelo
    [SerializeField] private Transform groundCheck; // Punto para verificar el suelo
    [SerializeField] private float groundCheckRadius = 0.2f; // Radio de verificaci�n del suelo

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private bool mirandoDerecha = true;

    [Header("Animacion")]
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Obtener entrada horizontal (izquierda/derecha)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Verificar si est� en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Saltar si est� en el suelo y se presiona la tecla de salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Aplicar movimiento horizontal
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        // Girar sprite según la dirección del movimiento
        if (moveInput > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (moveInput < 0 && mirandoDerecha)
        {
            Girar();
        }
    }

    // Opcional: Visualizar el �rea de verificaci�n del suelo en el editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
    public void OnDetected()
    {
        // Reproducir animación
        if (anim != null)
        {
            //anim.SetTrigger("Detectado"); //Una vez se tenga una animiacion definida
            GetComponent<SpriteRenderer>().color = Color.red; // De momento solo usar esta
        }

        // Cancelar movimiento del jugador
        moveInput = 0f;
        rb.velocity = Vector2.zero;
        this.enabled = false; // Detiene futuras lecturas de input
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
}