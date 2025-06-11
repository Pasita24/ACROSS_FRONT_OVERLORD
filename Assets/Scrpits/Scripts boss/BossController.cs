using UnityEngine;
using HeneGames.DialogueSystem;

public class BossController : MonoBehaviour
{
    private enum BossState { Patrol, Idle, MovingRight }

    [Header("Patrol Settings")]
    [SerializeField] private float moveSpeed = 3f; // Velocidad de patrulla
    [SerializeField] private float patrolDistance = 5f; // Distancia de patrulla
    [SerializeField] private LayerMask groundLayer; // Capa del suelo

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck; // Punto de verificación del suelo
    [SerializeField] private float groundCheckRadius = 0.2f; // Radio de verificación

    [Header("Dialogue Settings")]
    [SerializeField] private DialogueUI dialogueUI; // Referencia al DialogueUI
    [SerializeField] private float moveSpeedRight = 2f; // Velocidad constante para moverse a la derecha

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private bool movingRight = true;
    private bool isGrounded;
    private BossState currentState = BossState.Patrol;
    private KeyCode originalActionInput;
    private bool isMovingRight;
    private Vector2 moveTargetPosition;
    private float moveDistance;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        if (dialogueUI != null)
        {
            originalActionInput = dialogueUI.actionInput; // Guardar la tecla original
        }
    }

    void Update()
    {
        // Verificar si está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Manejar el estado actual
        switch (currentState)
        {
            case BossState.Patrol:
                if (isGrounded)
                {
                    Patrol();
                }
                break;
            case BossState.Idle:
                rb.velocity = Vector2.zero; // Detener movimiento
                break;
            case BossState.MovingRight:
                UpdateMoveRight();
                break;
        }
    }

    void Patrol()
    {
        // Mover en la dirección actual
        float moveDirection = movingRight ? 1f : -1f;
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        // Verificar si ha alcanzado los límites de patrulla
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
        // Invertir la dirección del sprite
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void StopMovement()
    {
        ChangeState(BossState.Idle);
    }

    public void MoveRightAndDeactivate()
    {
        if (currentState != BossState.MovingRight)
        {
            ChangeState(BossState.MovingRight);
            LockDialogueInput();

            // Calcular la posición objetivo fuera de la cámara
            Camera mainCamera = Camera.main;
            float cameraRightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane)).x;
            moveTargetPosition = new Vector2(cameraRightEdge + 2f, transform.position.y); // 2 unidades más allá del borde
            moveDistance = Vector2.Distance(transform.position, moveTargetPosition);
            isMovingRight = true;
        }
    }

    private void UpdateMoveRight()
    {
        if (!isMovingRight) return;

        // Mover a velocidad constante
        Vector2 newPosition = Vector2.MoveTowards(transform.position, moveTargetPosition, moveSpeedRight * Time.deltaTime);
        rb.MovePosition(newPosition);

        // Verificar si el movimiento está completo
        if (Vector2.Distance(transform.position, moveTargetPosition) <= 0.01f)
        {
            isMovingRight = false;
            gameObject.SetActive(false); // Desactivar el boss
            UnlockDialogueInput();
        }
    }

    private void ChangeState(BossState newState)
    {
        currentState = newState;
    }

    public void LockDialogueInput()
    {
        if (dialogueUI != null)
        {
            dialogueUI.actionInput = KeyCode.None; // Desactivar la entrada
        }
    }

    public void UnlockDialogueInput()
    {
        if (dialogueUI != null)
        {
            dialogueUI.actionInput = originalActionInput; // Restaurar la tecla original
        }
    }

    // Visualizar el punto de verificación del suelo en el editor
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}