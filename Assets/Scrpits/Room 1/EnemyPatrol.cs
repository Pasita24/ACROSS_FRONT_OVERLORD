using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float patrolSpeed = 3f; // Velocidad de patrulla
    [SerializeField] private Transform[] patrolPoints; // Puntos entre los que patrulla
    [SerializeField] private float waitTime = 1f; // Tiempo de espera en cada punto
    [SerializeField] private LayerMask playerLayer; // Layer del jugador para detecci�n
    [SerializeField] private float detectionRange = 3f; // Rango de detecci�n del jugador
    [SerializeField] private LayerMask obstacleLayer; // Layer de los muros/obst�culos

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
                // Determinar la direcci�n de movimiento para la rotaci�n (si tu sprite rota)
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

        // Rotar el sprite del enemigo para que mire en la direcci�n del movimiento
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

        // Visualizar el raycast en el editor (solo para depuraci�n)
        Debug.DrawRay(transform.position, directionToPlayer * detectionRange, Color.yellow);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                // Si el raycast golpe� al jugador directamente
                Debug.Log("Jugador detectado!");
                // Aqu� puedes a�adir la l�gica para que el jugador pierda o se active una alarma
                // Por ejemplo:
                // GameManager.Instance.GameOver(); // Si tienes un GameManager
                // SceneManager.LoadScene("GameOverScene"); // Para reiniciar la escena
                StartCoroutine(HandlePlayerDetection()); // Se llama a la corrutina de la detección del player

            }
            else if (hit.collider.CompareTag("Obstacle") && hit.collider.GetComponent<ObstacleController>() != null)
            {
                // Si el raycast golpe� un obst�culo ANTES de golpear al jugador
                // Y el obst�culo es un muro que puede ocultar al jugador
                float distanceToObstacle = Vector2.Distance(transform.position, hit.collider.transform.position);
                float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

                // Si el jugador est� detr�s del obst�culo y el obst�culo es un muro
                if (distanceToPlayer > distanceToObstacle && hit.collider.GetComponent<ObstacleController>().isHidingSpot)
                {
                    Debug.Log("Jugador oculto detr�s del muro.");
                    // El jugador no es detectado
                }
                else if (hit.collider.CompareTag("Player"))
                {
                    // Esta condicion es para el caso en que el raycast golpea directamente al jugador y no un obstaculo
                    Debug.Log("Jugador detectado!");
                    // Luego aqui implementar lógica de gameOver
                }
            }
        }
    }
    //Visualizar el rango de detecci�n en el editor
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
        Debug.Log("Jugador detectado: congelando y reiniciando en 2 segundos");

        // Con esto se detiiene el player
        var playerController = playerTransform.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.OnDetected();
        }
        this.enabled = false;
        UIController.Instance.ShowBustedMessage();

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Utilicé buildIndex ya que la idea es reiniciar la escena actual
    }

}
