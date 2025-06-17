using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("Patrullaje")]
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitTime = 1f;

    [Header("Detección")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float detectionRange = 5f;

    private int currentPatrolPointIndex = 0;
    private float currentWaitTime;
    private Transform playerTransform;
    private bool playerInSight = false;
    private bool canMove = true;

    public bool PlayerInSight => playerInSight; // Propiedad pública si otro script necesita saberlo

    void Start()
    {
        currentWaitTime = waitTime;
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (!playerInSight && canMove)
        {
            PatrolMovement();
        }

        DetectPlayer();
    }

    void PatrolMovement()
    {
        if (patrolPoints.Length == 0) return;

        Vector3 target = patrolPoints[currentPatrolPointIndex].position;
        Vector3 moveDirection = target - transform.position;

        // Invertir la lógica: positivo para izquierda, negativo para derecha
        if (moveDirection.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Sign(moveDirection.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        // Movimiento hacia el punto actual
        transform.position = Vector2.MoveTowards(
            transform.position,
            target,
            patrolSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            if (currentWaitTime <= 0)
            {
                currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
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
        if (playerTransform == null) return;

        Vector2 direction = (playerTransform.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            detectionRange,
            playerLayer | obstacleLayer
        );

        Debug.DrawRay(transform.position, direction * detectionRange, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            playerInSight = true;
        }
        else
        {
            playerInSight = false;
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

    public void SetCanMove(bool value)
    {
        canMove = value;
    }
}
