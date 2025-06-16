using UnityEngine;

public class GroundEffectMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Velocidad del movimiento del efecto
    [SerializeField] private float lifetime = 5f; // Tiempo de vida del efecto
    [SerializeField] private float minX = -10f; // L�mite izquierdo
    [SerializeField] private float maxX = 10f;  // L�mite derecho

    private bool movingRight = true; // Direcci�n inicial del movimiento

    void Start()
    {
        Destroy(gameObject, lifetime); // Destruir el objeto despu�s de su tiempo de vida
    }

    void Update()
    {
        // Determinar direcci�n del movimiento
        float direction = movingRight ? 1f : -1f;

        // Mover el efecto
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        // Verificar l�mites
        if (transform.position.x >= maxX)
        {
            movingRight = false; // Cambiar direcci�n al llegar al l�mite derecho
        }
        else if (transform.position.x <= minX)
        {
            movingRight = true; // Cambiar direcci�n al llegar al l�mite izquierdo
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