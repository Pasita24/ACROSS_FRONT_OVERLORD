using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonEstaticoIZQ : MonoBehaviour
{
    [Header("Disparo")]
    public Transform puntoDisparo;
    public GameObject balaPrefab;
    public float radioDeteccion = 10f;
    public LayerMask capaJugador;
    public float tiempoEntreDisparos = 2f;

    private float tiempoSiguienteDisparo = 0f;
    private Animator animator;
    private Transform jugador;

    private void Start()
    {
        animator = GetComponent<Animator>();
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
{
    if (jugador == null) return;

    float distancia = Vector2.Distance(transform.position, jugador.position);
    bool jugadorEnRango = distancia <= radioDeteccion;

    // Ver si el jugador está al frente (misma dirección en que mira el cañón)
    float direccionAlJugador = jugador.position.x - transform.position.x;
    bool jugadorAlFrente = (transform.localScale.x > 0 && direccionAlJugador < 0) ||
                       (transform.localScale.x < 0 && direccionAlJugador > 0);

    // Si está en rango y al frente
    if (jugadorEnRango && jugadorAlFrente && Time.time >= tiempoSiguienteDisparo)
    {
        animator.SetTrigger("Disparar");
        tiempoSiguienteDisparo = Time.time + tiempoEntreDisparos;
    }
}

    // Este método se llama desde un evento de la animación en el frame exacto del disparo
    public void Disparar()
    {
        Vector2 direccion = Vector2.left;

        GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
        bala.GetComponent<BalaCanon>()?.SetDireccion(direccion);
    }
}
