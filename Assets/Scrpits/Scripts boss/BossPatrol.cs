using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class BossPatrol : MonoBehaviour
{
    // Configuración de patrulla
    [Header("Patrol Settings")]
    [SerializeField] private float patrolSpeed = 3f; // Velocidad de movimiento entre puntos
    [SerializeField] private Transform[] patrolPoints; // Array de puntos a visitar en orden
    [SerializeField] private float waitTime = 1f; // Tiempo que espera en cada punto

    // Configuración de detección del jugador
    [Header("Player Detection")]
    [SerializeField] private LayerMask playerLayer; // Capa del jugador para el raycast
    [SerializeField] private LayerMask obstacleLayer; // Capa de obstáculos para el raycast
    [SerializeField] private float detectionRange = 5f; // Rango máximo de detección

    // Variables de estado
    private int currentPatrolPointIndex; // Índice del punto actual de patrulla
    private float currentWaitTime; // Tiempo restante de espera en el punto actual
    private Transform playerTransform; // Referencia al transform del jugador
    private BossController bossController; // Referencia al controlador principal del boss

    void Start()
    {
        // Inicialización de variables
        currentPatrolPointIndex = 0; // Comienza en el primer punto
        currentWaitTime = waitTime; // Configura el tiempo de espera inicial
        playerTransform = FindObjectOfType<MovimientoJugador>().transform;
        bossController = GetComponent<BossController>(); // Obtiene referencia al BossController
    }

    void Update()
    {
        // Cada frame actualiza el movimiento y la detección
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
    /// Detecta al jugador mediante raycast, considerando obstáculos
    /// </summary>
    void DetectPlayer()
    {
        if (playerTransform == null) return;

        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            directionToPlayer,
            detectionRange,
            playerLayer | obstacleLayer
        );
        UnityEngine.Debug.DrawRay(transform.position, directionToPlayer * detectionRange, Color.red);


        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                // ¡Jugador visible directamente!
                UnityEngine.Debug.Log("Jugador detectado. Reiniciando nivel.");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                // Hay un obstáculo entre el boss y el jugador → no hacer nada
                UnityEngine.Debug.Log("Jugador oculto detrás de un obstáculo. No detectado.");
            }
        }
    }



    /// <summary>
    /// Dibuja gizmos en el editor para visualizar la configuración
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Dibuja el rango de detección
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

                // Dibuja una línea al siguiente punto (excepto para el último)
                if (i < patrolPoints.Length - 1)
                {
                    Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
                }
            }
        }
    }
}