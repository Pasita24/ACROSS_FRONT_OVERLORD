using System.Diagnostics;
using UnityEngine;

public class BossPatrol : MonoBehaviour
{
    // Configuraci�n de patrulla
    [Header("Patrol Settings")]
    [SerializeField] private float patrolSpeed = 3f; // Velocidad de movimiento entre puntos
    [SerializeField] private Transform[] patrolPoints; // Array de puntos a visitar en orden
    [SerializeField] private float waitTime = 1f; // Tiempo que espera en cada punto

    // Configuraci�n de detecci�n del jugador
    [Header("Player Detection")]
    [SerializeField] private LayerMask playerLayer; // Capa del jugador para el raycast
    [SerializeField] private LayerMask obstacleLayer; // Capa de obst�culos para el raycast
    [SerializeField] private float detectionRange = 5f; // Rango m�ximo de detecci�n

    // Variables de estado
    private int currentPatrolPointIndex; // �ndice del punto actual de patrulla
    private float currentWaitTime; // Tiempo restante de espera en el punto actual
    private Transform playerTransform; // Referencia al transform del jugador
    private BossController bossController; // Referencia al controlador principal del boss

    void Start()
    {
        // Inicializaci�n de variables
        currentPatrolPointIndex = 0; // Comienza en el primer punto
        currentWaitTime = waitTime; // Configura el tiempo de espera inicial
        playerTransform = FindObjectOfType<PlayerController>().transform; // Busca al jugador en la escena
        bossController = GetComponent<BossController>(); // Obtiene referencia al BossController
    }

    void Update()
    {
        // Cada frame actualiza el movimiento y la detecci�n
        PatrolMovement();
        DetectPlayer();
    }

    /// <summary>
    /// Maneja el movimiento entre puntos de patrulla
    /// </summary>
    void PatrolMovement()
    {
        // Mueve al boss hacia el punto actual con suavidad
        transform.position = Vector2.MoveTowards(
            transform.position,
            patrolPoints[currentPatrolPointIndex].position,
            patrolSpeed * Time.deltaTime
        );

        // Comprueba si ha llegado al punto actual
        if (Vector2.Distance(transform.position, patrolPoints[currentPatrolPointIndex].position) < 0.1f)
        {
            if (currentWaitTime <= 0)
            {
                // Avanza al siguiente punto (con wrap-around al final del array)
                currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
                currentWaitTime = waitTime; // Reinicia el contador de espera
            }
            else
            {
                // Reduce el tiempo de espera restante
                currentWaitTime -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Detecta al jugador mediante raycast, considerando obst�culos
    /// </summary>
    void DetectPlayer()
    {
        // Si no hay referencia al jugador, salir
        if (playerTransform == null) return;

        // Calcula direcci�n normalizada al jugador
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // Dispara un raycast que detecte jugador u obst�culos
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            directionToPlayer,
            detectionRange,
            playerLayer | obstacleLayer
        );

        // Dibuja el raycast en el editor (solo para depuraci�n)
       // Debug.DrawRay(transform.position, directionToPlayer * detectionRange, Color.yellow);

        // Si el raycast golpea al jugador directamente
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            // Inicia la fase de combate del boss
            //bossController.StartMachineGunPhase();
        }
    }

    /// <summary>
    /// Dibuja gizmos en el editor para visualizar la configuraci�n
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Dibuja el rango de detecci�n
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Dibuja los puntos de patrulla y las conexiones entre ellos
        if (patrolPoints != null)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                // Dibuja una esfera en cada punto
                Gizmos.DrawWireSphere(patrolPoints[i].position, 0.2f);

                // Dibuja una l�nea al siguiente punto (excepto para el �ltimo)
                if (i < patrolPoints.Length - 1)
                {
                    Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
                }
            }
        }
    }
}