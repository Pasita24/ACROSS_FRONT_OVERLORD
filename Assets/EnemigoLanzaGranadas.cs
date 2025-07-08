using UnityEngine;

public class EnemigoLanzaGranadas : MonoBehaviour
{
    public Transform controladorLanzamiento;
    public GameObject prefabGranada;
    public float radioDeteccion = 8f;
    public float fuerzaHorizontal = 5f;
    public float fuerzaVertical = 7f;
    public float tiempoEntreLanzamientos = 3f;

    private float tiempoUltimoLanzamiento;
    private Transform jugador;
    private Animator animator;

    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (jugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (distancia <= radioDeteccion && Time.time > tiempoUltimoLanzamiento + tiempoEntreLanzamientos)
        {
            LanzarGranada();
            tiempoUltimoLanzamiento = Time.time;
        }
    }

    private void LanzarGranada()
    {
        // Activar animación si la tienes
        /*if (animator != null)
            animator.SetTrigger("Lanzar");*/

        // Dirección hacia el jugador
        Vector2 direccion = (jugador.position - controladorLanzamiento.position).normalized;

        GameObject granada = Instantiate(prefabGranada, controladorLanzamiento.position, Quaternion.identity);

        Rigidbody2D rb = granada.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 fuerza = new Vector2(direccion.x * fuerzaHorizontal, fuerzaVertical);
            rb.AddForce(fuerza, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
    }
}
