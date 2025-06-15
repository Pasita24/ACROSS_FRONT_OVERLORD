using UnityEngine;

public class GroundEffectMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Velocidad del movimiento del efecto
    [SerializeField] private float lifetime = 5f; // Tiempo de vida del efecto

    void Start()
    {
        Destroy(gameObject, lifetime); // Destruir el objeto después de su tiempo de vida
    }

    void Update()
    {
        // Mover el efecto de izquierda a derecha
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    // Detectar colisión con el jugador
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UnityEngine.Debug.Log("Player hit by ground effect!");
            Destroy(gameObject); // Destruir el efecto al colisionar
        }
    }
}