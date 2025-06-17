using UnityEngine;

public class GroundEffectMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Velocidad de la bala
    [SerializeField] private float lifetime = 5f; // Tiempo de vida
    [SerializeField] private Transform spawnPointZone1; // Punto de inicio en Zona 1
    [SerializeField] private Transform spawnPointZone2; // Punto de inicio en Zona 2
    [SerializeField] private Transform trench1Left; // Borde izquierdo trinchera 1
    [SerializeField] private Transform trench1Right; // Borde derecho trinchera 1
    [SerializeField] private Transform trench2Left; // Borde izquierdo trinchera 2
    [SerializeField] private Transform trench2Right; // Borde derecho trinchera 2

    private bool movingRight = true;
    private bool isActive = false;
    private string currentZone = ""; // "" = ninguna zona, "Zone1" o "Zone2"
    private Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        Destroy(gameObject, lifetime);
        gameObject.SetActive(false); // Inactivo al inicio
    }

    void Update()
    {
        if (!isActive) return;

        float direction = movingRight ? 1f : -1f;
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        // Teletransporte en trincheras
        if (currentZone == "Zone1")
        {
            if (movingRight && Mathf.Abs(transform.position.x - trench1Right.position.x) < 0.1f)
            {
                transform.position = new Vector3(trench1Left.position.x, transform.position.y, transform.position.z);
                UnityEngine.Debug.Log("Teletransportado de borde derecho a borde izquierdo de trinchera 1");
            }
            else if (!movingRight && Mathf.Abs(transform.position.x - trench1Left.position.x) < 0.1f)
            {
                transform.position = new Vector3(trench1Right.position.x, transform.position.y, transform.position.z);
                UnityEngine.Debug.Log("Teletransportado de borde izquierdo a borde derecho de trinchera 1");
            }
        }
        else if (currentZone == "Zone2")
        {
            if (movingRight && Mathf.Abs(transform.position.x - trench2Right.position.x) < 0.1f)
            {
                transform.position = new Vector3(trench2Left.position.x, transform.position.y, transform.position.z);
                UnityEngine.Debug.Log("Teletransportado de borde derecho a borde izquierdo de trinchera 2");
            }
            else if (!movingRight && Mathf.Abs(transform.position.x - trench2Left.position.x) < 0.1f)
            {
                transform.position = new Vector3(trench2Right.position.x, transform.position.y, transform.position.z);
                UnityEngine.Debug.Log("Teletransportado de borde izquierdo a borde derecho de trinchera 2");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UnityEngine.Debug.Log("Player golpeado por la bala!");
            Destroy(gameObject); // Opcional: añadir lógica para matar al jugador
        }
        else if (other.CompareTag("limEnemy"))
        {
            movingRight = !movingRight; // Rebotar al tocar los bordes de la zona
            UnityEngine.Debug.Log($"Rebotando en borde de {other.tag}");
        }
    }

    // Métodos públicos para ser llamados desde el script del jugador o un manager
    public void ActivateInZone(string zone, Vector3 spawnPoint)
    {
        currentZone = zone;
        isActive = true;
        gameObject.SetActive(true);
        transform.position = spawnPoint;
        movingRight = true; // Siempre inicia hacia la derecha
        UnityEngine.Debug.Log($"Bala activada en {zone} en posición {spawnPoint}");
    }

    public void Deactivate()
    {
        isActive = false;
        currentZone = "";
        gameObject.SetActive(false);
        UnityEngine.Debug.Log("Bala desactivada");
    }
}