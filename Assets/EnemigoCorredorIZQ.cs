using UnityEngine;

public class EnemigoCorredorIZQ : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private float distancia;
    [SerializeField] private bool moviendoizquierda;

    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private float dañoAlJugador = 1f; // Cantidad de daño al colisionar

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        RaycastHit2D informacionSuelo = Physics2D.Raycast(controladorSuelo.position, Vector2.down, distancia);

        rb.velocity = new Vector2(velocidad, rb.velocity.y);

        if (informacionSuelo == false)
        {
            Girar();
        }

        if (animator != null)
        {
            animator.SetBool("Moviendo", Mathf.Abs(rb.velocity.x) > 0.01f);
        }
    }

    private void Girar()
    {
        moviendoizquierda = !moviendoizquierda;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y - 180, 0);
        velocidad *= 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.collider.CompareTag("Player"))
    {
        MovimientoJugador jugador = collision.collider.GetComponent<MovimientoJugador>();
        if (jugador != null)
        {
            jugador.TomarDaño(dañoAlJugador); // Le quita toda la vida si así lo deseas
        }
    }
}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(controladorSuelo.position, controladorSuelo.position + Vector3.down * distancia);
    }
}
