using UnityEngine;

public class BalaTanque : MonoBehaviour
{
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float daño = 1f;
    [SerializeField] private float tiempoVida = 5f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Destruir la bala después de un tiempo por seguridad
        Destroy(gameObject, tiempoVida);
    }

    public void DispararEnDireccion(Vector2 direccion)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = direccion.normalized * velocidad;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MovimientoJugador jugador = collision.GetComponent<MovimientoJugador>();
            if (jugador != null)
            {
                jugador.TomarDaño(daño);
            }

            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
