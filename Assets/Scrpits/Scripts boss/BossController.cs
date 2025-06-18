using UnityEngine;
using HeneGames.DialogueSystem;

public class BossController : MonoBehaviour
{
    private enum BossState { Patrol, Idle, MovingRight, MachineGun, Resting }

    [Header("Patrol Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float patrolDistance = 5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;

    [Header("Dialogue Settings")]
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private float moveSpeedRight = 2f;

    [Header("Machine Gun Settings")]
    [SerializeField] private Camera bossCamera;
    [SerializeField] private Transform punto1;
    [SerializeField] private ParticleSystem shootingParticle; // Añadido

    [Header("Fatiga Settings")]
    [SerializeField] private float shootingDuration = 10f;
    [SerializeField] private float restDuration = 5f;
    [SerializeField] private GameObject weaponObject; // Objeto con Animator
    [SerializeField] private string shootingAnimParam = "IsShooting";

    private float shootingTimer;
    private float restTimer;
    private bool isShootingEnabled;
    public bool IsShootingEnabled => isShootingEnabled;
    private Animator weaponAnimator;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private bool movingRight = true;
    private bool isGrounded;
    private BossState currentState = BossState.Patrol;
    private KeyCode originalActionInput;
    private bool isMovingRight;
    private Vector2 moveTargetPosition;
    private float moveDistance;


    public static BossController Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        if (dialogueUI != null)
        {
            originalActionInput = dialogueUI.actionInput;
        }
        if (weaponObject != null)
        {
            weaponAnimator = weaponObject.GetComponent<Animator>();
        }
        shootingTimer = shootingDuration;
    }

    void Update()
    {
        // Verificar si está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Manejar fatiga solo en estado MachineGun
        if (currentState == BossState.MachineGun)
        {
            if (isShootingEnabled)
            {
                shootingTimer -= Time.deltaTime;
                if (shootingTimer <= 0f)
                {
                    StartResting();
                }
            }
            else
            {
                restTimer -= Time.deltaTime;
                if (restTimer <= 0f)
                {
                    ResumeShooting();
                }
            }
        }


        // Manejar el estado actual
        switch (currentState)
        {
            case BossState.Patrol:
                if (isGrounded) Patrol();
                break;
            case BossState.Idle:
                rb.velocity = Vector2.zero;
                break;
            case BossState.MovingRight:
                UpdateMoveRight();
                break;
            case BossState.MachineGun:
                UpdateMachineGun();
                break;
            case BossState.Resting:
                // Estado de descanso (no hace nada especial)
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

    private void StartResting()
    {
        isShootingEnabled = false;
        restTimer = restDuration;

        // Desactivar el weaponObject
        if (weaponObject != null)
        {
            weaponObject.SetActive(false);
        }

        // Detener la partícula de disparo si existe
        if (shootingParticle != null)
        {
            shootingParticle.Stop();
        }

        // Desactivar disparos
        if (BulletZoneController.Instance != null)
        {
            BulletZoneController.Instance.DisableShooting();
        }
    }

    private void ResumeShooting()
    {
        isShootingEnabled = true;
        shootingTimer = shootingDuration;

        // Reactivar elementos visuales
        if (weaponObject != null) weaponObject.SetActive(true);
        if (shootingParticle != null) shootingParticle.Play();

        // Reactivación de disparos
        if (BulletZoneController.Instance != null)
        {
            BulletZoneController.Instance.EnableShooting();
            BulletZoneController.Instance.RegenerateBulletInCurrentZone();
        }
    }

    // Método opcional para casos especiales (mantenemos por compatibilidad)
    public void EnableShootingManually()
    {
        if (BulletZoneController.Instance != null)
        {
            BulletZoneController.Instance.EnableShooting();
        }
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

    public void StartMachineGunPhase()
    {
        // Reactivar el boss y moverlo a la posición de punto1
        gameObject.SetActive(true);
        if (punto1 != null)
        {
            transform.position = punto1.position;
        }
        ChangeState(BossState.MachineGun);

        // Activar la BossCamera
        if (bossCamera != null)
        {
            bossCamera.enabled = true;
        }
    }

    private void UpdateMachineGun()
    {
        // Estado MachineGun sin funcionalidad de disparo
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