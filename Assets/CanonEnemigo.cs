using System.Collections;
using UnityEngine;

public class CanonEnemigo : MonoBehaviour
{
    [Header("Disparo")]
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private float tiempoEntreDisparos = 2f;
    [SerializeField] private float rangoDeteccion = 10f;

    [Header("Vida")]
    [SerializeField] private float vida = 3f;
    [SerializeField] private float tiempoFlash = 0.1f;

    [Header("Referencias")]
    [SerializeField] private LayerMask capaJugador;
    private Transform objetivoJugador;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool puedeDisparar = true;
    private bool estaMuerto = false;

    private void Start()
    {
        objetivoJugador = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (estaMuerto || objetivoJugador == null) return;

        float distancia = Vector2.Distance(transform.position, objetivoJugador.position);

        if (distancia <= rangoDeteccion && puedeDisparar)
        {
            StartCoroutine(Disparar());
        }
    }

    private IEnumerator Disparar()
    {
        puedeDisparar = false;

        // Apunta al jugador
        Vector2 direccion = (objetivoJugador.position - puntoDisparo.position).normalized;
        GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
        bala.GetComponent<Rigidbody2D>().velocity = direccion * 5f; // puedes ajustar velocidad

        yield return new WaitForSeconds(tiempoEntreDisparos);
        puedeDisparar = true;
    }

    public void TomarDaño(float daño)
    {
        if (estaMuerto) return;

        vida -= daño;

        StartCoroutine(ParpadeoBlanco());

        if (vida <= 0)
        {
            estaMuerto = true;
            Morir();
        }
    }

    private IEnumerator ParpadeoBlanco()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(tiempoFlash);
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f); // Color normal
    }

    private void Morir()
    {
        if (animator != null)
        {
            animator.SetTrigger("Morir"); // Debe existir en tu Animator
        }

        StartCoroutine(DestruirLuego());
    }

    private IEnumerator DestruirLuego()
    {
        yield return new WaitForSeconds(1f); // Tiempo según animación de muerte
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}
