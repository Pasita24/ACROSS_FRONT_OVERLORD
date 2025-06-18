using UnityEngine;

public class GroundEffectMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;

    [HideInInspector] public Transform trench1Left;
    [HideInInspector] public Transform trench1Right;
    [HideInInspector] public Transform trench2Left;
    [HideInInspector] public Transform trench2Right;

    private bool movingRight = true;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        float direction = movingRight ? 1f : -1f;
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        // Lógica de trincheras (sin cambios)
        if (trench1Left && trench1Right)
        {
            if (movingRight && Mathf.Abs(transform.position.x - trench1Right.position.x) < 0.1f)
            {
                transform.position = new Vector3(trench1Left.position.x, transform.position.y, transform.position.z);
            }
            else if (!movingRight && Mathf.Abs(transform.position.x - trench1Left.position.x) < 0.1f)
            {
                transform.position = new Vector3(trench1Right.position.x, transform.position.y, transform.position.z);
            }
        }

        if (trench2Left && trench2Right)
        {
            if (movingRight && Mathf.Abs(transform.position.x - trench2Right.position.x) < 0.1f)
            {
                transform.position = new Vector3(trench2Left.position.x, transform.position.y, transform.position.z);
            }
            else if (!movingRight && Mathf.Abs(transform.position.x - trench2Left.position.x) < 0.1f)
            {
                transform.position = new Vector3(trench2Right.position.x, transform.position.y, transform.position.z);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Llamar al GameManager para activar el "Busted"
            GameManager.Instance.GameOver();
            Destroy(gameObject);
        }
        else if (other.CompareTag("limEnemy"))
        {
            movingRight = !movingRight;
        }
    }
}