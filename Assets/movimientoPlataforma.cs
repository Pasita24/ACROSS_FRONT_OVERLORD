using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimientoPlataforma : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private float distancia;
    [SerializeField] private bool moviendoDerecha;

    private Rigidbody2D rb;
    private Animator animator;

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

        // Animación basada en velocidad horizontal
        if (animator != null)
        {
            animator.SetBool("Moviendo", Mathf.Abs(rb.velocity.x) > 0.01f);
        }
    }

    private void Girar()
    {
        moviendoDerecha = !moviendoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        velocidad *= -1;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(controladorSuelo.position, controladorSuelo.position + Vector3.down * distancia);
    }
}
