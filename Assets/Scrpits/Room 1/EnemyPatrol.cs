using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float patrolSpeed = 3f; // Velocidad de patrulla
    [SerializeField] private Transform[] patrolPoints; // Puntos entre los que patrulla
    [SerializeField] private float waitTime = 1f; // Tiempo de espera en cada punto
    [SerializeField] private LayerMask playerLayer; // Layer del jugador para detección
    [SerializeField] private float detectionRange = 3f; // Rango de detección del jugador
    [SerializeField] private LayerMask obstacleLayer; // Layer de los muros/obstáculos

    private int currentPatrolPointIndex;
    private float currentWaitTime;
    private bool movingRight = true; // Para saber si se mueve a la derecha o izquierda
    private Transform playerTransform; // Referencia al transform del jugador

    void Start()
    {
        currentPatrolPointIndex = 0;
        currentWaitTime = waitTime;
        playerTransform = FindObjectOfType<MovimientoJugador>().transform; // Encuentra al jugador
    }

    void Update()
    {
        PatrolMovement();
        DetectPlayer();
    }

    void PatrolMovement()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            return; // No hay puntos de patrulla, el enemigo permanece estático
        }

        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPatrolPointIndex].position, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, patrolPoints[currentPatrolPointIndex].position) < 0.1f)
        {
            if (currentWaitTime <= 0)
            {
                currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
                if (patrolPoints[currentPatrolPointIndex].position.x > transform.position.x)
                {
                    movingRight = true;
                }
                else
                {
                    movingRight = false;
                }
                currentWaitTime = waitTime;
            }
            else
            {
                currentWaitTime -= Time.deltaTime;
            }
        }
    }

    void DetectPlayer()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // Raycast para buscar obstáculos o jugador
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange, playerLayer | obstacleLayer);

        Debug.DrawRay(transform.position, directionToPlayer * detectionRange, Color.yellow);

        if (hit.collider != null)
        {
            // Si el raycast golpea directamente al jugador, es detectado
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Jugador detectado!");
                // Asegurarse de que el jugador esté realmente dentro del rango de detección y no detrás de un lugar para esconderse
                if (Vector2.Distance(transform.position, playerTransform.position) <= detectionRange)
                {
                    StartCoroutine(HandlePlayerDetection());
                }
            }
            // Si el raycast golpea un obstáculo
            else if (hit.collider.CompareTag("Obstacle"))
            {
                ObstacleController obstacle = hit.collider.GetComponent<ObstacleController>();
                // Verificar si el obstáculo es un lugar para esconderse y está entre el enemigo y el jugador
                if (obstacle != null && obstacle.isHidingSpot)
                {
                    // Ahora, lanza otro rayo *solo* para el jugador, comenzando desde la posición del obstáculo
                    // Esto verifica si el jugador está realmente detrás del obstáculo desde la perspectiva del enemigo.
                    RaycastHit2D playerBehindObstacleHit = Physics2D.Raycast(hit.point + directionToPlayer * 0.01f, directionToPlayer, detectionRange - hit.distance, playerLayer);

                    if (playerBehindObstacleHit.collider != null && playerBehindObstacleHit.collider.CompareTag("Player"))
                    {
                        Debug.Log("Jugador está oculto detrás del punto de escondite.");
                        // El jugador está oculto, no activar la detección
                    }
                    else
                    {
                        Debug.Log("Obstáculo (no es un punto de escondite, o el jugador no está detrás). Jugador potencial detectado.");
                        // Si el obstáculo es un lugar para esconderse pero el jugador NO está detrás de él,
                        // o si no es un lugar para esconderse en absoluto, y el jugador *está* dentro del rango
                        // pero no fue golpeado por el rayo inicial debido a este obstáculo, debemos tener cuidado.
                        // La detección principal del jugador debe ser manejada por el golpe directo a la etiqueta "Player".
                        // Este bloque 'else' aquí principalmente captura casos donde un obstáculo es golpeado pero el jugador no está oculto.
                        // Para la detección directa del jugador, el primer 'if (hit.collider.CompareTag("Player"))' tiene prioridad.
                    }
                }
                else
                {
                    Debug.Log("Obstáculo (no es un punto de escondite).");
                    // Si es solo un obstáculo normal y no un lugar para esconderse, entonces simplemente bloquea la vista.
                    // El jugador no será detectado si este obstáculo está en el camino.
                }
            }
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (patrolPoints != null)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                Gizmos.DrawWireSphere(patrolPoints[i].position, 0.2f);
                if (i < patrolPoints.Length - 1)
                {
                    Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
                }
            }
        }
    }

    private IEnumerator HandlePlayerDetection()
    {
 

        // El PlayerController no existe, debería ser MovimientoJugador
        var playerController = playerTransform.GetComponent<MovimientoJugador>();
        if (playerController != null)
        {
            playerController.TomarDaño(1f);
            playerController.PausarMovimiento();

        }
        this.enabled = false;
        yield return new WaitForSeconds(1000000000f);

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}