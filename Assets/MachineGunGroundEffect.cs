using UnityEngine;

public class GroundEffectMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Velocidad del movimiento del efecto
    [SerializeField] private float lifetime = 5f; // Tiempo de vida del efecto
    [SerializeField] private float minX = -10f; // Límite izquierdo
    [SerializeField] private float maxX = 10f;  // Límite derecho

    private bool movingRight = true; // Dirección inicial del movimiento

    void Start()
    {
        Destroy(gameObject, lifetime); // Destruir el objeto después de su tiempo de vida
    }

    void Update()
    {
        // Determinar dirección del movimiento
        float direction = movingRight ? 1f : -1f;

        // Mover el efecto
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        // Verificar límites
        if (transform.position.x >= maxX)
        {
            movingRight = false; // Cambiar dirección al llegar al límite derecho
        }
        else if (transform.position.x <= minX)
        {
            movingRight = true; // Cambiar dirección al llegar al límite izquierdo
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        UnityEngine.Debug.Log($"Collided with: {other.gameObject.name}, Tag: {other.tag}");
        if (other.CompareTag("Player"))
        {
            UnityEngine.Debug.Log("Player hit by ground effect!");
            Destroy(gameObject);
        }
        else if (other.CompareTag("limEnemy"))
        {
            UnityEngine.Debug.Log("Collided with limEnemy!");
            movingRight = !movingRight;
        }
    }
}