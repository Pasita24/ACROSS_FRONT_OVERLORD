using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonEstatico : MonoBehaviour
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

        // Mira hacia el jugador
        if (jugadorEnRango)
        {
            transform.localScale = new Vector3(jugador.position.x < transform.position.x ? -1 : 1, 1, 1);
        }

        // Si el jugador está en rango y ha pasado el tiempo
        if (jugadorEnRango && Time.time >= tiempoSiguienteDisparo)
        {
            animator.SetTrigger("Disparar");
            tiempoSiguienteDisparo = Time.time + tiempoEntreDisparos;
        }
    }

    // Este método se llama desde un evento de la animación en el frame exacto del disparo
    public void Disparar()
    {
        Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
    }
}



