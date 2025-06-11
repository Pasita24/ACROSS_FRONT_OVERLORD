using UnityEngine;

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
        playerTransform = FindObjectOfType<PlayerController>().transform; // Encuentra al jugador
    }

    void Update()
    {
        PatrolMovement();
        DetectPlayer();
    }

    void PatrolMovement()
    {
        // Mover hacia el punto de patrulla actual
        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPatrolPointIndex].position, patrolSpeed * Time.deltaTime);

        // Si ha llegado al punto de patrulla
        if (Vector2.Distance(transform.position, patrolPoints[currentPatrolPointIndex].position) < 0.1f)
        {
            if (currentWaitTime <= 0)
            {
                currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length; // Siguiente punto
                // Determinar la dirección de movimiento para la rotación (si tu sprite rota)
                if (patrolPoints[currentPatrolPointIndex].position.x > transform.position.x)
                {
                    movingRight = true;
                }
                else
                {
                    movingRight = false;
                }
                currentWaitTime = waitTime; // Reiniciar tiempo de espera
            }
            else
            {
                currentWaitTime -= Time.deltaTime; // Esperar
            }
        }

        // Rotar el sprite del enemigo para que mire en la dirección del movimiento
        // 
        // if (movingRight && transform.localScale.x < 0 || !movingRight && transform.localScale.x > 0)
        // {
        //     transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        // }
    }

    void DetectPlayer()
    {
        // Raycast para detectar al jugador
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange, playerLayer | obstacleLayer);

        // Visualizar el raycast en el editor (solo para depuración)
        Debug.DrawRay(transform.position, directionToPlayer * detectionRange, Color.yellow);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                // Si el raycast golpeó al jugador directamente
                Debug.Log("Jugador detectado!");
                // Aquí puedes añadir la lógica para que el jugador pierda o se active una alarma
                // Por ejemplo:
                // GameManager.Instance.GameOver(); // Si tienes un GameManager
                // SceneManager.LoadScene("GameOverScene"); // Para reiniciar la escena
            }
            else if (hit.collider.CompareTag("Obstacle") && hit.collider.GetComponent<ObstacleController>() != null)
            {
                // Si el raycast golpeó un obstáculo ANTES de golpear al jugador
                // Y el obstáculo es un muro que puede ocultar al jugador
                float distanceToObstacle = Vector2.Distance(transform.position, hit.collider.transform.position);
                float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

                // Si el jugador está detrás del obstáculo y el obstáculo es un muro
                if (distanceToPlayer > distanceToObstacle && hit.collider.GetComponent<ObstacleController>().isHidingSpot)
                {
                    Debug.Log("Jugador oculto detrás del muro.");
                    // El jugador no es detectado
                }
                else if (hit.collider.CompareTag("Player"))
                {
                    // Esta condición es para el caso en que el raycast golpea directamente al jugador y no un obstáculo
                    Debug.Log("Jugador detectado!");
                    // Lógica de Game Over
                }
            }
        }
    }

    //Visualizar el rango de detección en el editor
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
}